<template>
  <a-select v-model="curValue" placeholder="托盘分区" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in listData" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    typeId: { type: String, required: true },
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
    }
  },
  mounted() {
    this.curValue = this.value
    this.getListData()
  },
  methods: {
    getListData() {
      this.$http.post('/PB/PB_TrayZone/GetDataListByType?typeId=' + this.typeId)
        .then(resJson => {
          this.listData = resJson.Data
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>