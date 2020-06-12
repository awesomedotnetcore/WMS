<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="编号" prop="Code">
              <a-input v-model="entity.Code" :disabled="$para('GenerateTrayTypeCode')=='1'" :placeholder="$para('GenerateTrayTypeCode')=='1'?'系统自动生成':'编号'" autocomplete="off" />
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
            <a-form-model-item label="长" prop="Length">
              <a-input-number v-model="entity.Length" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="宽" prop="Width">
              <a-input-number v-model="entity.Width" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="高" prop="High">
              <a-input-number v-model="entity.High" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="分区" prop="IsZone">
              <a-radio-group v-model="entity.IsZone" :defaultValue="false" button-style="solid">
                <a-radio-button :value="true">是</a-radio-button>
                <a-radio-button :value="false">否</a-radio-button>
              </a-radio-group>
            </a-form-model-item>
          </a-col>
        </a-row>
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
      entity: { IsZone: false },
      rules: {
        Name: [{ required: true, message: '请输入名称', trigger: 'blur' }]
      },
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = { IsZone: false }
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.title = title
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_TrayType/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_TrayType/SaveData', this.entity).then(resJson => {
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
