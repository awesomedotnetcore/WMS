<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-dropdown-button v-if="storage.IsTray && hasPerm('TD_InStorage.Add')" type="primary" @click="hanldleAdd()">
        <a-icon type="plus" />新建
        <a-menu v-if="hasPerm('TD_InStorage.Tray')" slot="overlay" @click="handleAddMenuClick">
          <a-menu-item key="Tray">
            <a-icon type="border" />空托盘入库</a-menu-item>
        </a-menu>
      </a-dropdown-button>
      <a-button v-else-if="hasPerm('TD_InStorage.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('TD_InStorage.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
      <a-divider type="vertical" />
      <a-radio-group v-model="queryParam.Status" button-style="solid" @change="getDataList">
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
              <enum-select code="InStorageType" v-model="queryParam.InType"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="入库/关联单号" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-range-picker @change="onInStorTimeChange" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = { Status: null})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="InType" slot-scope="text">
        <enum-name code="InStorageType" :value="text"></enum-name>
      </template>
      <template slot="Status" slot-scope="text, record">
        <a-tag v-if="record.Status===0" color="blue">待审核</a-tag>
        <a-tag v-else-if="record.Status===1" color="green">审核通过</a-tag>
        <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
        <a-tag v-else color="blue">待审核</a-tag>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleShow(record.Id)">查看</a>
          <a-divider v-if="hasPerm('TD_InStorage.Edit') && record.Status===0" type="vertical" />
          <a v-if="hasPerm('TD_InStorage.Edit') && record.Status===0" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('TD_InStorage.Delete') && record.Status===0" type="vertical" />
          <a v-if="hasPerm('TD_InStorage.Delete') && record.Status===0" @click="handleDelete([record.Id])">删除</a>
          
          <!-- <a v-if="hasPerm('TD_InStorage.Auditing') || record.Status === 1" @click="handleShow(record.Id)">{{ record.Status === 0?'审核':'查看' }}</a> -->
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this"></edit-form>
    <in-tray ref="inTray"></in-tray>
  </a-card>
</template>

<script>
import moment from 'moment'
import EditForm from './EditForm'
import InTray from './InBlankTray'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'

const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '入库单号', dataIndex: 'Code' },
  { title: '入库类型', dataIndex: 'InType', scopedSlots: { customRender: 'InType' } },
  { title: '入库时间', dataIndex: 'InStorTime', customRender: filterDate },
  { title: '关联单号', dataIndex: 'RefCode' },
  { title: '状态', dataIndex: 'Status', scopedSlots: { customRender: 'Status' } },
  { title: '供应商', dataIndex: 'Supplier.Name' },
  { title: '入库数量', dataIndex: 'TotalNum' },
  { title: '制单人', dataIndex: 'CreateUser.RealName' },
  { title: '审核人', dataIndex: 'AuditUser.RealName' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    EnumSelect,
    InTray
  },
  mounted() {
    this.getCurStorage()
    this.getDataList()
  },
  data() {
    return {
      storage: {},
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'CreateTime', order: 'desc' },
      loading: false,
      columns,
      queryParam: { Status: null },
      selectedRowKeys: [],
      disabled: false
    }
  },
  methods: {
    moment,
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
        .post('/TD/TD_InStorage/GetDataList', {
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
    onSelectChange(selectedRowKeys, selectedRows) {
      var ids = []
      selectedRows.forEach((val, index, arr) => {
        if (val.Status === 0) ids.push(val.Id)
      })
      this.selectedRowKeys = ids
    },
    onInStorTimeChange(dates, dateStrings) {
      this.queryParam.InStorTimeStart = dateStrings[0]
      this.queryParam.InStorTimeEnd = dateStrings[1]
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.disabled = false
      this.$refs.editForm.openForm()
    },
    handleAddMenuClick(e) {
      console.log('handleAddMenuClick', e)
      if (e.key === 'Tray') {
        this.$refs.inTray.openForm()
      }
    },
    handleEdit(id) {
      this.disabled = false
      this.$refs.editForm.openForm(id)
    },
    handleShow(id) {
      this.disabled = true
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_InStorage/DeleteData', ids).then(resJson => {
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
    }
  }
}
</script>
<style>
.ant-btn-group > .ant-btn:first-child:not(:last-child) {
  margin-right: 0px;
}
</style>