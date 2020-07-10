<template>
  <a-modal title="货位选择" width="60%" :visible="visible" :confirmLoading="loading" okText="选择" @ok="handleChoose" @cancel="()=>{this.visible=false}">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="5" :sm="24">
            <a-form-item>
              <all-storage-select v-model="queryParam.StorId"></all-storage-select>
            </a-form-item>
          </a-col>
          <a-col :md="5" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="编码\名称\巷道\货架" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.AreaName" placeholder="货区编码/名称" />
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
      <template slot="Type" slot-scope="text">
        <enum-name code="LocationType" :value="text"></enum-name>
      </template>
    </a-table>
  </a-modal>
</template>

<script>
import EnumName from '../BaseEnum/BaseEnumName'
import AllStorageSelect from '../Storage/AllStorageSelect'

const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}
const columns = [
  { title: '货位编号', dataIndex: 'Code' },
  { title: '货位名称', dataIndex: 'Name' },
  // { title: '货位类型', dataIndex: 'Type', width: '10%', scopedSlots: { customRender: 'Type' } },
  { title: '仓库', dataIndex: 'PB_Storage.Name' },
  { title: '货区', dataIndex: 'PB_StorArea.Name' },
  { title: '巷道', dataIndex: 'PB_Laneway.Name' },
  { title: '货架', dataIndex: 'PB_Rack.Name' }
  // { title: '剩余容量', dataIndex: 'OverVol', width: '10%' },
  // { title: '是否禁用', dataIndex: 'IsForbid', width: '5%', customRender: filterYesOrNo }, // 是否禁用
  // { title: '是否默认', dataIndex: 'IsDefault', width: '5%', customRender: filterYesOrNo }// 是否默认库位
]

export default {
  props: {
    type: { type: String, default: 'radio', required: false },
    storid: { type: String, default: '', required: false }
  },
  components: {
    EnumName,
    AllStorageSelect
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
  mounted() {
    if (this.storid) {
      this.queryParam.StorId = this.storid
    }
  },
  watch: {
    storid(storid) {
      if (this.storid) {
        this.queryParam.StorId = this.storid
      }
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
        .post('/PB/PB_Location/GetDataList', {
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
      this.getDataList()
    },
    handleChoose() {
      this.visible = false
      this.$emit('onChoose', this.selectedRows)
    }
  }
}
</script>