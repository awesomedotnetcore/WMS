<template>
  <a-select v-model="curValue" placeholder="选择货区" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in listData" :key="item.Id" :value="item.Id">{{ item.Name }}({{ item.Code }})</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
   // typeId: { type: String, default: '', required: false },
    storId: { type: String, default: '', required: false },
    value: { type: String, required: true }
  },
  data() {
    return {
      curValue: '',
      listData: []
    }
  },
  watch: {
    // typeId(typeId) {
    //   this.getListData()
    // },
    value(value) {
      this.curValue = value
    },
    storId(storId) {
      this.getListData()
    }
  },
  mounted() {
    this.curValue = this.value
    this.getListData()
  },
  methods: {
    getListData() {
      // if (this.typeId != '') {
      //   this.$http.post('/PB/PB_StorArea/GetDataListByType?typeId=' + this.typeId).then(resJson => {
      //     this.listData = resJson.Data
      //   })
      // } else 
      if (this.storId != '') {
        this.$http.post('/PB/PB_StorArea/GetDataListByStor?StorId=' + this.storId).then(resJson => {
          this.listData = resJson.Data
        })
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
      var rowQuery = this.listData.filter((item, index, arr) => { return item.Id == val })
      if (rowQuery.length > 0) {
        this.$emit('select', { ...rowQuery[0] })
      }
    }
  }
}
</script>