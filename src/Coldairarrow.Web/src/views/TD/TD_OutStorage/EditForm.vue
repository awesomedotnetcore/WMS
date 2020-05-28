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
        <a-form-model-item label="仓库ID" prop="StorageId">
          <a-input v-model="entity.StorageId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="出库单号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="出库时间" prop="OutTime">
          <a-input v-model="entity.OutTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="出库类型(枚举)" prop="OutType">
          <a-input v-model="entity.OutType" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="出库数量" prop="OutNum">
          <a-input v-model="entity.OutNum" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总金额" prop="TotalAmt">
          <a-input v-model="entity.TotalAmt" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="设备ID" prop="EquId">
          <a-input v-model="entity.EquId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="状态" prop="Status">
          <a-input v-model="entity.Status" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="客户ID" prop="CusId">
          <a-input v-model="entity.CusId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标地址ID" prop="AddrId">
          <a-input v-model="entity.AddrId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-input v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="审核人ID" prop="AuditUserId">
          <a-input v-model="entity.AuditUserId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="审核时间" prop="AuditeTime">
          <a-input v-model="entity.AuditeTime" autocomplete="off" />
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
        this.$http.post('/TD/TD_OutStorage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_OutStorage/SaveData', this.entity).then(resJson => {
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
