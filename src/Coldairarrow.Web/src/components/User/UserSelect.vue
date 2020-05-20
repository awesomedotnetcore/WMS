<template>
  <a-select ref="select" :allowClear="true" :showSearch="true" :filterOption="false" @search="handleSearch" @change="handleChange" v-model="thisValue" v-bind="$attrs">
    <a-select-option v-for="item in listData" :key="item.Id">{{ item.RealName }}({{ item.UserName }})</a-select-option>
  </a-select>
</template>

<script>
let qGlobal = ''
let timeout = null

export default {
  props: {
    value: { type: String, default: '', required: false }
  },
  mounted() {
    this.thisValue = this.value
    this.reload()
  },
  data() {
    return {
      listData: [],
      thisValue: ''
    }
  },
  watch: {
    value(value) {
      this.thisValue = value
    }
  },
  methods: {
    reload(q) {
      qGlobal = q
      clearTimeout(timeout)
      timeout = setTimeout(() => {
        this.$http.get('/Base_Manage/Base_User/GetSelectData?keyword=' + escape(q === undefined ? '' : q) + '&selected=' + this.thisValue)
          .then(resJson => {
            if (resJson.Success && q == qGlobal) {
              this.listData = resJson.Data
            }
          })
      }, 300)
    },
    handleSearch(value) {
      this.reload(value)
    },
    handleChange(value) {
      this.$emit('input', value)
    }
  }
}
</script>
