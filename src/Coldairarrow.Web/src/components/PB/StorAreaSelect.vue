<template>
  <a-checkbox-group v-model="curValue" :disabled="disabled" @change="handleSelected" v-bind="$attrs" style="width:100%;">
    <a-row style="width:100%;">
      <a-col v-for="item in areaList" :span="6" :key="item.Id">
        <a-checkbox :value="item.Id" autocomplete="off">
          {{item.Name}}
        </a-checkbox>
      </a-col>
    </a-row>
  </a-checkbox-group>
</template>
<script>
export default {
  props: {
    value: { type: Array, required: false },
    disabled:{type:Boolean,default:false,required:false}
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
    }
  },
  mounted() {
    this.curValue = this.value
    this.getAreaData()
  },
  methods: {
    getAreaData() {
        this.$http
        .post('/PB/PB_StorArea/Query')
        .then(resJson => {
          this.areaList = resJson.Data          
        })
    },
    handleSelected(val) {
      this.$emit('input', val)
    }
  }
}
</script>