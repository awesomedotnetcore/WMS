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
        <a-form-model-item label="货位编号" prop="Code">
          <a-input v-model="entity.Code" :disabled="$para('GenerateLocationCode')=='1'" placeholder="系统自动生成" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="货位名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>        
        <a-form-model-item label="仓库" prop="StorId">
          <storage-select v-model="entity.StorId"></storage-select>
        </a-form-model-item>

        <a-form-model-item  label="货区" prop="AreaId">
           <area-select :storId="entity.StorId" v-model="entity.AreaId" @select="handleAreaSelect"></area-select>
          <!-- <a-select placeholder="请选择" v-model="entity.AreaId">
            <a-select-option v-for="item in this.StorAreaList" :key="item.Id">{{item.Name}}</a-select-option>
          </a-select> -->
        </a-form-model-item>

        <a-form-model-item label="巷道" prop="LanewayId">
          <laneway-select :storId="entity.StorId" v-model="entity.LanewayId" @select="handleLanewaySelect"></laneway-select>
          <!-- <a-select placeholder="请选择" :storId="entity.StorId" v-model="entity.LanewayId">
            <a-select-option v-for="item in this.LanewayList" :key="item.Id">{{item.Name}}</a-select-option>
          </a-select> -->
        </a-form-model-item>
        <a-form-model-item label="货架" prop="RackId">
          <rack-select :storId="entity.StorId" v-model="entity.RackId" @select="handleRackSelect"></rack-select>
          <!-- <a-select placeholder="请选择" v-model="entity.RackId">
            <a-select-option v-for="item in this.RackList" :key="item.Id">{{item.Name}}</a-select-option>
          </a-select> -->
        </a-form-model-item>
        <a-form-model-item label="剩余容量" prop="OverVol">
          <a-input v-model="entity.OverVol" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否禁用" prop="IsForbid">
          <a-radio-group v-model="entity.IsForbid">
            <a-radio-button :value="true">
              启用
            </a-radio-button>
            <a-radio-button :value="false">
              停用
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>
        <a-form-model-item label="是否默认" prop="IsDefault">
          <a-radio-group v-model="entity.IsDefault">
            <a-radio-button :value="true">
              是
            </a-radio-button>
            <a-radio-button :value="false">
              否
            </a-radio-button>
          </a-radio-group>
        </a-form-model-item>
        <!-- <a-form-model-item label="故障代码" prop="ErrorCode">
          <a-input v-model="entity.ErrorCode" autocomplete="off" />
        </a-form-model-item> -->
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
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
      rules: {},
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
