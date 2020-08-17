<template>
  <a-card title="自动出库">
    <a-button slot="extra" type="primary" ghost @click="handlerSubmit" :loading="loading">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="MaterialCode">
        <input-code v-model="entity.MaterialCode" placeholder="物料条码"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="BatchNo">
        <a-input v-model="entity.BatchNo" placeholder="批次号" />
      </a-form-model-item>
      <a-form-model-item prop="Num">
        <a-input-number v-model="entity.Num" :style="{width:'100%'}" :min="1" placeholder="物料数量" />
      </a-form-model-item>
    </a-form-model>
  </a-card>
</template>

<script>
import InputCode from '../../components/InputBarcode'
import OutStorageSvc from '../../api/TD/OutStorageSvc'
export default {
  components: {
    InputCode
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        MaterialCode: [{ required: true, message: '请输入物料条码', trigger: 'blur' }],
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
        OutStorageSvc.ProductOut(this.entity).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.$message.success(resJson.Msg)
            this.$router.push({ path: `/OutStorage/Detail/${resJson.Data.Id}` })
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