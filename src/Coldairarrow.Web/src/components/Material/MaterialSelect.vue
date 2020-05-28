<template>
  <div>
    <a-auto-complete placeholder="选择物料" v-model="curValue" @select="onSelect" @search="handleSearch">
      <template slot="dataSource">
        <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
      </template>
      <a-input>
        <a-button slot="suffix" style="margin-right: -12px" class="search-btn" type="primary" @click="handleOpenChoose">
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
      dropdownVisible: false,
      curValue: '',
      keyword: '',
      timeout: null,
      dataSource: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
      this.reload('')
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
      var rowQuery = this.dataSource.filter((val, index, arr) => { return val.Id == value })
      if (rowQuery.length > 0) {
        this.$emit('select', rowQuery[0])
      }
    },
    handleSearch(value) {
      this.reload(value)
    },
    handleOpenChoose() {
      this.$refs.materialChoose.openChoose()
    },
    handleChoose(selectedRows) {
      this.dataSource = [...selectedRows]
      var row = selectedRows[0]
      this.curValue = row.Id
      this.$emit('input', this.curValue)
      this.$emit('select', row)
    }
  }
}
</script>