<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleEdit(record)">编辑</a>
          <a-divider type="vertical" />
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
  </div>
</template>

<script>
const columns1 = [
  { title: '货位', dataIndex: 'FromLocal.Name', width: '15%' },
  { title: '物料', dataIndex: 'Material.Name', width: '15%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '15%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },
  { title: '单价', dataIndex: 'Price', width: '5%' },
  { title: '残余值', dataIndex: 'Surplus', width: '5%' },
  { title: '报损数量', dataIndex: 'LocalNum', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '货位', dataIndex: 'FromLocal.Name', width: '15%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '15%' },
  { title: '物料', dataIndex: 'Material.Name', width: '15%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '15%' },
  { title: '单价', dataIndex: 'Price', width: '5%' },
  { title: '残余值', dataIndex: 'Surplus', width: '5%' },
  { title: '报损数量', dataIndex: 'LocalNum', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '货位', dataIndex: 'FromLocal.Name', width: '15%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '分区', dataIndex: 'TrayZone.Name', width: '5%' },
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '单价', dataIndex: 'Price', width: '5%' },
  { title: '残余值', dataIndex: 'Surplus', width: '6%' },
  { title: '数量', dataIndex: 'LocalNum', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false }
  },
  data() {
    return {
      storage: {},
      data: [],
      columns: columns1,
      tempId: 0,
      selectedRowKeys: [],
      selectedRows: []
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
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      // this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      // this.$refs.editForm.openForm(id)
    },
    handleDelete(items) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            items.forEach(element => {
              var index = thisObj.data.indexOf(element)
              thisObj.data.splice(index, 1)
              var keyIndex = thisObj.selectedRowKeys.indexOf(element.Id)
              thisObj.selectedRowKeys.splice(keyIndex, 1)
            })
            thisObj.$emit('input', [...thisObj.data])
            resolve()
          })
        }
      })
    }
  }
}
</script>