<template>
  <div>
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()" :disabled="disabled">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="From" slot-scope="text, record">
        <a-breadcrumb>
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
      </template>
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
      <template slot="MoveNum" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :max="record.LocalNum" :min="1" @change="e=>handleValChange(e,'MoveNum',record)"></a-input-number>
      </template>
      <template slot="ToLocalId" slot-scope="text, record">
        <local-select size="small" v-model="record.ToLocalId" :storid="storage.Id" :disabled="disabled"></local-select>
      </template>
      <template slot="ToTrayId" slot-scope="text, record">
        <tray-select size="small" v-model="record.ToTrayId" :locartalId="record.ToLocalId" :materialId="record.MaterialId" :disabled="disabled"></tray-select>
      </template>
      <template slot="ToZoneId" slot-scope="text, record">
        <zone-select size="small" v-model="record.ToZoneId" :trayId="record.ToTrayId" :disabled="disabled"></zone-select>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <move-choose ref="moveChoose" @choose="handleMoveChoose"></move-choose>
  </div>
</template>

<script>
import MoveChoose from './MoveChoose'
import LocalSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
const columns1 = [
  { title: '原货位', dataIndex: 'FromLocal', scopedSlots: { customRender: 'From' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '移库', dataIndex: 'MoveNum', scopedSlots: { customRender: 'MoveNum' } },
  { title: '目标货位', dataIndex: 'ToLocalId', scopedSlots: { customRender: 'ToLocalId' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns2 = [
  { title: '原货位', dataIndex: 'FromLocal', scopedSlots: { customRender: 'From' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '移库', dataIndex: 'MoveNum', scopedSlots: { customRender: 'MoveNum' } },
  { title: '目标货位', dataIndex: 'ToLocalId', scopedSlots: { customRender: 'ToLocalId' } },
  { title: '目标托盘', dataIndex: 'ToTrayId', scopedSlots: { customRender: 'ToTrayId' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const columns3 = [
  { title: '原货位', dataIndex: 'FromLocal', scopedSlots: { customRender: 'From' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '库存', dataIndex: 'LocalNum' },
  { title: '移库', dataIndex: 'MoveNum', scopedSlots: { customRender: 'MoveNum' } },
  { title: '目标货位', dataIndex: 'ToLocalId', scopedSlots: { customRender: 'ToLocalId' } },
  { title: '目标托盘', dataIndex: 'ToTrayId', scopedSlots: { customRender: 'ToTrayId' } },
  { title: '目标分区', dataIndex: 'ToZoneId', scopedSlots: { customRender: 'ToZoneId' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    MoveChoose,
    LocalSelect,
    TraySelect,
    ZoneSelect
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
      this.$refs.moveChoose.openChoose()
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
    handleMoveChoose(rows) {
      console.log('handleMoveChoose', rows)
      rows.forEach(element => {
        this.tempId += 1
        var item = { ...element }
        delete item.Id
        delete item.Location
        delete item.LocalId
        delete item.Tray
        delete item.TrayId
        delete item.TrayZone
        delete item.ZoneId
        item.Id = 'newid_' + this.tempId

        item.FromLocal = { ...element.Location }
        item.FromLocalId = element.LocalId
        item.ToLocal = { ...element.Location }
        item.ToLocalId = element.LocalId

        item.FromTray = { ...element.Tray }
        item.FromTrayId = element.TrayId
        item.ToTray = { ...element.Tray }
        item.ToTrayId = element.TrayId

        item.FromZone = { ...element.TrayZone }
        item.FromZoneId = element.ZoneId
        item.ToZone = { ...element.TrayZone }
        item.ToZoneId = element.ZoneId

        item.LocalNum = element.Num
        item.MoveNum = element.Num
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