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
         <!-- <a-form-model-item label="仓库编号" prop="Code">
          <a-input v-model="entity.Code" :disabled="$para('GenerateStorageCode')=='1'" placeholder="系统自动生成" autocomplete="off" />       
        </a-form-model-item> -->
        
        <a-form-model-item label="托盘管理:" prop="IsTray">
          <a-radio-group :disabled="entity.Disable" v-model="entity.IsTray" button-style="solid">
            <a-radio-button :value="true">
              启用
            </a-radio-button>
            <a-radio-button :value="false">
              停用
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>
        <a-form-model-item label="分区管理:" prop="IsZone">
          <a-radio-group :disabled="entity.Disable"  v-model="entity.IsZone" button-style="solid">
            <a-radio-button :value="true">
              启用
            </a-radio-button>
            <a-radio-button :value="false">
              停用
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>
        <a-form-model-item label="仓库状态:" prop="Disable" >
          <a-radio-group v-model="entity.Disable" button-style="solid">
            <a-radio-button :value="true">
              启用
            </a-radio-button>
            <a-radio-button :value="false">
              停用
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>        
        <a-form-model-item label="默认仓库:" prop="IsDefault">
          <a-radio-group :disabled="entity.Disable" v-model="entity.IsDefault" button-style="solid">
            <a-radio-button :value="true">
              是
            </a-radio-button>
            <a-radio-button :value="false">
              否
            </a-radio-button>
          </a-radio-group>
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
      this.entity = {}   
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
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
