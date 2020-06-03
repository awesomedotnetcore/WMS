<template>
  <div>
    <div class="table-operator">
      <a-button :disabled="disabled" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleEdit(record)">编辑</a>
          <a-divider type="vertical" />
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <edit-form ref="editForm" @submit="handlerDetailSubmit"></edit-form>
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
  { title: '数量', dataIndex: 'Num', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '编码', dataIndex: 'Material.Code', width: '10%' },
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '编码', dataIndex: 'Material.Code', width: '10%' },
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '托盘分区', dataIndex: 'TrayZone.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
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
      this.tempId += 1
      var curDetail = { Id: 'newid_' + this.tempId.toString(), LocalId: '', TrayId: '', ZoneId: '', MaterialId: '' }
      this.$refs.editForm.openForm(curDetail)
    },
    handleEdit(item) {
      this.$refs.editForm.openForm({ ...item })
    },
    handlerDetailSubmit(item) {
      console.log('handlerDetailSubmit', item)
      var isNew = true
      this.data.forEach(element => {
        if (element.Id === item.Id) {
          isNew = false
          Object.keys(item).forEach(prop => {
            element[prop] = item[prop]
          })
        }
      })
      if (isNew) {
        this.data.push({ ...item })
      }
      this.$emit('input', [...this.data])
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