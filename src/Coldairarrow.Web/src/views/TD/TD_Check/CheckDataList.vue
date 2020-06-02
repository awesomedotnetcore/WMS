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
      <template slot="LocalNum" slot-scope="text, record">
        <span v-if="NumVisible===true">
          {{record.LocalNum}}
        </span>
        <span v-else>
          -
        </span>
      </template>
      <template slot="CheckNum" slot-scope="text, record">
        <span v-if="NumVisible===true">
          {{record.CheckNum}}
        </span>
        <span v-else>
          -
        </span>
      </template>
      <template slot="DisNum" slot-scope="text, record">
        <span v-if="NumVisible===true">
          {{record.DisNum}}
        </span>
        <span v-else>
          -
        </span>
      </template>
    </a-table>
</template>

<script>
const columns = [
  { title: '库区', dataIndex: 'AreaName', width: '7%' },
  { title: '巷道', dataIndex: 'LanewayName', width: '7%' },
  { title: '货架', dataIndex: 'RackName', width: '7%' },
  { title: '货位编码', dataIndex: 'LocationCode', width: '7%' },
  { title: '货位名称', dataIndex: 'LocationName', width: '7%' },
  { title: '物料编码', dataIndex: 'MaterialCode', width: '10%' },
  { title: '物料名称', dataIndex: 'MaterialName' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '库存数量', dataIndex: 'LocalNum', width: '7%' , scopedSlots: { customRender: 'LocalNum' }},
  { title: '盘点数量', dataIndex: 'CheckNum', width: '7%', scopedSlots: { customRender: 'CheckNum' } },
  { title: '盘差数量', dataIndex: 'DisNum', width: '7%' , scopedSlots: { customRender: 'DisNum' }}
]

export default {
  props: {
    checkId: { type: String, required: false },
    isComplete: { type:Boolean, default:false, required:false}
  },
  watch:{
    checkId(id){
      this.queryParam.CheckId=id
      this.pagination.current=1
      this.getDataList()
    },isComplete(val){
      this.NumVisible=val
    }
  },
  data() {
    return {
      data: [],
      NumVisible:false,
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
        .post('/TD/TD_CheckData/QueryDataList', {
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