<template>
  <a-select v-model="curValue" placeholder="请选择仓库" @select="handleSelected" :allowClear="true" v-bind="$attrs">
    <a-select-option v-for="item in storageList" :key="item.Id" :value="item.Id">{{ item.Name }} {{ item.Code }}</a-select-option>
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
      storageList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getMeasureData()
  },
  methods: {
    getMeasureData() {
      this.$http
        .post('/PB/PB_Storage/QueryStorageData')
        .then(resJson => {
          this.storageList = resJson.Data
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
      var rowQuery = this.storageList.filter((item, index, arr) => {
        return item.Id == val
      })
      if (rowQuery.length > 0) {
        this.$emit('select', { ...rowQuery[0] })
        // this.$emit('input', val)
      }
    }
  }
}
</script>