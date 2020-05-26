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
        <a-form-model-item label="盘点时间" prop="CheckTime">
          <a-input v-model="entity.CheckTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="盘点类型
            整体盘点 区域盘点 特定物料盘点 随机物料盘点(百分比) " prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="设备ID" prop="EquId">
          <a-input v-model="entity.EquId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否盘差状态(0待盘 1已盘)" prop="IsComplete">
          <a-input v-model="entity.IsComplete" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="状态(0待审核;1审核通过;2审核失败)" prop="Status">
          <a-input v-model="entity.Status" autocomplete="off" />
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
        this.$http.post('/TD/TD_Check/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_Check/SaveData', this.entity).then(resJson => {
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
