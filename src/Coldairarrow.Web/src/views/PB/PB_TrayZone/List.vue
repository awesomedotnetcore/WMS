<template>
  <a-drawer title="分区信息" placement="right" :closable="true" @close="onDrawerClose" :visible="visible" :width="900" :getContainer="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'

const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}
const columns = [
  { title: '编号', dataIndex: 'Code', width: '10%' },
  { title: '名称', dataIndex: 'Name', width: '10%' },
  { title: 'X', dataIndex: 'X', width: '10%' },
  { title: 'Y', dataIndex: 'Y', width: '10%' },
  { title: '是否默认', dataIndex: 'IsDefault', width: '10%', customRender: filterYesOrNo },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
  },
  mounted() {
  },
  data() {
    return {
      data: [],
      filters: {},
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      visible: false,
      typeId: null
    }
  },
  methods: {
    handleTableChange(filters, sorter) {
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getDataList() {
      var thisObj = this
      this.selectedRowKeys = []

      this.loading = true
      this.$http
        .post('/PB/PB_TrayZone/GetDataListByType?typeId=' + thisObj.typeId)
        .then(resJson => {
          thisObj.loading = false
          thisObj.data = resJson.Data
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.editForm.openForm(this.typeId, null, '新增')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(this.typeId, id, '编辑')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_TrayZone/DeleteData', ids).then(resJson => {
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
    }
  }
}
</script>