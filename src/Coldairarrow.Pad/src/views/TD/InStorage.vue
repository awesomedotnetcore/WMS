<template>
  <a-card title="生产入库" :bordered="false" :loading="loading">
    <a-button slot="extra" type="primary" size="small" @click="handlerSubmit">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="MaterialCode">
        <input-code v-model="entity.MaterialCode" placeholder="物料"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="TrayCode">
        <input-code v-model="entity.TrayCode" placeholder="托盘号"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="BatchNo">
        <a-input v-model="entity.BatchNo" placeholder="批次号" />
      </a-form-model-item>
      <a-form-model-item prop="Num">
        <a-input-number v-model="entity.Num" :min="1" />
      </a-form-model-item>
    </a-form-model>
  </a-card>
</template>

<script>
import InputCode from '../../components/InputBarcode'
import InStorageSvc from '../../api/TD/InStorageSvc'
export default {
  components: {
    InputCode
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        MaterialCode: [{ required: true, message: '请输入物料编码', trigger: 'blur' }],
        TrayCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }],
        Num: [{ required: true, message: '请输入物料数量', trigger: 'blur' }]
      }
    }
  },
  methods: {
    handlerSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        InStorageSvc.AutoInByTary(this.entity).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.$refs['from'].resetFields()
            this.$message.success(resJson.Msg)
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>

<style>
</style>