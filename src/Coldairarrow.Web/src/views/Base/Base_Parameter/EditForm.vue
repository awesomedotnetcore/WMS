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
        <a-form-model-item label="参数类型" prop="Type">
          <enum-select code="SystemType" v-model="entity.Type" >             
          </enum-select>
        </a-form-model-item>
        <a-form-model-item label="参数编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="参数名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="参数值" prop="Val">
          <a-input v-model="entity.Val" autocomplete="off" />
        </a-form-model-item>   
        <a-form-model-item label="描述" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>     
        <a-form-model-item label="是否系统必须" prop="IsSystem">
          <a-select v-model="entity.IsSystem" autocomplete="off" @select="DataTypeChange" disabled>
            <a-select-option :value="false" >否</a-select-option>
            <a-select-option :value="true" >是</a-select-option>
          </a-select>
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
      rules: {},
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      // this.entity = {}
      this.entity.IsSystem= false
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/Base/Base_Parameter/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/Base/Base_Parameter/SaveData', this.entity).then(resJson => {
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
    },
    DataTypeChange(val, option) {
      this.DataType = val
    }
  }
}
</script>
