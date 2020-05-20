<template>
  <a-select v-model="curValue" placeholder="TEST" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in enumItem" :key="item.Value" :value="item.Value">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
import moment from 'moment'
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, required: false }
  },
  data() {
    return {
      curValue: '',
      enumItem: []
    }
  },
  watch: {
    code(code) {
      this.getEnumItem()
    },
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
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