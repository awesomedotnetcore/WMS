<template>
  <div>
    <a-tag v-for="item in areaList" color="cyan" :key="item.Id">{{ item.Name }}</a-tag>
  </div>
</template>
<script>
export default {
  props: {
    value: { type: Array, required: false },
    disabled: { type: Boolean, default: false, required: false }
  },
  data() {
    return {
      curValue: [],
      areaList: []
    }
  },
  watch: {
    value(value) {
      this.curValue = value
      this.getAreaData()
    }
  },
  mounted(){
    this.areaList=[]
  },
  methods: {
    getAreaData() {
      this.areaList=[]
      this.$http.post('/PB/PB_StorArea/Query').then(resJson => {
        resJson.Data.forEach(element => {
          if (this.curValue.indexOf(element.Id) > -1) {
            this.areaList.push(element)
          }
        })
      })
    }
  }
}
</script>