<template>
  <div>
    <a-row>
      <a-col :span="20">
        <a-select placeholder="选择库存" v-model="curValue" @select="onSelect" @search="handleSearch" :allowClear="true" :showSearch="true" :filterOption="false">
          <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Material.Name }}( {{ item.Material.Code }} )</a-select-option>
        </a-select>
      </a-col>
      <a-col :span="2">
        <a-button type="primary" @click="handleOpenChoose">
          <a-icon type="search" />
        </a-button>
      </a-col>
    </a-row>
    <Localmaterial-choose ref="localMaterialChoose" @onChoose="handleChoose"></Localmaterial-choose>
  </div>
</template>
<script>
import LocalmaterialChoose from './LocalMaterialChoose'
export default {
  props: {
    value: { type: String, default: '', required: false }
  },
  components: {
    LocalmaterialChoose
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
      if (this.curValue !== '' && this.curValue !== undefined && this.curValue !== null) {
        this.reload('')
      }
    }
  },
  mounted() {
    this.curValue = this.value
    if (this.curValue !== '' && this.curValue !== undefined && this.curValue !== null) {
      this.reload('')
    }
  },
  methods: {
    reload(q) {
      this.keyword = q
      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        this.$http.post('/IT/IT_LocalMaterial/GetQueryData', { Id: this.curValue, Keyword: q, Take: 10 })
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
      this.$refs.localMaterialChoose.openChoose()
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