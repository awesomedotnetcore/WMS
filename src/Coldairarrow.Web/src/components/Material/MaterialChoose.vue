<template>
  <a-modal title="物料选择" width="60%" :visible="visible" :confirmLoading="loading" okText="选择" @ok="handleChoose" @cancel="()=>{this.visible=false}">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <materialType-select v-model="queryParam.TypeId"></materialType-select>
            </a-form-item>
          </a-col>
          <a-col :md="5" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="物料名称\编号\条码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.ContactName" placeholder="客户\供应商名称或编码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange, type:type }" :bordered="true" size="small"></a-table>
  </a-modal>
</template>

<script>
import MaterialTypeSelect from '../PB/MaterialTypeSelect'
const columns = [
  { title: '物料名称', dataIndex: 'Name', width: '20%' },
  { title: '物料编码', dataIndex: 'Code', width: '20%' },
  { title: '物料类别', dataIndex: 'MaterialType.Name', width: '20%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },
  { title: '物料简称', dataIndex: 'SimpleName', width: '10%' },
  { title: '物料规格', dataIndex: 'Spec', width: '15%' }
]

export default {
  props: {
    type: { type: String, default: 'radio', required: false }
  },
  components: {
    MaterialTypeSelect
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
        .post('/PB/PB_Material/QueryDataList', {
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
      this.getDataList()
      this.visible = true
    },
    handleChoose() {
      this.visible = false
      this.$emit('onChoose', this.selectedRows)
    }
  }
}
</script>