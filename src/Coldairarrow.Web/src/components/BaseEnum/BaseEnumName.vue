<template>
  <div class="editable-cell">
    <div class="editable-cell-text-wrapper">{{ enumName || ' ' }}</div>
  </div>
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
      enumItem: []
    }
  },
  watch: {
    code(code) {
      this.getEnumItem()
    }
  },
  mounted() {
    this.getEnumItem()
  },
  computed: {
    enumName: function () {
      var search = this.enumItem.filter((item, index, arr) => { return item.Value === this.value })
      if (search.length <= 0) return ' '
      else return search[0].Name
    }
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
            localStorage.setItem(expKey, moment().minute(15).toJSON())
          })
      } else {
        var listData = JSON.parse(enumJson)
        this.enumItem = listData
      }
    }
  }
}
</script>