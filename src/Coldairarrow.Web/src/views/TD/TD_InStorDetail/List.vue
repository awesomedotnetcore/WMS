<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>
    <edit-form ref="editForm" v-model="curDetail"></edit-form>
  </div>
</template>

<script>
import EditForm from './EditForm'

const columns1 = [
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '编码', dataIndex: 'Material.Code', width: '10%' },
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '单价', dataIndex: 'Price', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '编码', dataIndex: 'Material.Code', width: '10%' },
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '单价', dataIndex: 'Price', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '编码', dataIndex: 'Material.Code', width: '10%' },
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '托盘分区', dataIndex: 'TrayZone.Name', width: '10%' },
  { title: '单价', dataIndex: 'Price', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
  },
  props: {
    value: { type: Object, required: true }
  },
  data() {
    return {
      storage: {},
      data: [],
      curDetail: {},
      columns: columns1,
      tempId: 0,
      selectedRowKeys: []
    }
  },
  watch: {
    value(val) {
      this.data = [...this.value]
    }
  },
  mounted() {
    this.data = [...this.value]
    this.getCurStorage()
  },
  methods: {
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
          if (this.storage.IsTray && this.storage.IsZone) {
            this.columns = columns3
          } else if (this.storage.IsTray) {
            this.columns = columns2
          } else {
            this.columns = columns1
          }
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.tempId += 1
      this.curDetail = { Id: 'TEMP_' + this.tempId.toString(), LocalId: '', TrayId: '', ZoneId: '', MaterialId: '' }
      this.data.push(this.curDetail)
      this.$refs.editForm.openForm()
    },
    handleEdit(item) {
      this.curDetail = item
      this.$refs.editForm.openForm()
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_InStorDetail/DeleteData', ids).then(resJson => {
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
    }
  }
}
</script>