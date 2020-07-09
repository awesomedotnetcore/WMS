<template>
  <div>
    <div class="table-operator">
      <a-button :disabled="disabled" type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRows)" :disabled="!hasSelected() || disabled">删除</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :pagination="false" :dataSource="data" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="PlanNum" slot-scope="text, record">
        <a-input-number size="small" :disabled="disabled" :value="text" :min="1" @change="e=>handleValChange(e,'PlanNum',record)"></a-input-number>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a :disabled="disabled" @click="handleDelete([record])">删除</a>
        </template>
      </span>
    </a-table>
    <material-choose ref="materialChoose" @onChoose="handerMaterialChoose"></material-choose>
  </div>
</template>

<script>
import MaterialChoose from '../../../components/Material/MaterialChoose'
const columns = [
  { title: '物料名称', dataIndex: 'Material.Name' },
  { title: '物料编码', dataIndex: 'Material.Code' },
  { title: '物料规格', dataIndex: 'Material.Spec' },
  { title: '物料单价', dataIndex: 'Material.Price' },
  { title: '数量', dataIndex: 'PlanNum', scopedSlots: { customRender: 'PlanNum' } },
  { title: '总价', dataIndex: 'Amount' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    MaterialChoose
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false }
  },
  data() {
    return {
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
  },
  methods: {
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.materialChoose.openChoose()
    },
    handerMaterialChoose(materials) {
      materials.forEach(material => {
        var isNew = true
        this.data.forEach(element => {
          if (element.MaterialId === material.Id && isNew) {
            isNew = false
          }
        })
        if (isNew) {
          this.tempId += 1
          var curDetail = { Id: 'newid_' + this.tempId.toString(), MaterialId: material.Id, Material: material, MeasureId: material.MeasureId, Price: material.Price }
          this.data.push({ ...curDetail })
          this.$emit('input', [...this.data])
        }
      })
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
      if (name === 'PlanNum') {
        item.Amount = item.PlanNum * item.Price
      }
      this.$emit('input', [...this.data])
    }
  }
}
</script>