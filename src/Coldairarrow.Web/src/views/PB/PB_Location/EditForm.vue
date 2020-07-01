<template>
  <a-modal
    :title="title"
    width="60%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="货位编号" prop="Code">
              <a-input v-model="entity.Code" :disabled="$para('GenerateLocationCode')=='1'" placeholder="系统自动生成" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="货位名称" prop="Name">
              <a-input v-model="entity.Name" autocomplete="off" />
            </a-form-model-item>  
          </a-col>
        </a-row>      
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="仓库" prop="StorId">
              <storage-select v-model="entity.StorId"></storage-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item  label="货区" prop="AreaId">
              <area-select :storId="entity.StorId" v-model="entity.AreaId" @select="handleAreaSelect"></area-select>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="巷道" prop="LanewayId">
              <laneway-select :storId="entity.StorId" v-model="entity.LanewayId" @select="handleLanewaySelect"></laneway-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="货架" prop="RackId">
              <rack-select :storId="entity.StorId" v-model="entity.RackId" @select="handleRackSelect"></rack-select>
            </a-form-model-item>
          </a-col>
        </a-row>    
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="是否禁用" prop="IsForbid">
            <a-radio-group v-model="entity.IsForbid" :default-value="true" button-style="solid">
              <a-radio-button :value="true">
                启用
              </a-radio-button>
              <a-radio-button :value="false">
                停用
              </a-radio-button>
            </a-radio-group>
          </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="剩余容量" prop="OverVol">
            <a-input v-model="entity.OverVol" autocomplete="off" />
            </a-form-model-item>            
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="是否默认" prop="IsDefault">
              <a-radio-group v-model="entity.IsDefault" :default-value="false" button-style="solid" >
                <a-radio-button :value="true">
                  是
                </a-radio-button>
                <a-radio-button :value="false">
                  否
                </a-radio-button>
                </a-radio-group>
              </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="备注" prop="Remarks">
              <a-textarea v-model="entity.Remarks" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>    
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import StorageSelect from '../../../components/Storage/StorageSelect'
import AreaSelect from '../../../components/Storage/AreaSelect'
import LanewaySelect from '../../../components/Storage/LanewaySelect'
import RackSelect from '../../../components/Storage/RackSelect'

export default {
  components: {
    EnumSelect,
    StorageSelect,
    AreaSelect,
    LanewaySelect,
    RackSelect
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
        Name: [{ required: true, message: '请输入货位名称', trigger: 'blur' }],
        StorId: [{ required: true, message: '请选择仓库', trigger: 'change' }],
        AreaId: [{ required: true, message: '请选择货区', trigger: 'change' }],    
        IsForbid: [{ required: true, message: '请选择禁用状态', trigger: 'blur' }], 
        IsDefault: [{ required: true, message: '请选择默认状态', trigger: 'blur' }], 
      },
      title: '',
      StorAreaList: [],
      LanewayList:[],
      RackList:[],
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.StorAreaList = []
      this.LanewayList = []
      this.RackList = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
      this.GetStorAreaList()
      this.GetLanewayList()
      this.GetRackList()

      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Location/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Location/SaveData', this.entity).then(resJson => {
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
    GetStorAreaList() {
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_StorArea/QueryStorAreaData').then(resJson => {
       // this.$http.post('/PB/PB_StorArea/GetDataListByStor?StorId=' + this.StorId).then(resJson => {
        this.loading = false
        thisObj.StorAreaList = resJson.Data
      })
    },
    GetLanewayList() {
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_Laneway/QueryLanewayData').then(resJson => {
        this.loading = false
        thisObj.LanewayList = resJson.Data
      })
    },
    GetRackList() {
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_Rack/QueryRackData').then(resJson => {
        this.loading = false
        thisObj.RackList = resJson.Data
      })
    },
    DataTypeChange(val, option) {
      this.DataType = val
    },
    handleAreaSelect(Area) {
      console.log('handleAreaSelect', Area)
    },
    handleLanewaySelect(Laneway) {
      console.log('handleLanewaySelect', Laneway)
    },
    handleRackSelect(Rack) {
      console.log('handleRackSelect', Rack)
    }
  }
}
</script>
