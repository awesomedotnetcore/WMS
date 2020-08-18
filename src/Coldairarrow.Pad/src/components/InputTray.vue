<template>
  <div>
    <a-input v-model="curValue" @change="handlerInputChange" placeholder="托盘编码" v-bind="$attrs">
      <a-icon slot="addonBefore" type="border-outer" @click="$refs.trayChoose.openChoose()"></a-icon>
      <a-icon slot="addonAfter" type="scan" @click="$refs.barcodeReader.openReader()"></a-icon>
    </a-input>
    <barcode-reader ref="barcodeReader" @success="handlerReaderDone"></barcode-reader>
    <tray-choose ref="trayChoose" @select="handlerChoose"></tray-choose>
  </div>
</template>

<script>
import BarcodeReader from './BarcodeReader'
import TrayChoose from './TrayChoose'
export default {
  inheritAttrs: false,
  components: {
    BarcodeReader,
    TrayChoose
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
    handlerInputChange() {
      this.$emit('input', this.curValue)
    },
    handlerReaderDone(result) {
      this.curValue = result
      this.$emit('input', result)
    },
    handlerChoose(tray) {
      this.curValue = tray.Code
      this.$emit('input', this.curValue)
    }
  }
}
</script>

<style>
</style>