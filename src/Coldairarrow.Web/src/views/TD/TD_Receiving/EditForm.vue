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
        <a-form-model-item label="收货单号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="订单日期" prop="OrderTime">
          <a-input v-model="entity.OrderTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="收货日期" prop="RecTime">
          <a-input v-model="entity.RecTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="收货类型(枚举)" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="状态(0待审核;1确认；2：取消;3审核通过;4审核失败;5部分入库；6全部入库)" prop="Status">
          <a-input v-model="entity.Status" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="供应商ID" prop="SupId">
          <a-input v-model="entity.SupId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="收货数量" prop="TotalNum">
          <a-input v-model="entity.TotalNum" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="入库数量" prop="InNum">
          <a-input v-model="entity.InNum" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总金额" prop="TotalAmt">
          <a-input v-model="entity.TotalAmt" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-input v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="确认ID" prop="ConfirmUserId">
          <a-input v-model="entity.ConfirmUserId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="确认时间" prop="ConfirmTime">
          <a-input v-model="entity.ConfirmTime" autocomplete="off" />
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
        this.$http.post('/TD/TD_Receiving/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_Receiving/SaveData', this.entity).then(resJson => {
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
