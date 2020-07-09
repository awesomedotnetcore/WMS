<template>
  <a-modal title="发货" width="80%" :visible="visible" :confirmLoading="loading" okText="选择" @ok="handleChoose" @cancel="()=>{this.visible=false}">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <material-type v-model="queryParam.MaterialTypeId"></material-type>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料/条码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-row>
              <a-col :span="12">
                <a-form-item label="少于预警">
                  <a-switch v-model="queryParam.MinAlert" />
                </a-form-item>
              </a-col>
              <a-col :span="12">
                <a-form-item label="超出预警">
                  <a-switch v-model="queryParam.MaxAlert" />
                </a-form-item>
              </a-col>
            </a-row>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = { MinAlert: false, MaxAlert: false })">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <!-- <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :bordered="true" size="small"> -->
    <a-table :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">

      <a-breadcrumb slot="Material" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">编码:{{ record.Code }} 简称:{{ record.SimpleName }}</template>
            {{ record.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.BarCode">
          <a-tooltip>
            <template slot="title">条码</template>
            {{ record.BarCode }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
      <a-tooltip slot="MinAlert" slot-scope="text, record">
        <template slot="title">最小库存</template>
        <a-tag v-if="text !== null && text >= record.SumCount" color="red">{{ text === null ? '无' : text }}</a-tag>
        <a-tag v-else>{{ text === null ? '无' : text }}</a-tag>
      </a-tooltip>
      <a-tooltip slot="MaxAlert" slot-scope="text, record">
        <template slot="title">最大库存</template>
        <a-tag v-if="text !== null && text <= record.SumCount" color="red">{{ text === null ? '无' : text }}</a-tag>
        <a-tag v-else>{{ text === null ? '无' : text }}</a-tag>
      </a-tooltip>
    </a-table>
  </a-modal>
</template>

<script>
import MaterialType from '../../../components/PB/MaterialTypeSelect'
const columns = [
  { title: '物料名称', dataIndex: 'Name', scopedSlots: { customRender: 'Material' } },
  { title: '物料编码', dataIndex: 'Code' },
  { title: '物料类别', dataIndex: 'MaterialTypeName' },
  { title: '批次号', dataIndex: 'BatchNo' },
  { title: '规格', dataIndex: 'Spec' },
  { title: '单位', dataIndex: 'MeasureName' },
  { title: '最小库存', dataIndex: 'Min', scopedSlots: { customRender: 'MinAlert' } },
  { title: '最大库存', dataIndex: 'Max', scopedSlots: { customRender: 'MaxAlert' } },
  { title: '库存总量', dataIndex: 'SumCount' }
]

export default {
  components: {
    MaterialType
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
      queryParam: { MinAlert: false, MaxAlert: false },
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
        .post('/Report/MaterialSummary/GetDataList', {
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
    openChoose() {
      this.getDataList()
      this.visible = true
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    handleChoose() {
      this.visible = false
      this.$emit('choose', [...this.selectedRows])
    }
  }
}
</script>