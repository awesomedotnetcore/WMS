<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="调拨ID" prop="AllocateId">
          <a-input v-model="entity.AllocateId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原仓库ID" prop="FromStorId">
          <a-input v-model="entity.FromStorId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原货位ID" prop="FromlocalId">
          <a-input v-model="entity.FromlocalId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原托盘ID" prop="FromTrayId">
          <a-input v-model="entity.FromTrayId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原托盘分区ID" prop="FromZoneId">
          <a-input v-model="entity.FromZoneId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标仓库ID" prop="ToStorId">
          <a-input v-model="entity.ToStorId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标货位ID" prop="TolocalId">
          <a-input v-model="entity.TolocalId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料ID" prop="MaterialId">
          <a-input v-model="entity.MaterialId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="批次号" prop="BatchNo">
          <a-input v-model="entity.BatchNo" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总额" prop="Amount">
          <a-input v-model="entity.Amount" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="调拨数量" prop="LocalNum">
          <a-input v-model="entity.LocalNum" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
  props: {
    parentObj: Object
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: {},
      rules: {},
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_AllocateDetail/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_AllocateDetail/SaveData', this.entity).then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false

            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>
