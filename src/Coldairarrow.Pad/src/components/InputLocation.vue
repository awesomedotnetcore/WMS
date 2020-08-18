<template>
  <div>
    <a-input v-model="curValue" @change="handlerInputChange" placeholder="货位编码" v-bind="$attrs">
      <a-icon slot="addonBefore" type="table" @click="$refs.localChoose.openChoose()"></a-icon>
      <a-icon slot="addonAfter" type="scan" @click="$refs.barcodeReader.openReader()"></a-icon>
    </a-input>
    <barcode-reader ref="barcodeReader" @success="handlerReaderDone"></barcode-reader>
    <local-choose ref="localChoose" @select="handlerChoose"></local-choose>
  </div>
</template>

<script>
import BarcodeReader from './BarcodeReader'
import LocalChoose from './LocalChoose'
export default {
  inheritAttrs: false,
  components: {
    BarcodeReader,
    LocalChoose
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
    handlerChoose(local) {
      this.curValue = local.Code
      this.$emit('input', this.curValue)
    }
  }
}
</script>

<style>
</style>