<template>
<a-drawer
    title="关联货位"
    placement="right"
    :closable="true"
    @close="onDrawerClose"
    :visible="visible"
    :width="900"
    :getContainer="false"
  >
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.LocalId"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <template slot="Type" slot-scope="text">
        <enum-name code="	StorAreaType" :value="text"></enum-name>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <!-- <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" /> -->
          <a @click="handleDelete([record.LocalId])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <location-Choose ref="locationChoose" @onChoose="handleChoose" :parentObj="this" type="checkbox"></location-Choose>
  </a-card>
</a-drawer>
</template>

<script>
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import LocationChoose from '../../../components/Location/LocationChoose'

const columns = [
  { title: '货位编号', dataIndex: 'PB_Location.Code'},
  { title: '货位名称', dataIndex: 'PB_Location.Name'},  
  { title: '货区', dataIndex: 'PB_Location.PB_StorArea.Name' },//, scopedSlots: { customRender: 'Type' }
  { title: '仓库', dataIndex: 'PB_Location.PB_Storage.Name' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName, 
    LocationChoose
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
      sorter: { field: 'LocalId', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      visible: false,
      typeId: null,
      ids: []
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
      this.queryParam.Keyword = this.typeId

      this.loading = true
      this.$http
        .post('/PB/PB_LocalTray/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'LocalId',
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
      this.$refs.locationChoose.openChoose()
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
            thisObj.$http.post('/PB/PB_LocalTray/DeleteData?typeId=' + thisObj.typeId, thisObj.ids)
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
    openDrawer(typeId) {
      this.typeId = typeId
      this.visible = true
      if (typeId != null) {
        this.getDataList()
      }
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
        this.$http.post('/PB/PB_LocalTray/SaveDatas', {id:this.typeId,keys:this.targetKeys}).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })        
    }
  }
}
</script>