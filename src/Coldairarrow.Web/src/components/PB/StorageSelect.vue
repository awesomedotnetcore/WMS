<template>
  <a-select v-model="curValue" placeholder="仓库" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in sstorageList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
  </a-select>
</template>
<script>
export default {
  props: {
    value: { type: String, required: false }
  },
  data() {
    return {
      curValue: '',
      storageList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getStorageData()
  },
  methods: {
    getStorageData() {
        this.$http
        .post('/PB/PB_Storage/QueryStorageData')
        .then(resJson => {
          this.storageList = resJson.Data          
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>