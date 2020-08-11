<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button v-if="hasPerm('PD_Plan.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('PD_Plan.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="计划编号" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="Material" slot-scope="record">
        <a-tooltip>
          <template slot="title">{{ record.Code }}</template>
          {{ record.Name }}
        </a-tooltip>      
      </template>
      <template slot="YesOrNo" slot-scope="text">
        <a-tag v-if="text" color="green">已完成</a-tag>
        <a-tag v-else color="pink">未完成</a-tag>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="!record.Status" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="!record.Status" type="vertical" />
          <a v-if="!record.Status" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="!record.Status" type="vertical" />
          <a @click="handleStatus(record.Id,record.Status==0?1:0)">{{ record.Status==1?'未完成':'已完成' }}</a>
          <a-divider type="vertical" />
        </template>
      </span>
    </a-table>
    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'

const columns = [
  { title: '计划编号', dataIndex: 'Code', width: '12%' },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '计划日期', dataIndex: 'PlanDate',width: '12%' },
  { title: '开始日期', dataIndex: 'StartDate',width: '12%' },
  { title: '完成日期', dataIndex: 'FinishDate',width: '12%' },
  { title: '生产单元', dataIndex: 'UnitCode' },
  { title: '状态', dataIndex: 'Status', width: '6%', scopedSlots: { customRender: 'YesOrNo' } }, //是否完成},
  { title: '操作', dataIndex: 'action', width: '18%', scopedSlots: { customRender: 'action' } }
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
        .post('/PD/PD_Plan/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新增')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PD/PD_Plan/DeleteData', ids).then(resJson => {
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
    handleStatus(id, status) {
      var thisObj = this
      this.$confirm({
        title: '确认' + (status ? '已完成' : '未完成') + '吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PD/PD_Plan/Status?Id=' + id + '&status=' + status).then(resJson => {
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