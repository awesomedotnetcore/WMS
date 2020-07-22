<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button :disabled="disabled" type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.Id"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <template slot="PlanNum" slot-scope="text, record">
        <a-input-number  size="small" :disabled="disabled" :value="text" :max="record.PlanNum" :min="1" @change="e=>handleValChange(e,'PlanNum',record)"></a-input-number>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <!-- <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" /> -->
          <a :disabled="disabled" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <sendmaterial-list ref="sendmaterialList" @choose="handlerDetailSubmit"></sendmaterial-list>
  </a-card>
</template>

<script>
import SendmaterialList from './SendMaterialList'

const columns = [
  // { title: '发货Id', dataIndex: 'SendId', width: '10%' },
  // { title: '仓库ID', dataIndex: 'StorId', width: '10%' },
  { title: '物料', dataIndex: 'Material.Name' },//MaterialId
  { title: '单位', dataIndex: 'Measure.Name'},//MeasureId
  { title: '批次号', dataIndex: 'BatchNo'},
  { title: '库存数量', dataIndex: 'LocalNum'},
  { title: '出库数量', dataIndex: 'PlanNum' , scopedSlots: { customRender: 'PlanNum' }},
  { title: '单价', dataIndex: 'Price'},
  // { title: '发货数量', dataIndex: 'SendNum', width: '10%' },
  // { title: '总价', dataIndex: 'Amount', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    SendmaterialList
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false }
  },
  watch: {
    value(val) {
      this.data = [...this.value]
    }
  },
  mounted() {
    this.data = [...this.value]
    this.getCurStorage()
    // this.getDataList()
  },
  data() {
    return {
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: []
    }
  },
  methods: {
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
          if (this.storage) {
            this.columns = columns
          }
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.sendmaterialList.openChoose()
      // this.$refs.materialReport.openChoose()
      //this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id)
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
        item.Id = 'newid_' + this.tempId
        //  item.Location = { ...element.Location }
        item.localId = element.localId
        item.MaterialId = element.MaterialId
        item.MeasureId = element.MeasureId
   
        item.BatchNo = element.BatchNo
        item.LocalNum = element.Num
        item.PlanNum = 1
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