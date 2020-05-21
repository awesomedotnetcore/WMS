<template>
  <a-select v-model="curValue" placeholder="单位" @select="handleSelected" v-bind="$attrs">
    <a-select-option v-for="item in measureList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
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
      measureList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  mounted() {
    this.curValue = this.value
    this.getMeasureData()
  },
  methods: {
    getMeasureData() {
        this.$http
        .post('/PB/PB_Measure/QueryAllData')
        .then(resJson => {
          this.measureList = resJson.Data          
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>