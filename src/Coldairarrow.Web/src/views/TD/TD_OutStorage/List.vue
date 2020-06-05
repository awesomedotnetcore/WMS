<template>
  <a-card :bordered="false">
    

    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
       <a-row :gutter="10">
        <!--<a-col :md="8" :sm="24">
            <a-form-item>
              <a-radio-group v-model="queryParam.Status" :default-value="-1" button-style="solid" @change="getDataList">
                <a-radio-button :value="-1">
                  全部
                </a-radio-button>
                <a-radio-button :value="0">
                  待审核
                </a-radio-button>
                <a-radio-button :value="1">
                  审核通过
                </a-radio-button>
                <a-radio-button :value="2">
                  审核失败
                </a-radio-button>
              </a-radio-group>
            </a-form-item>
          </a-col> 
          </a-row> -->
          <!-- <a-row > -->
          <!-- <a-col :md="4" :sm="24">
            <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
            <a-button type="primary"
              icon="minus"
              @click="handleDelete(selectedRowKeys)"
              :disabled="!hasSelected()"
              :loading="loading"
            >删除</a-button>
            <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>            
          </a-col> -->
            <a-col :md="4" :sm="24">
              <a-form-item>
                <enum-select code="OutStorageType" v-model="queryParam.OutType"></enum-select>
              </a-form-item>
            </a-col>
            <a-col :md="4" :sm="24">
              <a-form-item>
                <a-input v-model="queryParam.Code" placeholder="出库单号" />
              </a-form-item>
            </a-col>
            <a-col :md="4" :sm="24">
              <a-form-item>
                <a-range-picker @change="onOutStorTimeChange" />
              </a-form-item>
            </a-col>
            <a-col :md="2" :sm="24">
            <a-form-item>
              <a-select v-model="queryParam.Status" placeholder="状态" :allowClear="true">
                <a-select-option :key="-1" :value="-1">全部</a-select-option>
                <a-select-option :key="0" :value="0">待审核</a-select-option>
                <a-select-option :key="1" :value="1">审核通过</a-select-option>
                <a-select-option :key="2" :value="2">审核失败</a-select-option>
              </a-select>
            </a-form-item>
            </a-col>

            <a-col :md="4" :sm="24">
              <a-button type="primary" @click="getDataList">查询</a-button>
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
        <a-tag v-if="record.Status===0" color="green">待审核</a-tag>
        <a-tag v-else-if="record.Status===1" color="#87d068">审核通过</a-tag>
        <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
        <a-tag v-else color="green">待审核</a-tag>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleAudite(record.Id)">{{ record.Status === 0?'审核':'查看' }}</a>
          <a-divider type="vertical" />
          <a v-if="record.Status===0" @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a v-if="record.Status===0" @click="handleDelete([record.Id])">删除</a>          
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import moment from 'moment'
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'

const filterDate = (value, row, index) => {
  return moment(value).format('YYYY-MM-DD')
}

const columns = [
  // { title: '仓库ID', dataIndex: 'StorageId', width: '10%' },
  // { title: '关联单号', dataIndex: 'RefCode', width: '10%' },
  // { title: '总金额', dataIndex: 'TotalAmt', width: '10%' },
  // { title: '设备ID', dataIndex: 'EquId', width: '10%' },
  // { title: '备注', dataIndex: 'Remarks', width: '10%' },
  { title: '出库单号', dataIndex: 'Code', width: '15%' },
  { title: '出库时间', dataIndex: 'OutTime', width: '10%' , customRender: filterDate },
  { title: '出库类型', dataIndex: 'OutType', width: '8%' , scopedSlots: { customRender: 'OutType' } },  
  { title: '出库数量', dataIndex: 'OutNum', width: '8%' },  
  { title: '状态', dataIndex: 'Status', width: '7%', scopedSlots: { customRender: 'Status' }  },
  { title: '客户', dataIndex: 'Customer.Name', width: '10%' },
  // { title: '客户地址', dataIndex: 'Address.Address', width: '10%' },  
  { title: '制单人', dataIndex: 'CreateUser.RealName', width: '10%' },
  { title: '审核人', dataIndex: 'AuditUser.RealName', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName, 
    EnumSelect,
  },
  mounted() {
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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      disabled: false,
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
  }
}
</script>