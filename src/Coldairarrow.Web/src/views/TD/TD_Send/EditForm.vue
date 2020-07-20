<template>
  <a-drawer title="发货" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
        <a-col :span="8">
          <a-form-model-item label="发货编号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateSendCode')=='1'|| disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" :disabled="disabled"/>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="发货类型" prop="Type">
          <enum-select code="SendType" v-model="entity.Type" :disabled="disabled"></enum-select>
          </a-form-model-item>          
        </a-col>               
      </a-row>        
      <a-row>
        <a-col :span="8">          
          <a-form-model-item label="单据日期" prop="OrderTime">
          <a-date-picker v-model="entity.OrderTime" :style="{width:'100%'}" :disabled="disabled"/>
        </a-form-model-item>
        </a-col>          
        <a-col :span="8">
          <a-form-model-item label="发货日期" prop="SendTime">
            <a-date-picker v-model="entity.SendTime" :style="{width:'100%'}" :disabled="disabled"/>
          </a-form-model-item>          
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="客户" prop="CusId">
          <cus-select v-model="entity.CusId" :disabled="disabled"></cus-select>
          </a-form-model-item>          
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="客户地址" prop="AddrId">
            <a-select placeholder="请选择" v-model="entity.AddrId" :disabled="disabled">
            <a-select-option v-for="item in this.CusAddrList" :key="item.Id" >{{item.Address}}</a-select-option>
            </a-select>
          </a-form-model-item>          
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" :disabled="disabled"/>
          </a-form-model-item>
        </a-col>
      </a-row>      
      </a-form-model>
      <list-detail v-model="listDetail" :disabled="disabled"></list-detail>
      <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
        <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
        <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Send.Confirm')" @click="handleAudit(entity.Id,'Confirm')">确认</a-button>
        <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 1 && disabled && hasPerm('TD_Send.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
        <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 1 && disabled && hasPerm('TD_Send.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
        <a-button type="primary" @click="handleSubmit" v-if="entity.Status === 0 && !disabled">保存</a-button>   
      </div>
  </a-drawer>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import ListDetail from '../TD_SendDetail/List'
import CusSelect from '../../../components/PB/CustomerSelect'
import moment from 'moment'

export default {
  components: {
    EnumSelect,
    // EnumName,
    CusSelect,
    ListDetail,
  },
  props: {
    parentObj: { type: Object, required: true },
    disabled: { type: Boolean, required: false, default: false }
  },
  data() {    
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: {Status: 0},
      rules: {},
      title: '',
      CusAddrList:[],
      listDetail:[]
    }
  },
  watch:{
    entity:{
      handler: function (val, oldVal) { /* ... */ },
      deep:true//对象内部的属性监听，也叫深度监听
    },
    'entity.CusId':function(val,oldval){
        this.GetCusAddrList()
    },//键路径必须加上引号
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = {Status: 0,OrderTime:moment(),SendTime:moment()}
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Send/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.OrderTime = moment(this.entity.OrderTime)
          this.entity.SendTime = moment(this.entity.SendTime)
          this.listDetail = [...resJson.Data.SendDetails]
        })
      }
    },
    handleSubmit() {
      
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        // 数据处理
        
        var SendDetails = [...this.listDetail]        
        SendDetails.forEach(element => {
          element.Material = null
          element.Measure = null
          element.Location = null
        })
        var entityData = { ...this.entity }
        entityData.SendDetails = SendDetails
        
        this.$http.post('/TD/TD_Send/SaveData', entityData).then(resJson => {
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
    GetCusAddrList() {      
      this.loading = true
      this.$http.post('/PB/PB_Address/QueryDataList',{
          PageIndex: 1,
          PageRows: 50,
          SortField: 'Id',
          SortType: "asc",
          Search: {
            SupId:"",
            CusId:this.entity.CusId
          },
          filters:{}
        }).then(resJson => {
        this.loading = false
        this.CusAddrList = resJson.Data
      })
    },
    handleAudit(id, type) {
      this.loading = true
      this.$http.post('/TD/TD_Send/Approval', { Id: id, AuditType: type }).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success('操作成功!')
          this.visible = false
          this.parentObj.getDataList()
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>
