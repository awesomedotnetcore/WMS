<template>
<div>
  <a-spin :spinning="loading">
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
      <span v-if="NumVisible===true">{{record.LocalNum}}</span>
      <span v-else>-</span>
    </template>
    <template slot="CheckNum" slot-scope="text, record">
      <span v-if="Enabled===true">
        <a-input type="number" v-model="record.CheckNum" 
          :key="record.Id"
          :min="0" 
          :addon-after="record.MeasureName"
          @blur="e =>handleCheckNumChange(e.target, record)">
        </a-input>
      </span>
      <span v-else>
        <span v-if="NumVisible===true">{{record.CheckNum}}</span>
      <span v-else>-</span>
      </span>
    </template>
    <template slot="DisNum" slot-scope="text, record">
      <span v-if="NumVisible===true">{{record.DisNum}}</span>
      <span v-else>-</span>
    </template>
  </a-table>
  </a-spin>
</div>
</template>

<script>
const columns = [
  { title: '库区', dataIndex: 'AreaName' },
  { title: '巷道', dataIndex: 'LanewayName'},
  { title: '货架', dataIndex: 'RackName' },
  { title: '托盘', dataIndex: 'TrayName'},
  { title: '托盘分区', dataIndex: 'ZoneName'},
  { title: '货位编码', dataIndex: 'LocationCode'},
  { title: '货位名称', dataIndex: 'LocationName'},
  { title: '批次号', dataIndex: 'BatchNo'},
  { title: '物料编码', dataIndex: 'MaterialCode'},
  { title: '物料名称', dataIndex: 'MaterialName' },  
  { title: '库存数量', dataIndex: 'LocalNum', scopedSlots: { customRender: 'LocalNum' } },
  { title: '盘点数量', dataIndex: 'CheckNum', scopedSlots: { customRender: 'CheckNum' } },
  { title: '盘差数量', dataIndex: 'DisNum', scopedSlots: { customRender: 'DisNum' } }
]

export default {
  props: {
    checkId: { type: String, required: false },
    isCompleted: { type: Boolean, required: true },
    isCheckd: { type: Boolean, required: true }
  },
  watch: {
    checkId(id) {
      this.queryParam.CheckId = id
      this.getDataList()
    },
    isCompleted(val) {
      this.NumVisible = val
    },
    isCheckd(val) {
      this.Enabled = val
    }
  },
  mounted(){
    this.queryParam.CheckId = this.checkId
    this.NumVisible = this.isCompleted
    this.Enabled = this.isCheckd

    this.pagination.current = 1
  },
  data() {
    return {
      data: [],
      NumVisible: false,
      Enabled: false,
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
    handleCheckNumChange(target, data){
      this.loading = true
      if(target.value>=0){
        this.$http.post('/TD/TD_CheckData/ModifyCheckNum', { Id: data.Id, CheckNum:target.value })
        .then(resJson => {
          this.loading = false
        })
      }
      else{
        target.value=null
        this.loading = false
      }
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