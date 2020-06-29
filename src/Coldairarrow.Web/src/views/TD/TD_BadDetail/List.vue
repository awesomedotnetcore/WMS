<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">货位:{{ record.FromLocal.Code }}</template>
            {{ record.FromLocal.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.TrayId">
          <a-tooltip>
            <template slot="title">托盘:{{ record.Tray.Code }}</template>
            {{ record.Tray.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.ZoneId">
          <a-tooltip>
            <template slot="title">分区:{{ record.TrayZone.Code }}</template>
            {{ record.TrayZone.Name }}
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
const columns = [
  { title: '货位', dataIndex: 'FromLocal', scopedSlots: { customRender: 'Location' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '报损', dataIndex: 'BadNum', scopedSlots: { customRender: 'BadNum' } },
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