<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="用户" prop="UserId">
          <user-select v-model="entity.UserId"></user-select>
        </a-form-model-item>
        <a-form-model-item label="仓库" prop="StorId">
          <storage-select v-model="entity.StorId"></storage-select>
        </a-form-model-item>
        <a-form-model-item label="默认仓库" prop="IsDefault">
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
import StorageSelect from '../../../components/Storage/StorageSelect'
import UserSelect from '../../../components/User/UserSelect'
export default {
  components: {
    StorageSelect,
    UserSelect
  },
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
        UserId: [{ required: true, message: '请选择用户', trigger: 'change' }],
        StorId: [{ required: true, message: '请选择仓库', trigger: 'change' }],
        IsDefault: [{ required: true, message: '是否默认', trigger: 'change' }]
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
    openForm(id, title) {
      this.init()
      this.title = title
      if (id) {
        this.loading = true
        this.$http.post('/Base/Base_UserStor/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/Base/Base_UserStor/SaveData', this.entity).then(resJson => {
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
