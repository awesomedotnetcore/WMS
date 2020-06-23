<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">增加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="BadNum" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :max="record.LocalNum" :min="1" @change="e=>handleValChange(e,'BadNum',record)"></a-input-number>
      </template>
      <template slot="Surplus" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :max="100" :min="0" @change="e=>handleValChange(e,'Surplus',record)" :formatter="value => `${value}%`" :parser="value => value.replace('%', '')"></a-input-number>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <bad-choose ref="badChoose" @choose="handleBadChoose"></bad-choose>
  </div>
</template>

<script>
import BadChoose from './BadChoose'
const columns1 = [
  { title: '货位', dataIndex: 'FromLocal.Name'},
  { title: '物料', dataIndex: 'Material.Name' },
  { title: '批次', dataIndex: 'BatchNo',  },
  { title: '条码', dataIndex: 'BarCode', },
  { title: '库存', dataIndex: 'LocalNum',  },
  { title: '报损', dataIndex: 'BadNum', scopedSlots: { customRender: 'BadNum' } },
  { title: '残余', dataIndex: 'Surplus',  scopedSlots: { customRender: 'Surplus' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '货位', dataIndex: 'FromLocal.Name' },
  { title: '托盘', dataIndex: 'Tray.Name' },
  { title: '物料', dataIndex: 'Material.Name' },
  { title: '批次', dataIndex: 'BatchNo'},
  { title: '条码', dataIndex: 'BarCode'},
  { title: '库存', dataIndex: 'LocalNum'},
  { title: '报损', dataIndex: 'BadNum', scopedSlots: { customRender: 'BadNum' } },
  { title: '残余', dataIndex: 'Surplus', scopedSlots: { customRender: 'Surplus' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '货位', dataIndex: 'FromLocal.Name'},
  { title: '托盘', dataIndex: 'Tray.Name' },
  { title: '分区', dataIndex: 'TrayZone.Name' },
  { title: '物料', dataIndex: 'Material.Name' },
  { title: '批次', dataIndex: 'BatchNo' },
  { title: '条码', dataIndex: 'BarCode'},
  { title: '库存', dataIndex: 'LocalNum'},
  { title: '报损', dataIndex: 'BadNum',  scopedSlots: { customRender: 'BadNum' } },
  { title: '残余', dataIndex: 'Surplus', scopedSlots: { customRender: 'Surplus' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    BadChoose
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
      this.$refs.badChoose.openChoose()
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
    handleBadChoose(rows) {
      console.log('handleBadChoose', rows)
      rows.forEach(element => {
        this.tempId += 1
        var item = { ...element }
        delete item.Id
        delete item.Location
        delete item.LocalId
        item.Id = 'newid_' + this.tempId
        item.FromLocal = { ...element.Location }
        item.FromLocalId = element.LocalId
        item.LocalNum = element.Num
        item.BadNum = 1
        item.Surplus = 0
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