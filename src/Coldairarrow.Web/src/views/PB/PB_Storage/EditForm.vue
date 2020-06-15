<template>
  <a-modal
    :title="title"
    width="30%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="仓库编号" prop="Code">
          <a-input v-model="entity.Code" :disabled="$para('GenerateStorageCode')=='1'" placeholder="系统自动生成" autocomplete="off" />       
        </a-form-model-item>
        <a-form-model-item label="仓库名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="仓库类型" prop="Type">
          <enum-select code="StorageType" v-model="entity.Type" >             
          </enum-select>
        </a-form-model-item>
        <!-- <a-form-model-item label="默认仓库:" prop="IsDefault">
          <a-select v-model="entity.IsDefault" autocomplete="off" @select="DataTypeChange" disabled>
            <a-select-option :value="false" >否</a-select-option>
            <a-select-option :value="true" >是</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="托盘管理:" prop="IsTray">
          <a-select  v-model="entity.IsTray" autocomplete="off" @select="DataTypeChange" disabled>
            <a-select-option :value="false" >停用</a-select-option>
            <a-select-option :value="true" >启用</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="分区管理:" prop="IsZone">
          <a-select  v-model="entity.IsZone" autocomplete="off" @select="DataTypeChange" disabled> 
            <a-select-option :value="false" >停用</a-select-option>
            <a-select-option :value="true" >启用</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="仓库状态:" prop="Disable">
          <a-select  v-model="entity.Disable" autocomplete="off" @select="DataTypeChange" disabled>
            <a-select-option :value="false" >停用</a-select-option>
            <a-select-option :value="true" >启用</a-select-option>
          </a-select>
        </a-form-model-item>         -->

        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
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
        Name: [{ required: true, message: '请输入仓库名称', trigger: 'blur' }],
        Type: [{ required: true, message: '请选择仓库类型', trigger: 'change' }]
      },
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      //this.entity.Type=''
      
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
      this.entity.IsTray= false
      this.entity.IsZone= false
      this.entity.disable= false
      this.entity.IsDefault= false
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Storage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Storage/SaveData', this.entity).then(resJson => {
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
