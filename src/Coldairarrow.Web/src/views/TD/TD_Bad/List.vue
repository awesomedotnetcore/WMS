<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="2" :sm="24">
            <a-form-item>
              <enum-select code="BadType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="单号" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-range-picker @change="onBadTimeChange" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-select v-model="queryParam.Status" placeholder="状态" :allowClear="true">
                <a-select-option :key="0" :value="0">待审核</a-select-option>
                <a-select-option :key="1" :value="1">审核通过</a-select-option>
                <a-select-option :key="2" :value="2">审核失败</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="getDataList">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="Type" slot-scope="text">
        <enum-name code="BadType" :value="text"></enum-name>
      </template>
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
import moment from 'moment'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EditForm from './EditForm'
const filterStatus = (value, row, index) => {
  var status = '待审核'
  switch (value) {
    case 0: status = '待审核'; break
    case 1: status = '审核通过'; break
    case 2: status = '审核失败'; break
    default: break
  }
  return status
}
const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '报损单号', dataIndex: 'Code', width: '10%' },
  { title: '报损时间', dataIndex: 'BadTime', customRender: filterDate, width: '10%' },
  { title: '报损类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' }, width: '10%' },
  { title: '报损数量', dataIndex: 'BadNum', width: '10%' },
  { title: '总金额', dataIndex: 'TotalAmt', width: '10%' },
  { title: '状态', dataIndex: 'Status', customRender: filterStatus, width: '10%' },
  { title: '制单人', dataIndex: 'CreateUser.RealName', width: '10%' },
  { title: '审核人', dataIndex: 'AuditUser.RealName', width: '10%' },
  { title: '审核时间', dataIndex: 'AuditeTime', customRender: filterDate, width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    EnumSelect
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
        .post('/TD/TD_Bad/GetDataList', {
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
    onBadTimeChange(dates, dateStrings) {
      this.queryParam.BadTimeStart = dateStrings[0]
      this.queryParam.BadTimeEnd = dateStrings[1]
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
            thisObj.$http.post('/TD/TD_Bad/DeleteData', ids).then(resJson => {
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