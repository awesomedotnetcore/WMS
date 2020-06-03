<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.RefCode" placeholder="单号" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <enum-select code="RecordBookType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.LocalName" placeholder="原/目标 货架" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.BarCodeBatchNo" placeholder="条码/批次" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
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
    </a-table>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'

const columns = [
  { title: '相关单号', dataIndex: 'RefCode', width: '10%' },
  { title: '台账类型', dataIndex: 'Type', width: '10%' },
  { title: '原仓库', dataIndex: 'FromStorage.Name', width: '10%' },
  { title: '原货位', dataIndex: 'FromLocation.Name', width: '10%' },
  { title: '目标仓库', dataIndex: 'ToStorage.Name', width: '10%' },
  { title: '目标货位', dataIndex: 'ToLocation.Name', width: '10%' },
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '10%' },
  { title: '物料条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '5%' },
  { title: '数量', dataIndex: 'Num', width: '5%' }
]

export default {
  components: {
    EditForm,
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
        .post('/IT/IT_RecordBook/GetDataList', {
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