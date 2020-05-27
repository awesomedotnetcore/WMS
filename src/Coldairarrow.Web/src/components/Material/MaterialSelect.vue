<template>
  <div>
    <a-auto-complete placeholder="选择物料" v-model="curValue" @select="onSelect" @search="handleSearch">
      <template slot="dataSource">
        <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
      </template>
      <a-input>
        <a-button slot="suffix" style="margin-right: -12px" class="search-btn" type="primary" @click="$ref.materialChoose.openChoose">
          <a-icon type="search" />
        </a-button>
      </a-input>
    </a-auto-complete>
    <material-choose ref="materialChoose" @onChoose="handleChoose"></material-choose>
  </div>
</template>
<script>
import MaterialChoose from './MaterialChoose'
export default {
  props: {
    value: { type: String, default: '', required: false }
  },
  components: {
    MaterialChoose
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
        this.$http.post('/PB/PB_Material/GetQueryData', { Id: this.curValue, Keyword: q, Take: 10 })
          .then(resJson => {
            if (resJson.Success && q == this.keyword) {
              this.dataSource = resJson.Data
            }
          })
      }, 500)
    },
    onSelect(value, option) {
      this.$emit('input', value)
    },
    handleSearch(value) {
      this.reload(value)
    },
    handleChoose(selectedRows) {
      this.dataSource = [...selectedRows]
      this.curValue = selectedRows[0].Id
    }
  }
}
</script>