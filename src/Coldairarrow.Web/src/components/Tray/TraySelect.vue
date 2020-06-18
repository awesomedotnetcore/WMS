<template>
  <div>
    <a-row>
      <a-col :span="20">
        <a-select placeholder="选择托盘" :style="{width:'100%'}" :size="size" v-model="curValue" @select="onSelect" @search="handleSearch" :allowClear="true" :showSearch="true" :filterOption="false">
          <a-select-option v-for="item in dataSource" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
        </a-select>
      </a-col>
      <a-col :span="2">
        <a-button type="primary" :size="size" @click="handleOpenChoose">
          <a-icon type="search" />
        </a-button>
      </a-col>
    </a-row>
    <tray-choose ref="trayChoose" @onChoose="handleChoose"></tray-choose>
  </div>
</template>
<script>
import TrayChoose from './TrayChoose'
export default {
  props: {
    materialId: { type: String, default: '', required: false },
    locartalId: { type: String, default: '', required: false },
    size: { type: String, default: 'default', required: false },
    value: { type: String, required: false }
  },
  components: {
    TrayChoose
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
        this.$http.post('/PB/PB_Tray/GetQueryData', { Id: this.curValue, Keyword: q, Take: 10, MaterialId: this.materialId, LocationId: this.locartalId })
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
      this.$refs.trayChoose.openChoose()
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