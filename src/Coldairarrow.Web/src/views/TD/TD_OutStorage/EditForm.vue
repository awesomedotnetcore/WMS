 <template>
  <a-drawer title="出库" :width="1024" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="出库单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateOutStorageCode')=='1'" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="出库时间" prop="OutTime">
            <a-date-picker v-model="entity.OutTime" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="出库类型" prop="OutType">
            <enum-select code="OutStorageType" v-model="entity.OutType"></enum-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="客户" prop="CusId">
            <cus-select v-model="entity.CusId"></cus-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="客户地址" prop="AddrId">
            <a-input v-model="entity.AddrId" />
            <!-- <cus-select v-model="entity.CusId"></cus-select> -->
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="备注" prop="Remarks">
            <a-input v-model="entity.Remarks" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
    <list-detail v-model="listDetail"></list-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }">取消</a-button>
      <a-button type="primary">确定</a-button>
    </div>
  </a-drawer>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import CusSelect from '../../../components/PB/CustomerSelect'
import ListDetail from '../TD_OutStorDetail/List'

export default {
  components: {
    EnumSelect,
    EnumName,
    CusSelect,
    ListDetail
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
        this.$http.post('/TD/TD_OutStorage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_OutStorage/SaveData', this.entity).then(resJson => {
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
      var thisObj = this
      this.loading = true
      this.$http.post('/PB/PB_Address/QueryDataListAsync').then(resJson => {
        this.loading = false
        thisObj.SCusAddrList = resJson.Data
      })
    },
  }
}
</script>
