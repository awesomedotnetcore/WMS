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
        <a-form-model-item label="仓库" prop="StorId">
          <storage-select v-model="entity.StorId"></storage-select>
        </a-form-model-item>
        <a-form-model-item label="货区编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
           <!-- <a-input v-model="entity.Code" :disabled="$para('GenerateStorAreaCode')=='1'" placeholder="系统自动生成" autocomplete="off" /> -->
        </a-form-model-item>
        <a-form-model-item label="货区名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否缓存区" prop="IsCache">    
          <a-radio-group v-model="entity.IsCache">
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
import StorageSelect from '../../../components/Storage/StorageSelect'

export default {
  components: {
    StorageSelect,
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

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_StorArea/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_StorArea/SaveData', this.entity).then(resJson => {
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
