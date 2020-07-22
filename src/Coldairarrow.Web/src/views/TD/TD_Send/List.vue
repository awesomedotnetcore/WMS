<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>

      <a-radio-group v-model="queryParam.Status" :default-value="null" button-style="solid" @change="getDataList">
        <a-radio-button :value="null">全部</a-radio-button>
        <a-radio-button :value="0">编制中</a-radio-button>
        <a-radio-button :value="1">已确认</a-radio-button>
        <a-radio-button :value="3">审核通过</a-radio-button>
        <a-radio-button :value="4">审核失败</a-radio-button>
        <a-radio-button :value="5">部分出库</a-radio-button>
        <a-radio-button :value="6">全部出库</a-radio-button>
      </a-radio-group>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10"> 
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="单号/客户" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="SendType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="5" :sm="24">
            <a-form-item>
              <a-range-picker @change="onOrderTimeChange" />
            </a-form-item>
          </a-col>         
          <!-- <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="关键字" />
            </a-form-item>
          </a-col> -->
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {Status: null})">重置</a-button>
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

      <template slot="SendType" slot-scope="text">
        <enum-name code="SendType" :value="text"></enum-name>
      </template>

      <template slot="Status" slot-scope="text, record">
        <a-tag v-if="record.Status===0" color="blue">编制中</a-tag>
        <a-tag v-else-if="record.Status===1" color="blue">已确认</a-tag>
        <a-tag v-else-if="record.Status===3" color="green">审核通过</a-tag>
        <a-tag v-else-if="record.Status===4" color="red">审核失败</a-tag>
        <a-tag v-else-if="record.Status===5" color="green">部分出库</a-tag>
        <a-tag v-else-if="record.Status===6" color="green">全部出库</a-tag>
        <a-tag v-else color="blue">编制中</a-tag>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="record.Status===0 && hasPerm('TD_Send.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Send.Delete')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_Send.Delete')" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Send.Delete')" type="vertical" />
          <a v-if="record.Status===0" @click="handleApproval(record.Id)">确认</a>  

           <a-divider v-if="(record.Status===3 || record.Status===5) && hasPerm('TD_Send.OutStorage')" type="vertical" />          
          <a v-if="(record.Status===3 || record.Status===5) && hasPerm('TD_Send.OutStorage')" @click="handleOutStorage(record.Id)">出库</a>
          <a-divider v-if="(record.Status===3 || record.Status===5) && hasPerm('TD_Send.OutStorage')" type="vertical" /> 
          
          <a v-if="record.Status>0" @click="handleApproval(record.Id)">{{ record.Status>=3?'查看':'审批' }}</a>  
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this" ></edit-form>
    <out-storage ref="outStorage" :parentObj="this"></out-storage>
  </a-card>
</template>

<script>
import moment from 'moment'
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import OutStorage from '../TD_OutStorage/EditForm'

const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}

const columns = [  
  { title: '发货编号', dataIndex: 'Code'},
  { title: '单据日期', dataIndex: 'OrderTime',  customRender: filterDate },
  { title: '发货日期', dataIndex: 'SendTime', customRender: filterDate },
  { title: '发货类型', dataIndex: 'Type' , scopedSlots: { customRender: 'SendType' } },  
  { title: '发货状态', dataIndex: 'Status', scopedSlots: { customRender: 'Status' }},
  { title: '客户', dataIndex: 'Customer.Name' },
  { title: '计划数量', dataIndex: 'TotalNum'},  //总共数量
  { title: '发货数量', dataIndex: 'SendNum'},
  // { title: '确认人', dataIndex: 'ConfirmUserId'},
  // { title: '审核人', dataIndex: 'AuditUserId' },
  { title: '确认人', dataIndex: 'CreateUser.RealName'},
  { title: '审核人', dataIndex: 'AuditUser.RealName'},
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }

// { title: '仓库ID', dataIndex: 'StorId', width: '10%' },
  // { title: '客户ID', dataIndex: 'CusId', width: '10%' },
  // 
  // { title: '发货金额', dataIndex: 'TotalAmt', width: '10%' },
  // { title: '备注', dataIndex: 'Remarks'},  
  // { title: '确认时间', dataIndex: 'ConfirmTime', width: '10%' },  
  // { title: '审核时间', dataIndex: 'AuditeTime', width: '10%' },  
  // { title: '关联单号/出库单号', dataIndex: 'RefCode', width: '10%' },
]

export default {
  components: {
    EditForm,
    EnumName, 
    EnumSelect,
    OutStorage
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
        .post('/TD/TD_Send/GetDataList', {
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
    handleApproval(id) {
      this.disabled = true
      this.$refs.editForm.openForm(id)
    },
    handleOutStorage(id) {
      this.$refs.outStorage.openSendForm(id)
    },
    onOrderTimeChange(dates, dateStrings) {
      this.queryParam.OrderTimeStart = dateStrings[0]
      this.queryParam.OrderTimeEnd = dateStrings[1]
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_Send/DeleteData', ids).then(resJson => {
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