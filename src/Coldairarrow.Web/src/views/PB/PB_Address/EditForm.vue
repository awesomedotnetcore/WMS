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
        <a-form-model-item label="电话/投料点编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="联系人/投料点名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="地址" prop="Address">
          <a-input v-model="entity.Address" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否启用 " prop="IsEnable">
          <a-input v-model="entity.IsEnable" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否默认" prop="IsDefault">
          <a-input v-model="entity.IsDefault" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off"></a-textarea>
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
    openForm(id, CusId, SupId) {
      this.init()

      this.entity.CusId = CusId
      this.entity.SupId = SupId
      if (id!="") {
        this.loading = true
        this.$http.post('/PB/PB_Address/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Address/SaveData', this.entity).then(resJson => {
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
