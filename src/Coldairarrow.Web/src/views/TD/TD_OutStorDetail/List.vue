<template>
    <div>
    <div class="table-operator"  v-if="!receive">
      <a-button :disabled="disabled" type="primary" icon="plus" @click="hanldleAdd()"  >添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected() || disabled" >删除</a-button>
    </div>
    <a-table
      ref="table"    
      :columns="receive?sendColumns:columns" 
      :rowKey="row => row.Id"
      :pagination="false" 
      :dataSource="data"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
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

      <template slot="OutNum" slot-scope="text, record">
        <div v-if="record.Num > 0">          
          <a-input-number :disabled="disabled"  size="small" :value="text" :max="record.Num"  :min="1" @change="e=>handleValChange(e,'OutNum',record)"></a-input-number>
        </div>
        <div v-else>
          <a-input-number :disabled="disabled"  size="small" :value="text" :max="record.LocalNum"  :min="1" @change="e=>handleValChange(e,'OutNum',record)"></a-input-number>
        </div>
        <!-- <a-input-number :disabled="disabled"  size="small" :value="text" :max="record.LocalNum"  :min="1" @change="e=>handleValChange(e,'OutNum',record)"></a-input-number> -->
      </template>


      <span slot="action" slot-scope="text, record">       
        <template>
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>      
    </a-table>

    <localmaterial-list ref="localmaterialList" @choose="handlerDetailSubmit"></localmaterial-list>
  </div>  
</template>

<script>
import LocalmaterialList from './LocalMaterialList'
// import LocalSelect from '../../../components/Location/LocationSelect'
// import SendmaterialList from '../TD_SendDetail/SendMaterialList'

const columns = [
  { title: '物料', dataIndex: 'Material.Name'},
  { title: '编码', dataIndex: 'Material.Code' },
  { title: '货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  // { title: '货位', dataIndex: 'Location.Name'},
  { title: '条码', dataIndex: 'BarCode'},
  { title: '批次号', dataIndex: 'BatchNo'},
  { title: '库存', dataIndex: 'LocalNum'},
  { title: '出库数量', dataIndex: 'OutNum', scopedSlots: { customRender: 'OutNum' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
const sendColumns = [
  { title: '物料', dataIndex: 'Material.Name'},
  { title: '编码', dataIndex: 'Material.Code' },
  { title: '货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  { title: '批次号', dataIndex: 'BatchNo'},
  { title: '库存', dataIndex: 'LocalNum'},
  { title: '出库数量', dataIndex: 'OutNum', scopedSlots: { customRender: 'OutNum' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    LocalmaterialList,
    // LocalSelect
    // SendmaterialList
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false },
    receive: { type: Boolean, required: false, default: false } 
  },
  data() {
    return {
      storage: {},
      data: [],
      // curDetail: {},
      columns,
      sendColumns,
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
      //this.tempId += 1
      //var SendDetail = { Id: 'newid_' + this.tempId.toString(), LocalId: '', TrayId: null, ZoneId: null, MaterialId: '' }
      this.$refs.localmaterialList.openChoose()//SendDetail
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
    handlerDetailSubmit(rows) {
      console.log('handlerDetailSubmit', rows)
      rows.forEach(element => {
        this.tempId += 1
        var item = { ...element }
        delete item.Id
        delete item.Location
        delete item.LocalId
        item.Id = 'newid_' + this.tempId
        item.Location = { ...element.Location }
        item.LocalId = element.LocalId
        item.LocalNum = element.Num
        item.OutNum = 1
        // item.Surplus = 0
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