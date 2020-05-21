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
        <a-form-model-item label="物料名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料编码" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料简称" prop="SimpleName">
          <a-input v-model="entity.SimpleName" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料类型ID" prop="MaterialTypeId">
          <a-input v-model="entity.MaterialTypeId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单位ID" prop="MeasureId">
          <a-input v-model="entity.MeasureId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="物料规格" prop="Spec">
          <a-input v-model="entity.Spec" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="上限数量" prop="Max">
          <a-input v-model="entity.Max" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="下限数量" prop="Min">
          <a-input v-model="entity.Min" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="客户ID" prop="CusId">
          <a-input v-model="entity.CusId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="供应商ID" prop="SupId">
          <a-input v-model="entity.SupId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-input v-model="entity.Remarks" autocomplete="off" />
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
        this.$http.post('/PB/PB_Material/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Material/SaveData', this.entity).then(resJson => {
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
