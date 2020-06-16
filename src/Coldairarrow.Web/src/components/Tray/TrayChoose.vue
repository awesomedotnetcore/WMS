<template>
  <a-modal title="托盘选择" width="60%" :visible="visible" :confirmLoading="loading" okText="选择" @ok="handleChoose" @cancel="()=>{this.visible=false}">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="3" :sm="24">
            <materialType-select v-model="queryParam.TypeId"></materialType-select>
          </a-col>
          <a-col :md="3" :sm="24">
            <supplier-select v-model="queryParam.SupplierId"></supplier-select>
          </a-col>
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="托盘名称或编码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange,type:type }" :bordered="true" size="small">
    </a-table>
  </a-modal>
</template>

<script>
const filterYesOrNo = (value, row, index) => {
  if (value) return '启用'
  else return '停用'
}
const columns = [
  { title: '托盘号', dataIndex: 'Code', width: '15%' },
  { title: '托盘名称', dataIndex: 'Name', width: '15%' },
  { title: '托盘类型', dataIndex: 'PB_TrayType.Name', width: '10%' },
  { title: '货位', dataIndex: 'PB_Location.Name', width: '15%' },
  { title: '启用日期', dataIndex: 'StartTime', width: '20%' },
  { title: '托盘状态', dataIndex: 'Status', width: '10%', customRender: filterYesOrNo },
  { title: '备注', dataIndex: 'Remarks', width: '15%' }
]

export default {
  props: {
    type: { type: String, default: 'radio', required: false }
  },
  components: {
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
        .post('/PB/PB_Tray/GetDataList', {
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