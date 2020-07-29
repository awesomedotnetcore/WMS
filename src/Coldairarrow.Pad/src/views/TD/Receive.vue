<template>
  <a-card title="生产入库" :bordered="false">
    <a-button slot="extra" type="primary" size="small" @click="handlerSubmit">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="TrayCode">
        <input-code v-model="entity.TrayCode" placeholder="托盘号"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="MaterialCode">
        <input-code v-model="entity.MaterialCode" placeholder="物料"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="MaterialCode">
        <a-input-number v-model="entity.Num" :min="1" />
      </a-form-model-item>
    </a-form-model>
  </a-card>
</template>

<script>
import InputCode from '../../components/InputBarcode'

export default {
  components: {
    InputCode
  },
  data() {
    return {
      loading: false,
      entity: {},
      listData: [],
      rules: {
        MaterialCode: [{ required: true, message: '请输入物料编码', trigger: 'blur' }],
        TrayCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }]
      }
    }
  },
  methods: {
    handlerSubmit() { },
    openCodeReader() {
      this.$refs.codeReader.openReader()
    },
    handerReaderCode(code) {
      this.listData.push(code)
    }
  }
}
</script>

<style>
</style>