 <template>
  <a-drawer title="出库" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="出库单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateOutStorageCode')=='1'|| disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" :disabled="disabled"/>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="出库时间" prop="OutTime">
            <a-date-picker v-model="entity.OutTime"  :style="{width:'100%'}" :disabled="disabled"/>
          </a-form-model-item>
        </a-col>               
      </a-row>
      <a-row>        
        <a-col :span="8">
          <a-form-model-item label="出库类型" prop="OutType">
            <enum-select code="OutStorageType" v-model="entity.OutType" :disabled="disabled"></enum-select>
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
            <a-textarea v-model="entity.Remarks" />
          </a-form-model-item>
        </a-col> 
      </a-row>
    </a-form-model>
    <list-detail v-model="listDetail" :disabled="disabled" :receive="receive"></list-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled  && hasPerm('TD_OutStorage.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>     
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_OutStorage.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button :disabled="disabled" type="primary" @click="handleSubmit" v-if="entity.Status === 0">保存</a-button>      
      
    </div>

    

  </a-drawer>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import CusSelect from '../../../components/PB/CustomerSelect'
// import StorSelect from '../../../components/Storage/StorageSelect'
import ListDetail from '../TD_OutStorDetail/List'
import moment from 'moment'

export default {
  components: {
    EnumSelect,
    EnumName,
    CusSelect,
    ListDetail,
    // StorSelect
  },
  props: {
    // parentObj: Object
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
      receive: false, // 是否发货出库
      entity: {Id: '', Status: 0 },
      rules: {
        OutTime: [{ required: true, message: '请选择出库时间', trigger: 'change' }],
        OutType: [{ required: true, message: '请选择出库类型', trigger: 'change' }],
      },
      // title: '',
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
      this.CusAddrList = []
      this.entity = { Id: '', Status: 0 ,OutTime:moment()}
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    getCurrentData() {
      return new Date().toLocaleDateString()
    },
    openForm(id, title) {
      this.receive = false
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_OutStorage/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          
          this.entity = resJson.Data
          this.entity.OutTime = moment(this.entity.OutTime)
          this.listDetail = [...resJson.Data.OutStorDetails]          
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
        var OutStorDetails = [...this.listDetail]
        OutStorDetails.forEach(element => {
          element.OutStorage = null
          element.Location = null
          element.Tray = null
          element.TrayZone = null
          element.Material = null
        })
        var entityData = { ...this.entity }
        entityData.OutStorDetails = OutStorDetails
        this.$http.post('/TD/TD_OutStorage/SaveData', entityData).then(resJson => {
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
      this.$http.post('/TD/TD_OutStorage/Audit', { Id: id, AuditType: type }).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success('操作成功!')
          this.visible = false
          this.parentObj.getDataList()
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    openSendForm(id) {
      this.receive = true
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Send/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          var receive = resJson.Data
          this.entity = { Id: '', SendId: receive.Id, StorId: receive.StorId, OutTime: moment(), OutType: receive.Type, RefCode: receive.Code, Status: 0, CusId: receive.CusId, AddrId:receive.AddrId}
          var listItem = []
          var tempId = 0
          receive.SendDetails.forEach(detail => {
            tempId += 1
            
           // var item = { Id: 'newid_' + tempId.toString(), StorId: receive.StorId, MaterialId: detail.MaterialId, Material: detail.Material, Price: detail.Price, PlanNum: detail.PlanNum, RecNum: detail.RecNum, Num: detail.PlanNum - detail.RecNum, LocalId: null, TrayId: null, ZoneId: null }
            var item = { Id: 'newid_' + tempId.toString(), StorId: receive.StorId, LocalId:detail.LocalId,Location:detail.Location , TrayId: null, ZoneId: null ,MaterialId: detail.MaterialId, Material: detail.Material, BatchNo: detail.BatchNo , Price: detail.Price,LocalNum:detail.LocalNum,  PlanNum: detail.PlanNum, SendNum: detail.SendNum , OutNum: detail.PlanNum - detail.SendNum,Num: detail.PlanNum - detail.SendNum}//
            if (item.Num > 0) {
              listItem.push(item)
            }
          })
          this.listDetail = listItem
        })
      }
    },
  }
}
</script>
