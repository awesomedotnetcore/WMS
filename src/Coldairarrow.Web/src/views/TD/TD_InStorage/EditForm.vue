<template>
  <a-drawer title="入库" :width="1024" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="8">
            <a-form-model-item label="入库单号" prop="Code">
              <a-input v-model="entity.Code" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="8">
            <a-form-model-item label="入库时间" prop="InStorTime">
              <a-input v-model="entity.InStorTime" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="8">
            <a-form-model-item label="入库类型" prop="InType">
              <a-input v-model="entity.InType" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="8">
            <a-form-model-item label="关联单号" prop="RefCode">
              <a-input v-model="entity.RefCode" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="8">
            <a-form-model-item label="供应商" prop="SupId">
              <a-input v-model="entity.SupId" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="8">
            <a-form-model-item label="目标地址" prop="AddrId">
              <a-input v-model="entity.AddrId" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="24">
            <a-form-model-item label="备注" prop="Remarks" :labelCol="{span: 2}" :wrapperCol="{ span: 21 }">
              <a-textarea v-model="entity.Remarks" :rows="2" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
  </a-drawer>
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
      rules: {}
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
