<template>
  <a-modal title="库存物料选择" width="60%" :visible="visible" :confirmLoading="loading" okText="选择" @ok="handleChoose" @cancel="()=>{this.visible=false}">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="物料编码或名称" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table :columns="columns" :rowKey="row => row.Id" :dataSource="data" 
      :pagination="pagination" :loading="loading" 
      @change="handleTableChange" 
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange,type:type }" 
      :bordered="true" size="small">
    </a-table>
  </a-modal>
</template>

<script>

const filterCode = (value, row, index) => {
  return value.Name + '(' + value.Code + ')'
}
const columns = [  
  { title: '物料', dataIndex: 'Material', customRender: filterCode, width: '15%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },  
  { title: '批次号', dataIndex: 'BatchNo', width: '15%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '10%' },
  { title: '货位', dataIndex: 'Location', customRender: filterCode, width: '15%' },
  { title: '托盘', dataIndex: 'Tray', customRender: filterCode, width: '15%' },
  { title: '托盘分区', dataIndex: 'TrayZone', customRender: filterCode, width: '15%' },
  // { title: '数量', dataIndex: 'Num', width: '10%' }
]

export default {
  props: {
    type: { type: String, default: 'radio', required: false }
  },
  components: {
    // EnumName
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
      selectedRows: [],
      visible: false
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
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    openChoose() {
      this.visible = true
    },
    handleChoose() {
      this.visible = false
      this.$emit('onChoose', this.selectedRows)
    }
  }
}
</script>