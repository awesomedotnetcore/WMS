<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="类型" prop="Type">
          <enum-select code="BarCodeRuleType" v-model="entity.Type"></enum-select>
        </a-form-model-item>
        <a-form-model-item label="规则" prop="Rule">
          <a-input v-model="entity.Rule" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="排序" prop="Sort">
          <a-input-number v-model="entity.Sort" :style="{width:'100%'}" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="长度" prop="length">
          <a-input-number v-model="entity.length" :style="{width:'100%'}" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
export default {
  components: {
    EnumSelect
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
      entity: {},
      rules: {
        Type: [{ required: true, message: '请选择类型', trigger: 'change' }],
        Sort: [{ required: true, message: '请输入排序', trigger: 'blur' }]
      },
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
    openForm(typeId, id, title) {
      this.init()
      this.title = title
      this.entity.TypeId = typeId
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_BarCodeRule/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_BarCodeRule/SaveData', this.entity).then(resJson => {
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
