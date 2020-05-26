<template>
  <a-auto-complete placeholder="选择物料" v-model="curValue" @select="onSelect" @search="handleSearch">
    <template slot="dataSource">
      <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
    </template>
    <a-input>
      <a-button slot="suffix" style="margin-right: -12px" class="search-btn" type="primary">
        <a-icon type="search" />
      </a-button>
    </a-input>
  </a-auto-complete>
</template>
<script>
export default {
  props: {
    value: { type: String, default: '', required: false }
  },
  data() {
    return {
      curValue: '',
      keyword: '',
      timeout: null,
      dataSource: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.reload('')
  },
  methods: {
    reload(q) {
      this.keyword = q
      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        this.$http.get('/PB/PB_Material/GetQueryData?keyword=' + encodeURI(q === undefined ? '' : q) + '&id=' + this.curValue)
          .then(resJson => {
            if (resJson.Success && q == this.keyword) {
              this.dataSource = resJson.Data
            }
          })
      }, 300)
    },
    onSelect(value, option) {
      this.$emit('input', value)
    },
    handleSearch(value) {
      this.reload(value)
    }
  }
}
</script>