<template>
  <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.Id"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :bordered="true"
      size="small"
    >
    </a-table>
</template>

<script>
const columns = [
  { title: '盘点ID', dataIndex: 'CheckId', width: '10%' },
  { title: '仓库ID', dataIndex: 'StorId', width: '10%' },
  { title: '货位ID', dataIndex: 'localId', width: '10%' },
  { title: '物料ID', dataIndex: 'MaterialId', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '库存数量', dataIndex: 'LocalNum', width: '10%' },
  { title: '盘点数量', dataIndex: 'CheckNum', width: '10%' },
  { title: '盘差数量', dataIndex: 'DisNum', width: '10%' },
  { title: '盘点人ID', dataIndex: 'CheckUserId', width: '10%' }
]

export default {
  props: {
    checkId: { type: String, required: false }
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
      queryParam: {}
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
      this.loading = true
      this.$http
        .post('/TD/TD_CheckData/GetDataList', {
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
    }
  }
}
</script>