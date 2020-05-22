<template>
  <div class="editable-cell">
    <div class="editable-cell-text-wrapper">{{ enumName }}</div>
  </div>
</template>
<script>
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, required: true }
  },
  data() {
    return {
      enumData: {}
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
      var search = this.enumData.EnumItems.filter((item, index, arr) => { return item.Value === this.value })
      if (search.length <= 0) return ' '
      else return search[0].Name
    }
  },
  methods: {
    getEnum() {
      var storageKey = 'Enum_' + this.code
      var enumJson = localStorage.getItem(storageKey)
      if (enumJson === null || enumJson === '') {
        this.$http.get('/Base/Base_Enum/GetByCode?code=' + this.code)
          .then(resJson => {
            this.enumData = resJson.Data
            enumJson = JSON.stringify(resJson.Data)
            localStorage.setItem(storageKey, enumJson)
          })
      } else {
        this.enumData = JSON.parse(enumJson)
      }
    }
  }
}
</script>