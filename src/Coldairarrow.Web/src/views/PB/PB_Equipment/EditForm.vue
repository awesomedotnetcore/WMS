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
        <a-form-model-item label="编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" ><a-icon slot="prefix" type="scan" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" ><a-icon slot="prefix" type="paper-clip" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="设备码" prop="EquNum">
          <a-input v-model="entity.EquNum" :disabled="$para('EquipmentCode')=='1'" placeholder="系统自动生成" autocomplete="off" ><a-icon slot="prefix" type="barcode" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="状态" prop="Status">
          <a-radio-group v-model="entity.Status">
            <a-radio-button :value="true">
              启用
            </a-radio-button>
            <a-radio-button :value="false">
              停用
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remark">
          <a-textarea v-model="entity.Remark" autocomplete="off"></a-textarea>
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
      this.entity = {Status:true}    
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()        
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Equipment/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Equipment/SaveData', this.entity).then(resJson => {
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
