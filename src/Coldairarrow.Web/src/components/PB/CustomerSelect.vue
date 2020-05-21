<template>
  <a-select v-model="curValue" placeholder="客户/投料点" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in customerList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
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
      customerList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getCustomerData()
  },
  methods: {
    getCustomerData() {
        this.$http
        .post('/PB/PB_Customer/QueryAllData')
        .then(resJson => {
          this.customerList = resJson.Data          
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>