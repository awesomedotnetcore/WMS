<template>
  <div>
    <a-input-search v-model="curValue" @change="handlerInputChange" @search="handlerReader" v-bind="$attrs">
      <a-icon slot="enterButton" type="scan"></a-icon>
    </a-input-search>
    <barcode-reader ref="barcodeReader" @success="handlerReaderDone"></barcode-reader>
  </div>
</template>

<script>
import BarcodeReader from './BarcodeReader'
export default {
  inheritAttrs: false,
  components: {
    BarcodeReader
  },
  props: {
    value: { type: String, required: false, default: '' }
  },
  data() {
    return {
      curValue: ''
    }
  },
  watch: {
    value(value) {
      this.curValue = value
    }
  },
  methods: {
    handlerReader() {
      this.$refs.barcodeReader.openReader()
    },
    handlerInputChange() {
      this.$emit('input', this.curValue)
    },
    handlerReaderDone(result) {
      this.curValue = result
      this.$emit('input', result)
    }
  }
}
</script>

<style>
</style>