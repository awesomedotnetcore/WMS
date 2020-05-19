<template>
  <a-select v-model="value" @select="handleSelected">
    <a-select-option v-for="item in enumItem" :key="item.Value" :value="item.Value">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
import moment from 'moment'
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, default: null, required: false }
  },
  data() {
    return {
      enumItem: []
    }
  },
  watch: {
    value(code) {
      this.getEnumItem()
    }
  },
  mounted() {
    this.getEnumItem()
  },
  methods: {
    moment,
    getEnumItem() {
      var storageKey = 'EnumItem_' + this.code
      var enumJson = localStorage.getItem(storageKey)
      var expKey = 'EnumItemExp_' + this.code
      var expValue = localStorage.getItem(expKey)
      if (enumJson === null || enumJson === '' || this.moment(expValue).isAfter(moment())) {
        this.$http.get('/Base/Base_EnumItem/GetListByCode?code=' + this.code)
          .then(resJson => {
            this.enumItem = resJson.Data
            enumJson = JSON.stringify(resJson.Data)
            localStorage.setItem(storageKey, enumJson)
            localStorage.setItem(expKey, moment().minute(15).format())
          })
      } else {
        var listData = JSON.parse(enumJson)
        this.enumItem = listData
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>