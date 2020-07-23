<template>
  <div>
    <a-button type="primary" @click="handlerReader">扫描</a-button>
    <barcode-reader ref="barcodeReader" @success="readerSuccess"></barcode-reader>
  </div>
</template>

<script>
import BarcodeReader from '../../components/BarcodeReader'
export default {
  components: {
    BarcodeReader
  },
  data() {
    return {
    }
  },
  methods: {
    handlerReader() {
      this.$refs.barcodeReader.openReader()
    },
    readerSuccess(result) {
      this.$message.success(result.text)
    }
  }
}
</script>

<style>
</style>