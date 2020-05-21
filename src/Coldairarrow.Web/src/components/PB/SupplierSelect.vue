<template>
  <a-select v-model="curValue" placeholder="供应商/下料点" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in supplierList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    value: { type: String, required: false }
  },
  data() {
    return {
      curValue: '',
      supplierList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getSupplierData()
  },
  methods: {
    getSupplierData() {
        this.$http
        .post('/PB/PB_Supplier/QueryAllData')
        .then(resJson => {
          this.supplierList = resJson.Data          
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>