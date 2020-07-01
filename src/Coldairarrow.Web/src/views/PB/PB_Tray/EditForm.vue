<template>
  <a-modal :title="title" width="60%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="托盘号" prop="Code">
              <a-input v-model="entity.Code" :disabled="$para('GenerateTrayCode')=='1'" :placeholder="$para('GenerateTrayCode')=='1'?'系统自动生成':'托盘号'" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="托盘名称" prop="Name">
              <a-input v-model="entity.Name" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="托盘类型" prop="TrayTypeId">
              <a-select v-model="entity.TrayTypeId">
                <a-select-option v-for="item in this.TrayTypeList" :key="item.Id">{{ item.Name }}</a-select-option>
              </a-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="启用日期" prop="StartTime">
              <a-date-picker v-model="entity.StartTime" :style="{width:'100%'}" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
          <a-form-model-item label="备注" prop="Remarks">
            <a-textarea v-model="entity.Remarks" autocomplete="off" />
            <!-- :auto-size="{ minRows: 3, maxRows: 5 }" -->
          </a-form-model-item>
          </a-col>
        </a-row>
        
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'

export default {
  components: {
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
        Name: [{ required: true, message: '请输入名称', trigger: 'blur' }],
        TrayTypeId: [{ required: true, message: '请选择类型', trigger: 'change' }]
      },
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
      this.title = title
      this.init()
      // this.getLocationList()
      this.getTrayTypeList()

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Tray/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.StartTime = moment(this.entity.StartTime)
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
