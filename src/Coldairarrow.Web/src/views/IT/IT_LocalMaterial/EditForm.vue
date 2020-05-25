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
        <a-form-model-item label="仓库ID" prop="StorId">
          <a-input v-model="entity.StorId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="货位ID" prop="LocalId">
          <a-input v-model="entity.LocalId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="托盘号ID" prop="TrayId">
          <a-input v-model="entity.TrayId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="托盘分区ID" prop="ZonedId">
          <a-input v-model="entity.ZonedId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料ID" prop="MaterialId">
          <a-input v-model="entity.MaterialId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单位ID" prop="MeasureId">
          <a-input v-model="entity.MeasureId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="批次号" prop="BatchNo">
          <a-input v-model="entity.BatchNo" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="数量" prop="Num">
          <a-input v-model="entity.Num" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总额" prop="TotalAmt">
          <a-input v-model="entity.TotalAmt" autocomplete="off" />
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
        this.$http.post('/IT/IT_LocalMaterial/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/IT/IT_LocalMaterial/SaveData', this.entity).then(resJson => {
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
