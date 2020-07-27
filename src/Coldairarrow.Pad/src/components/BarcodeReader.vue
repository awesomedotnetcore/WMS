<template>
  <a-modal :visible="visible" title="扫描" :footer="null" @cancel="handlerClose">
    <a-descriptions v-if="devices.length>1">
      <a-descriptions-item label="选择摄像头">
        <a-select v-model="deviceId" @select="readerCode">
          <a-select-option v-for="device in devices" :key="device.deviceId" :value="device.deviceId">{{ device.label }}</a-select-option>
        </a-select>
      </a-descriptions-item>
    </a-descriptions>
    <video :id="videoId" width="300" height="200" style="border: 1px solid gray"></video>
  </a-modal>
</template>

<script>
import { BrowserMultiFormatReader } from '@zxing/library';
export default {
  props: {
    multiple: { type: Boolean, required: false, default: false }
  },
  created() {
    this.uuid()
  },
  data() {
    return {
      videoId: '',
      visible: false,
      deviceId: null,
      devices: [],
      codeReader: {}
    }
  },
  methods: {
    uuid() {
      var s = []
      var hexDigits = "0123456789abcdef"
      for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1)
      }
      s[14] = "4"
      s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1)
      s[8] = s[13] = s[18] = s[23] = "-"
      var uuid = 'video_' + s.join("")
      this.videoId = uuid
    },
    init() {
      this.codeReader = new BrowserMultiFormatReader()
    },
    openReader() {
      this.visible = true
      this.init()
      this.getDeviceId()
    },
    getDeviceId() {
      this.codeReader
        .getVideoInputDevices()
        .then(devices => {
          if (devices.length > 0) {
            this.devices = devices
            this.deviceId = devices[devices.length - 1].deviceId
            if (!this.multiple) {
              this.readerCodeOnce()
            } else {
              this.readerCodeContinuously()
            }
          } else {
            this.$message.error("没有找到摄像头")
          }
        })
        .catch(err => this.$message.error('获取摄像头列表出错 ' + err));
    },
    readerCode() {
      if (!this.multiple) {
        this.readerCodeOnce()
      } else {
        this.readerCodeContinuously()
      }
    },
    readerCodeOnce() {
      this.codeReader
        .decodeOnceFromVideoDevice(this.deviceId, this.videoId)
        .then(result => this.readerDoneOnce(result))
      //.catch(err => this.$message.error('扫码出错：' + err));
    },
    readerCodeContinuously() {
      this.codeReader
        .decodeFromVideoDevice(this.deviceId, this.videoId, this.readerDoneContinuously)
        .then(result => this.readerDoneContinuously(result))
        .catch(err => this.$message.error('连续扫码出错：' + err));
    },
    readerDoneOnce(result) {
      console.log(result.text)
      this.$message.success(result.text)
      this.$emit('success', result.text)
      this.handlerClose()
    },
    readerDoneContinuously(result) {
      console.log(result)
      if (result) {
        this.$message.success(result.text)
        this.$emit('success', result.text)
      }
    },
    handlerClose() {
      this.codeReader.reset()
      this.visible = false
    }
  }
}
</script>

<style>
</style>