<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.LocalName" placeholder="货位" />
            </a-form-item>
          </a-col>
          <a-col v-if="storage.IsTray" :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.TrayName" placeholder="托盘" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="批次/条码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="getDataList">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" @change="handleTableChange" :loading="loading" :bordered="true" size="small">
    </a-table>
  </a-card>
</template>

<script>
const filterCode = (value, row, index) => {
  if (value === null) {
    return ''
  } else {
    return value.Name + '(' + value.Code + ')'
  }
}
const columns1 = [
  { title: '货位', dataIndex: 'Location', customRender: filterCode, width: '15%' },
  { title: '物料', dataIndex: 'Material', customRender: filterCode, width: '15%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '15%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },
  { title: '数量', dataIndex: 'Num', width: '10%' }
]
const columns2 = [
  { title: '货位', dataIndex: 'Location', customRender: filterCode, width: '15%' },
  { title: '托盘', dataIndex: 'Tray', customRender: filterCode, width: '15%' },
  { title: '物料', dataIndex: 'Material', customRender: filterCode, width: '15%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },
  { title: '数量', dataIndex: 'Num', width: '10%' }
]
const columns3 = [
  { title: '货位', dataIndex: 'Location', customRender: filterCode, width: '15%' },
  { title: '托盘', dataIndex: 'Tray', customRender: filterCode, width: '15%' },
  { title: '托盘分区', dataIndex: 'TrayZone', customRender: filterCode, width: '15%' },
  { title: '物料', dataIndex: 'Material', customRender: filterCode, width: '15%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '10%' }
]

export default {
  components: {
  },
  mounted() {
    this.getDataList()
    this.getCurStorage()
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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns: columns1,
      queryParam: {}
    }
  },
  methods: {
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
          if (this.storage.IsTray && this.storage.IsZone) {
            this.columns = columns3
          } else if (this.storage.IsTray) {
            this.columns = columns2
          } else {
            this.columns = columns1
          }
        })
    },
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
        .post('/IT/IT_LocalMaterial/GetDataList', {
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