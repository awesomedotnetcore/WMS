<template>
  <div>
    <a-row>
      <a-col :md="20" :xs="20">
        <a-select placeholder="选择货位" :disabled="disabled" :style="{width:'100%'}" :size="size" v-model="curValue" @select="onSelect" @search="handleSearch" :allowClear="true" :showSearch="true" :filterOption="false">
          <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
        </a-select>
      </a-col>
      <a-col :md="2" :xs="2">
        <a-button type="primary" :disabled="disabled" :size="size" @click="handleOpenChoose">
          <a-icon type="search" />
        </a-button>
      </a-col>
    </a-row>
    <location-choose ref="locationChoose" :storid="storid" @onChoose="handleChoose"></location-choose>
  </div>
</template>
<script>
import LocationChoose from './LocationChoose'
export default {
  props: {
    size: { type: String, default: 'default', required: false },
    disabled: { type: Boolean, default: false, required: false },
    value: { type: String, default: '', required: false },
    storid: { type: String, default: '', required: false }
  },
  components: {
    LocationChoose
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
      if (q === '' && (this.curValue === '' || this.curValue === undefined || this.curValue === null)) return false
      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        this.$http.post('/PB/PB_Location/GetQueryData', { Id: this.curValue, StorId: this.storid, Keyword: q, Take: 10 })
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
      this.$refs.locationChoose.openChoose()
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