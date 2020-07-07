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
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item label="查询类别">
              <a-select allowClear v-model="queryParam.condition">
                <a-select-option key="StorId">仓库ID</a-select-option>
                <a-select-option key="Code">收货单号</a-select-option>
                <a-select-option key="Type">收货类型(枚举)</a-select-option>
                <a-select-option key="RefCode">关联单号</a-select-option>
                <a-select-option key="SupId">供应商ID</a-select-option>
                <a-select-option key="Remarks">备注</a-select-option>
                <a-select-option key="ConfirmUserId">确认ID</a-select-option>
                <a-select-option key="AuditUserId">审核人ID</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="关键字" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
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
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'

const columns = [
  { title: '仓库ID', dataIndex: 'StorId', width: '10%' },
  { title: '收货单号', dataIndex: 'Code', width: '10%' },
  { title: '订单日期', dataIndex: 'OrderTime', width: '10%' },
  { title: '收货日期', dataIndex: 'RecTime', width: '10%' },
  { title: '收货类型(枚举)', dataIndex: 'Type', width: '10%' },
  { title: '关联单号', dataIndex: 'RefCode', width: '10%' },
  { title: '状态(0待审核;1确认；2：取消;3审核通过;4审核失败;5部分入库；6全部入库)', dataIndex: 'Status', width: '10%' },
  { title: '供应商ID', dataIndex: 'SupId', width: '10%' },
  { title: '收货数量', dataIndex: 'TotalNum', width: '10%' },
  { title: '入库数量', dataIndex: 'InNum', width: '10%' },
  { title: '总金额', dataIndex: 'TotalAmt', width: '10%' },
  { title: '备注', dataIndex: 'Remarks', width: '10%' },
  { title: '确认ID', dataIndex: 'ConfirmUserId', width: '10%' },
  { title: '确认时间', dataIndex: 'ConfirmTime', width: '10%' },
  { title: '审核人ID', dataIndex: 'AuditUserId', width: '10%' },
  { title: '审核时间', dataIndex: 'AuditeTime', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
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
      selectedRowKeys: []
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
        .post('/TD/TD_Receiving/GetDataList', {
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
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_Receiving/DeleteData', ids).then(resJson => {
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