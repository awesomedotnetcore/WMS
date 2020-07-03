<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-dropdown-button v-if="storage.IsTray && hasPerm('TD_OutStorage.Add')" type="primary" @click="hanldleAdd()">
        <a-icon type="plus" />新建
        <a-menu v-if="hasPerm('TD_OutStorage.Tray')" slot="overlay" @click="handleAddMenuClick">
          <a-menu-item key="Tray">
            <a-icon type="border" />空托盘出库</a-menu-item>
        </a-menu>
      </a-dropdown-button>
      <a-button v-else-if="hasPerm('TD_OutStorage.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" v-if="hasPerm('TD_OutStorage.Delete')" 
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
      <a-divider type="vertical" />
      <a-radio-group v-model="queryParam.Status" :default-value="null" button-style="solid" @change="getDataList">
        <a-radio-button :value="null">全部</a-radio-button>
        <a-radio-button :value="0">待审核</a-radio-button>
        <a-radio-button :value="1">审核通过</a-radio-button>
        <a-radio-button :value="2">审核失败</a-radio-button>
      </a-radio-group>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
       <a-row :gutter="10">       
            <a-col :md="4" :sm="24">
              <a-form-item>
                <enum-select code="OutStorageType" v-model="queryParam.OutType"></enum-select>
              </a-form-item>
            </a-col>
            <a-col :md="4" :sm="24">
              <a-form-item>
                <a-input v-model="queryParam.Code" placeholder="出库/关联单号" />
              </a-form-item>
            </a-col>
            <a-col :md="5" :sm="24">
              <a-form-item>
                <a-range-picker @change="onOutStorTimeChange" />
              </a-form-item>
            </a-col>
            <a-col :md="4" :sm="24">
              <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
              <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
            </a-col>
        </a-row>        
      </a-form>
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.Id"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <template slot="OutType" slot-scope="text">
        <enum-name code="OutStorageType" :value="text"></enum-name>
      </template>

      <template slot="Status" slot-scope="text, record">
        <a-tag v-if="record.Status===0" color="blue">待审核</a-tag>
        <a-tag v-else-if="record.Status===1" color="green">审核通过</a-tag>
        <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
        <a-tag v-else color="blue">待审核</a-tag>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleAudite(record.Id)">查看</a>
          <!-- {{ record.Status === 0?'审核':'查看' }} -->
          <a-divider v-if="record.Status===0 && hasPerm('TD_OutStorage.Edit')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_OutStorage.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_OutStorage.Delete')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_OutStorage.Delete')" @click="handleDelete([record.Id])">删除</a>          
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this"></edit-form>
    <out-tray ref="outTray"></out-tray>
  </a-card>
</template>

<script>
import moment from 'moment'
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import OutTray from './OutBlankTray'

const filterDate = (value, row, index) => {
  return moment(value).format('YYYY-MM-DD')
}

const columns = [
  { title: '出库单号', dataIndex: 'Code' },
  { title: '出库时间', dataIndex: 'OutTime', customRender: filterDate },
  { title: '出库类型', dataIndex: 'OutType' , scopedSlots: { customRender: 'OutType' } },  
  { title: '出库数量', dataIndex: 'OutNum' },  
  { title: '状态', dataIndex: 'Status', scopedSlots: { customRender: 'Status' }  },
  { title: '客户', dataIndex: 'Customer.Name'},
  { title: '制单人', dataIndex: 'CreateUser.RealName'},
  { title: '审核人', dataIndex: 'AuditUser.RealName'},
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName, 
    EnumSelect,
    OutTray
  },
  mounted() {
    this.getCurStorage()
    this.getDataList()
  },
  data() {
    return {
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'OutTime', order: 'desc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      disabled: false,
      storage: {},
    }
  },
  methods: {
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getDataList() {
      this.selectedRowKeys = []

      this.loading = true
      this.$http
        .post('/TD/TD_OutStorage/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then(resJson => {
          this.loading = false
          this.data = resJson.Data
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    },
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
       this.disabled = false
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
       this.disabled = false
      this.$refs.editForm.openForm(id)
    },
    handleAudite(id) {
      this.disabled = true
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_OutStorage/DeleteData', ids).then(resJson => {
              resolve()

              if (resJson.Success) {
                thisObj.$message.success('操作成功!')

                thisObj.getDataList()
              } else {
                thisObj.$message.error(resJson.Msg)
              }
            })
          })
        }
      })
    },
    onOutStorTimeChange(dates, dateStrings) {
      this.queryParam.OutStorTimeStart = dateStrings[0]
      this.queryParam.OutStorTimeEnd = dateStrings[1]
    },
    handleAddMenuClick(e) {
      console.log('handleAddMenuClick', e)
      if (e.key === 'Tray') {
        this.$refs.outTray.openForm()
      }
    },

  }
}
</script>
<style>
.ant-btn-group > .ant-btn:first-child:not(:last-child) {
  margin-right: 0px;
}
</style>