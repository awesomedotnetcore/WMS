<template>
  <div>
    <a-row>
        <a-col :span="20">
          <a-select v-model="curValue" placeholder="供应商/下料点" @select="handleSelected" v-bind="$attrs">
            <a-select-option v-for="item in supplierList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :span="2">
          <a-button type="primary" @click="handleOpenChoose">
            <a-icon type="search" />
          </a-button>
        </a-col>
      </a-row>  
      <supplier-choose ref="supplierChoose" @onChoose="handleChoose"></supplier-choose>
  </div>  
</template>
<script>
import SupplierChoose from './SupplierChoose'
export default {
  props: {
    value: { type: String, required: false }
  },
  components:{
    SupplierChoose
  },
  data() {
    return {
      curValue: '',
      supplierList: []
    }
  },
  watch: {
    value(value) {
      this.getSupplierData(value)
    }
  },
  methods: {
    getSupplierData(cid) {
      if (cid !== '' && cid !== undefined && cid !== null) {
        this.$http.post('/PB/PB_Supplier/GetTheData', { id: cid }).then(resJson => {
          this.supplierList.push(resJson.Data)
          this.curValue = this.value
        })
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
    },
    handleOpenChoose(){
      this.$refs.supplierChoose.openChoose()
    },
    handleChoose(selectedRows){
      this.supplierList = [...selectedRows]
      var row = selectedRows[0]
      this.curValue = row.Id
      this.$emit('input', this.curValue)
      this.$emit('select', row)
    }
  }
}
</script>