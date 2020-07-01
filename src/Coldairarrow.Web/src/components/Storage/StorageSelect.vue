<template>
  <a-select v-model="curValue" placeholder="请选择仓库" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in listData" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    value: { type: String, required: true }
  },
  data() {
    return {
      curValue: '',
      listData: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getListData()
  },
  methods: {
    getListData() {
      this.$http.get('/Base/Base_UserStor/GetStorage').then(resJson => {
        this.listData = resJson.Data
      })
    },
    handleSelected(val) {
      this.$emit('input', val)
      var rowQuery = this.listData.filter((item, index, arr) => {
        return item.Id == val
      })
      if (rowQuery.length > 0) {
        this.$emit('select', { ...rowQuery[0] })
      }
    }
  }
}
</script>