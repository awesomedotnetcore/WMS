<template>
  <a-select v-model="curValue" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in listData" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    value: { type: String, default: '', required: false }
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
      this.$http.get('/Base/Base_UserStor/GetStorage')
        .then(resJson => {
          this.listData = resJson.Data
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>