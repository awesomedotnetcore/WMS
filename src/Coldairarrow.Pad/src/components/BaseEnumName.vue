<template>
  <div class="editable-cell">
    <div class="editable-cell-text-wrapper">{{ enumName }}</div>
  </div>
</template>
<script>
import EnumSvc from '../api/PB/EnumSvc'
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, required: true }
  },
  data() {
    return {
      enumData: {},
      enumItems: []
    }
  },
  watch: {
    code(code) {
      this.getEnum()
    }
  },
  mounted() {
    this.getEnum()
  },
  computed: {
    enumName: function () {
      var search = this.enumItems.filter((item, index, arr) => { return item.Value === this.value })
      var resultName = ''
      if (search.length <= 0) resultName = ' '
      else {
        resultName = search[0].Name
      }
      return resultName
    }
  },
  methods: {
    getEnum() {
      var storageKey = 'Enum_' + this.code
      var dataJson = localStorage.getItem(storageKey)
      if (dataJson === null || dataJson === '') {
        EnumSvc.GetByCode(this.code)
          .then(resJson => {
            this.enumData = { ...resJson.Data }
            this.enumItems = [...resJson.Data.EnumItems]
            dataJson = JSON.stringify(resJson.Data)
            localStorage.setItem(storageKey, dataJson)
          })
      } else {
        this.enumData = JSON.parse(dataJson)
        this.enumItems = [...this.enumData.EnumItems]
      }
    }
  }
}
</script>