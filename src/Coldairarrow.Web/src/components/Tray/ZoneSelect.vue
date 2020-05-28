<template>
  <a-select v-model="curValue" placeholder="托盘分区" @select="handleSelected" v-bind="$attrs">
    <a-select-option
      v-for="item in listData"
      :key="item.Id"
      :value="item.Id"
    >{{ item.Name }}({{ item.Code }})</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    typeId: { type: String, default: '', required: false },
    trayId: { type: String, default: '', required: false },
    value: { type: String, required: true }
  },
  data() {
    return {
      curValue: '',
      listData: []
    }
  },
  watch: {
    typeId(typeId) {
      this.getListData()
    },
    value(value) {
      this.curValue = value
    },
    trayId(trayId) {
      this.getListData()
    }
  },
  mounted() {
    this.curValue = this.value
    this.getListData()
  },
  methods: {
    getListData() {
      if (this.typeId != '') {
        this.$http.post('/PB/PB_TrayZone/GetDataListByType?typeId=' + this.typeId).then(resJson => {
          this.listData = resJson.Data
        })
      } else if (this.trayId != '') {
        this.$http.post('/PB/PB_TrayZone/GetDataListByTray?trayId=' + this.trayId).then(resJson => {
          this.listData = resJson.Data
        })
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>