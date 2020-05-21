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
        <a-form-model-item label="货位编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="货位名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="库位类型(枚举)" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="仓库ID" prop="StorId">
          <a-input v-model="entity.StorId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="库区ID" prop="AreaId">
          <a-input v-model="entity.AreaId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="巷道ID" prop="LanewayId">
          <a-input v-model="entity.LanewayId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="货架ID" prop="IdRack">
          <a-input v-model="entity.IdRack" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="剩余容量" prop="OverVol">
          <a-input v-model="entity.OverVol" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否禁用" prop="IsForbid">
          <a-input v-model="entity.IsForbid" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否默认" prop="IsDefault">
          <a-input v-model="entity.IsDefault" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="故障代码" prop="ErrorCode">
          <a-input v-model="entity.ErrorCode" autocomplete="off" />
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
        this.$http.post('/PB/PB_Location/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Location/SaveData', this.entity).then(resJson => {
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
