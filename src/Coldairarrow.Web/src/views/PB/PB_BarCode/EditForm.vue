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
        <a-form-model-item label="条码编号" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="条码类型" prop="BarCodeTypeId">
          <a-select v-model="entity.BarCodeTypeId">
            <a-select-option v-for="item in this.BarCodeTypeList" :value="item.Id" :key="item.Id">{{ item.Name }}</a-select-option>
          </a-select>
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
      title: '',
      BarCodeTypeList: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.BarCodeTypeList = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
      this.getBarCodeTypeList()

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_BarCode/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_BarCode/SaveData', this.entity).then(resJson => {
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
    getBarCodeTypeList() {
      var thisObj = this
      this.loading = true
      this.$http.get('/PB/PB_BarCodeType/GetAllData').then(resJson => {
        this.loading = false
        thisObj.BarCodeTypeList = resJson.Data
      })
    }
  }
}
</script>
