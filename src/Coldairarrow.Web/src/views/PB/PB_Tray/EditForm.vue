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
        <a-form-model-item label="货位" prop="LocalId">
          <a-select v-model="entity.LocalId">
            <a-select-option v-for="item in this.LocationList" :key="item.Id">{{item.Name}}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="托盘号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="托盘名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="托盘类型" prop="TrayTypeId">
          <a-select v-model="entity.TrayTypeId">
            <a-select-option v-for="item in this.TrayTypeList" :key="item.Id">{{item.Name}}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="启用日期" prop="StartTime">
          <a-input v-model="entity.StartTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-input v-model="entity.Remarks" autocomplete="off" />
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
      LocationList: [],
      TrayTypeList: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.LocationList = []
      this.TrayTypeList = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      this.getLocationList()
      this.getTrayTypeList()

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Tray/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      } else {
        this.entity.Status = 1
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/PB/PB_Tray/SaveData', this.entity).then(resJson => {
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
    getLocationList() {
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_Location/GetAllData').then(resJson => {
        this.loading = false
        thisObj.LocationList = resJson.Data
      })
    },
    getTrayTypeList() {
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_TrayType/GetAllData').then(resJson => {
        this.loading = false
        thisObj.TrayTypeList = resJson.Data
      })
    }
  }
}
</script>
