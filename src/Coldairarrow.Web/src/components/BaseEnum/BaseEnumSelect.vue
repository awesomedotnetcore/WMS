<template>
  <a-select v-model="curValue" :placeholder="enumData.Name" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in enumData.EnumItems" :key="item.Value" :value="item.Value">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    code: { type: String, required: true },
    value: { type: String, required: true }
  },
  data() {
    return {
      curValue: '',
      enumData: {}
    }
  },
  watch: {
    code(code) {
      this.getEnum()
    },
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getEnum()
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
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>