<template>
  <a-select v-model="curValue" :placeholder="enumData.Name" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in enumItem" :key="item.Value" :value="item.Value">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
import moment from 'moment'
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, required: true }
  },
  data() {
    return {
      curValue: '',
      enumData: {},
      enumItem: []
    }
  },
  watch: {
    code(code) {
      this.getEnumItem()
      this.getEnum()
    },
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getEnumItem()
    this.getEnum()
  },
  methods: {
    moment,
    getEnum() {
      var storageKey = 'Enum_' + this.code
      var enumJson = localStorage.getItem(storageKey)
      var expKey = 'EnumExp_' + this.code
      var expValue = localStorage.getItem(expKey)
      if (enumJson === null || enumJson === '' || this.moment(expValue).isAfter(moment())) {
        this.$http.get('/Base/Base_Enum/GetByCode?code=' + this.code)
          .then(resJson => {
            this.enumData = resJson.Data
            enumJson = JSON.stringify(resJson.Data)
            localStorage.setItem(storageKey, enumJson)
            localStorage.setItem(expKey, moment().minute(15).format())
          })
      } else {
        this.enumData = JSON.parse(enumJson)
      }
    },
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
        this.enumItem = JSON.parse(enumJson)
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>