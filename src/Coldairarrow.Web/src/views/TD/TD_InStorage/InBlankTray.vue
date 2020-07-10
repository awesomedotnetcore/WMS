<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">选择托盘</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="LocalId" slot-scope="text, record">
        <local-select v-model="record.LocalId" :storid="storage.Id" size="small"></local-select>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <tray-choose ref="trayChoose" type="checkbox" @onChoose="handleTrayChoose"></tray-choose>
  </a-modal>
</template>

<script>
import TrayChoose from '../../../components/Tray/TrayChoose'
import LocalSelect from '../../../components/Location/LocationSelect'
const columns = [
  { title: '托盘名称', dataIndex: 'Name', width: '15%' },
  { title: '托盘编码', dataIndex: 'Code', width: '15%' },
  { title: '托盘类型', dataIndex: 'PB_TrayType.Name', width: '15%' },
  { title: '货位', dataIndex: 'LocalId', scopedSlots: { customRender: 'LocalId' }, width: '30%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]
export default {
  components: {
    TrayChoose,
    LocalSelect
  },
  props: {},
  data() {
    return {
      visible: false,
      loading: false,
      data: [],
      columns,
      storage: {},
      selectedRowKeys: [],
      selectedRows: [],
      title: '空托盘入库'
    }
  },
  mounted() {
    this.getCurStorage()
  },
  methods: {
    init() {
      this.visible = true
      this.data = []
    },
    openForm() {
      this.init()
    },
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
        })
    },
    handleSubmit() {
      if (this.data.length == 0) return false
      this.loading = true
      var reqData = []
      this.data.forEach(element => {
        var item = { Key: element.Id, Value: element.LocalId }
        reqData.push(item)
      })
      this.$http.post('/TD/TD_InStorage/InBlankTray', reqData)
        .then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false
          } else {
            this.$message.error(resJson.Msg)
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
      this.$refs.trayChoose.openChoose()
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
            resolve()
          })
        }
      })
    },
    handleTrayChoose(trays) {
      this.data = [...trays]
    }
  }
}
</script>
