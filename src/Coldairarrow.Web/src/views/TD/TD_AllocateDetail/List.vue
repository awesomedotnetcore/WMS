<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="false" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">货位:{{ record.FromLocal.Code }}</template>
            {{ record.FromLocal.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.FromTrayId">
          <a-tooltip>
            <template slot="title">托盘:{{ record.FromTray.Code }}</template>
            {{ record.FromTray.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.FromZoneId">
          <a-tooltip>
            <template slot="title">分区:{{ record.FromZone.Code }}</template>
            {{ record.FromZone.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
      <a-breadcrumb slot="Material" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">物料:{{ record.Material.Code }}</template>
            {{ record.Material.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.BatchNo">
          <a-tooltip>
            <template slot="title">批次</template>
            {{ record.BatchNo }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.BarCode">
          <a-tooltip>
            <template slot="title">条码</template>
            {{ record.BarCode }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
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
const columns = [
  { title: '货位', dataIndex: 'FromLocal', scopedSlots: { customRender: 'Location' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
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
      columns,
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