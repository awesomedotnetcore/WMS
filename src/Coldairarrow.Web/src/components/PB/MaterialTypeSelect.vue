<template>
  <a-tree-select allowClear :dropdownStyle="{ maxHeight: '400px', overflow: 'auto' }" :treeData="TreeData" placeholder="物料类型" treeDefaultExpandAll v-model="curValue" @select="handleSelected" v-bind="$attrs"></a-tree-select>
</template>
<script>
export default {
  props: {
    value: { type: String, required: false }
  },
  data() {
    return {
      curValue: '',
      TreeData: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getEnumItem()
  },
  methods: {
    getEnumItem() {
      this.$http
        .post('/PB/PB_MaterialType/GetTreeDataList')
        .then(resJson => {
          this.TreeData = resJson.Data
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>