<template>
  <div>
    <div class="table-operator" v-if="!receive">
      <a-button :disabled="disabled" type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="receive?receiveColumns:columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
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
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <local-select v-if="receive" size="small" v-model="record.LocalId" :storid="storage.Id" :disabled="disabled" @select="e=>handleValChange(e,'Location',record)"></local-select>
          <a-tooltip v-else>
            <template slot="title">货位:{{ record.Location.Code }}</template>
            {{ record.Location.Name }}
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
      <template slot="Num" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :max="record.PlanNum-record.RecNum" :min="0" @change="e=>handleValChange(e,'Num',record)"></a-input-number>
      </template>
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
import LocalSelect from '../../../components/Location/LocationSelect'
const columns = [
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  { title: '数量', dataIndex: 'Num' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const receiveColumns = [
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  { title: '收货数量', dataIndex: 'PlanNum' },
  { title: '入库数量', dataIndex: 'Num', scopedSlots: { customRender: 'Num' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    LocalSelect
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false },
    receive: { type: Boolean, required: false, default: false } // 收货入库
  },
  data() {
    return {
      storage: {},
      data: [],
      columns,
      receiveColumns,
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
      this.tempId += 1
      var curDetail = { Id: 'newid_' + this.tempId.toString(), LocalId: '', TrayId: null, ZoneId: null, MaterialId: '' }
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
    },
    handleValChange(val, name, item) {
      item[name] = val
      this.$emit('input', [...this.data])
    }
  }
}
</script>