<template>
  <a-card :bordered="false" :visible="visible">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">选择物料</a-button>
      <a-button
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
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

    <material-choose ref="materialChoose" @onChoose="handleChoose" :parentObj="this" type="checkbox"></material-choose>
  </a-card>
</template>

<script>
import MaterialChoose from '../../../components/Material/MaterialChoose'
import EnumName from '../../../components/BaseEnum/BaseEnumName'

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
    MaterialChoose,
    EnumName
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
      this.selectedRowKeys = []
    },
    getDataList() {
      this.selectedRowKeys = []
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.materialChoose.openChoose()
    },
    handleDelete(ids) {
        this.loading = true
        ids.forEach((id)=>{            
            const dataSource = [...this.data];
            this.data = dataSource.filter(item => item.Id !== id);
        })
        this.selectedRowKeys = []     
        this.pagination.current=1   
        this.loading = false
    },
    handleChoose(chooseRows){
        chooseRows.forEach((row)=>{
            if(this.data.indexOf(row)===-1){
                this.data.push(row)
            }
        })
    }
  }
}
</script>