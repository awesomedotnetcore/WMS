<template>
  <a-card :bordered="false" :visible="visible">
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
  </a-card>
</template>

<script>
const columns = [
  { title: '物料名称', dataIndex: 'Name', width: '20%' },
  { title: '物料编码', dataIndex: 'Code', width: '20%' },
  { title: '条码', dataIndex: 'BarCode', width: '20%' },
  { title: '物料简称', dataIndex: 'SimpleName', width: '20%' },
  { title: '物料规格', dataIndex: 'Spec', width: '20%' }
]

export default {
  props: {
    checkId: { type: String, required: false }
  },
  components: {
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
      visible: false
    }
  },
  mounted(){
    this.getDataList()
  },
  methods: {
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
    },
    getDataList() {
      if(this.checkId && this.checkId!=''){
         this.loading = true
            this.$http
                .post('/TD/TD_CheckMaterial/QueryAsync?checkId='+ this.checkId)
                .then(resJson => {
                    this.loading = false
                    this.data = resJson.Data
                })
      }
    },
    getAllKeys(){
      var result=[]
      this.data.forEach((item)=>{
        result.push(item.Id)
      })
      return result
    },
    handleRandom(per){
      this.loading = true
      this.$http
          .post('/TD/TD_Check/LoadRandomMaterial?per='+ per)
          .then(resJson => {
                this.loading = false
                this.data = resJson.Data
            })
    }
  }
}
</script>