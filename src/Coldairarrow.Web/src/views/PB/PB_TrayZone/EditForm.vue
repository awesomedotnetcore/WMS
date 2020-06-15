<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="编号" prop="Code">
              <a-input v-model="entity.Code" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="名称" prop="Name">
              <a-input v-model="entity.Name" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="X" prop="X">
              <a-input-number v-model="entity.X" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="Y" prop="Y">
              <a-input-number v-model="entity.Y" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-form-model-item label="是否默认" prop="IsDefault">
          <a-radio-group v-model="entity.IsDefault" :default-value="false" button-style="solid">
            <a-radio-button :value="false">否</a-radio-button>
            <a-radio-button :value="true">是</a-radio-button>
          </a-radio-group>
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
      entity: { IsDefault: false },
      rules: {
        Code: [{ required: true, message: '请输入编号', trigger: 'blur' }],
        Name: [{ required: true, message: '请输入名称', trigger: 'blur' }],
        IsDefault: [{ required: true, message: '请选择是否默认', trigger: 'change' }]
      },
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = { IsDefault: false }
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(typeId, id, title) {
      this.init()
      this.title = title
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_TrayZone/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      } else {
        this.entity.TrayTypeId = typeId
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/PB/PB_TrayZone/SaveData', this.entity).then(resJson => {
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
