<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">增加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="AllocateNum" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :max="record.LocalNum" :min="1" @change="e=>handleValChange(e,'AllocateNum',record)"></a-input-number>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <allocate-choose ref="allocateChoose" @choose="handleAllocateChoose"></allocate-choose>
  </div>
</template>

<script>
import AllocateChoose from './AllocateChoose'
const columns1 = [
  { title: '货位', dataIndex: 'FromLocal.Name'},
  { title: '物料', dataIndex: 'Material.Name'},
  { title: '批次', dataIndex: 'BatchNo' },
  { title: '条码', dataIndex: 'BarCode' },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '调拨', dataIndex: 'AllocateNum', scopedSlots: { customRender: 'AllocateNum' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '货位', dataIndex: 'FromLocal.Name' },
  { title: '托盘', dataIndex: 'FromTray.Name' },
  { title: '物料', dataIndex: 'Material.Name' },
  { title: '批次', dataIndex: 'BatchNo' },
  { title: '条码', dataIndex: 'BarCode' },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '调拨', dataIndex: 'AllocateNum', scopedSlots: { customRender: 'AllocateNum' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '货位', dataIndex: 'FromLocal.Name'},
  { title: '托盘', dataIndex: 'FromTray.Name'},
  { title: '分区', dataIndex: 'FromZone.Name'},
  { title: '物料', dataIndex: 'Material.Name'},
  { title: '批次', dataIndex: 'BatchNo' },
  { title: '条码', dataIndex: 'BarCode' },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '调拨', dataIndex: 'AllocateNum', scopedSlots: { customRender: 'AllocateNum' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    AllocateChoose
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
      this.$refs.allocateChoose.openChoose()
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
    },
    handleAllocateChoose(rows) {
      console.log('handleAllocateChoose', rows)
      rows.forEach(element => {
        this.tempId += 1
        var item = { ...element }
        delete item.Id
        delete item.Location
        delete item.LocalId
        item.Id = 'newid_' + this.tempId
        item.FromLocal = { ...element.Location }
        item.FromLocalId = element.LocalId
        item.FromTray = { ...element.Tray }
        item.FromTrayId = element.TrayId
        item.FromZone = { ...element.TrayZone }
        item.FromZoneId = element.TrayZoneId
        item.LocalNum = element.Num
        item.AllocateNum = 1
        item.Price = element.Material.Price
        this.data.push(item)
      })
      this.$emit('input', [...this.data])
    },
    handleValChange(val, name, item) {
      item[name] = val
      this.$emit('input', [...this.data])
    }
  }
}
</script>