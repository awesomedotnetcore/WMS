<template>
  <a-modal :visible="visible" title="扫描" :destroyOnClose="true" @cancel="visible=false" @ok="visible=false">
    <a-select v-model="deviceId" @select="readerCode()">
      <a-select-option v-for="device in videoInputDevices" :key="device.deviceId" :value="device.deviceId">{{ device.label }}</a-select-option>
    </a-select>
    <video id="video" width="300" height="200" style="border: 1px solid gray"></video>
  </a-modal>
</template>

<script>
import { BrowserMultiFormatReader } from '@zxing/library';
export default {
  components: {
  },
  data() {
    return {
      visible: false,
      deviceId: null,
      videoInputDevices: [],
      codeReader: {}
    }
  },
  methods: {
    init() {
      this.codeReader = new BrowserMultiFormatReader()
    },
    openReader() {
      this.visible = true
      this.init()
      this.getDeviceId()
      this.readerCode()
    },
    getDeviceId() {
      this.codeReader
        .getVideoInputDevices()
        .then(videoInputDevices => {
          this.videoInputDevices = videoInputDevices
          var deviceString = ''
          var did = ''
          videoInputDevices.forEach(device => {
            deviceString += `label:${device.label},deviceId:${device.deviceId}  `
            did = device.deviceId
          })
          this.deviceId = did
          this.$message.success(deviceString)
        })
        .catch(err => { console.error(err); this.$message.error(err) });
    },
    readerCode() {
      this.$message.success(this.deviceId)
      this.codeReader.reset()
      this.codeReader
        .decodeOnceFromVideoDevice(this.deviceId, 'video')
        .then(result => { console.log(result); this.readerDone(result) })
        .catch(err => { console.error(err); this.$message.error(err) });
    },
    readerDone(result) {
      console.log(result.text)
      this.$message.success(result.text)
      this.$emit('success', result)
      this.visible = false
    }
  }
}
</script>

<style>
</style>