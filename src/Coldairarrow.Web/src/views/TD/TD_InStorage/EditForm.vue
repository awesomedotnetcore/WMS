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
        <a-form-model-item label="收货ID" prop="RecId">
          <a-input v-model="entity.RecId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="入库单号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="入库时间" prop="InStorTime">
          <a-input v-model="entity.InStorTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="入库类型(枚举)" prop="InType">
          <a-input v-model="entity.InType" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="状态(0待审核;1审核通过;2审核失败)" prop="Status">
          <a-input v-model="entity.Status" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="仓库ID" prop="StorId">
          <a-input v-model="entity.StorId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标地址ID" prop="AddrId">
          <a-input v-model="entity.AddrId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="供应商ID" prop="SupId">
          <a-input v-model="entity.SupId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="入库数量" prop="TotalNum">
          <a-input v-model="entity.TotalNum" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总金额" prop="TotalAmt">
          <a-input v-model="entity.TotalAmt" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="设备ID" prop="EqId">
          <a-input v-model="entity.EqId" autocomplete="off" />
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
        this.$http.post('/TD/TD_InStorage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_InStorage/SaveData', this.entity).then(resJson => {
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
