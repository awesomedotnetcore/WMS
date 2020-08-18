<template>
  <div>
    <a-input v-model="curValue" @change="handlerInputChange" placeholder="物料条码" v-bind="$attrs">
      <a-icon slot="addonBefore" type="gold" @click="$refs.materialChoose.openChoose()"></a-icon>
      <a-icon slot="addonAfter" type="scan" @click="$refs.barcodeReader.openReader()"></a-icon>
    </a-input>
    <barcode-reader ref="barcodeReader" @success="handlerReaderDone"></barcode-reader>
    <material-choose ref="materialChoose" @select="handlerChoose"></material-choose>
  </div>
</template>

<script>
import BarcodeReader from './BarcodeReader'
import MaterialChoose from './MaterialChoose'
export default {
  inheritAttrs: false,
  components: {
    BarcodeReader,
    MaterialChoose
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
    },
    handlerChoose(material) {
      this.curValue = material.BarCode
      this.$emit('input', this.curValue)
    }
  }
}
</script>

<style>
</style>