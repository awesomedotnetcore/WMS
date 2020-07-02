<template>
  <a-drawer title="关联物料清单" placement="right" :closable="true" @close="onDrawerClose" :visible="visible" :width="800" :getContainer="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.MaterialId" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="action" slot-scope="text, record">
        <template>
          <!-- <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" /> -->
          <a @click="handleDelete([record.MaterialId])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <material-Choose ref="materialChoose" @onChoose="handleChoose" :parentObj="this" type="checkbox"></material-Choose>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'
import MaterialChoose from '../../../components/Material/MaterialChoose'

const columns = [
  { title: '物料编码', dataIndex: 'PB_Material.Code', width: '20%' },
  { title: '物料名称', dataIndex: 'PB_Material.Name', width: '20%' },
  { title: '物料规格', dataIndex: 'PB_Material.Spec' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    MaterialChoose
  },
  mounted() {
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
      sorter: { field: 'MaterialId', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      visible: false,
      ids: [],
      targetKeys: [],
    }
  },
  methods: {
    init() {
      this.targetKeys = []
    },
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
        .post('/PB/PB_AreaMaterial/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'MaterialId',
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.materialChoose.openChoose()
      //this.$refs.editForm.openForm(this.filters.AreaId, '添加关联物料')
    },
    // handleEdit(id) {
    //   this.$refs.editForm.openForm(id)
    // },
    handleDelete(ids) {
      this.ids = ids
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_AreaMaterial/DeleteData?AreaId=' + thisObj.filters.AreaId, thisObj.ids)
              .then(resJson => {
                resolve()
                if (resJson.Success) {
                  thisObj.$message.success('操作成功!')
                  thisObj.getDataList()
                } else {
                  thisObj.$message.error(resJson.Msg)
                }
              })
          })
        }
      })
    },
    openDrawer(areaId) {
      this.filters.AreaId = areaId
      this.visible = true
      this.getDataList()
    },
    onDrawerClose() {
      this.visible = false
    },
    handleChoose(chooseRows){
      this.targetKeys = []
      chooseRows.forEach( row =>{    
        if(this.targetKeys.indexOf(row.Id)===-1){     
          this.targetKeys.push(row.Id)
        }
        })
        this.loading = true
        this.$http.post('/PB/PB_AreaMaterial/SaveDatas', {id:this.filters.AreaId,keys:this.targetKeys}).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.$message.success('操作成功!')
           // this.visible = false

            this.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
        
    }
  }
}
</script>