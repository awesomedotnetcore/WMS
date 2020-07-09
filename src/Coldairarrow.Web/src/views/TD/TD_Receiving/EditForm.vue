<template>
  <a-drawer title="收货" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="收货单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateInStorageCode')=='1' || disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="订单时间" prop="OrderTime">
            <a-date-picker v-model="entity.OrderTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="收货时间" prop="RecTime">
            <a-date-picker v-model="entity.RecTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="收货类型" prop="Type">
            <enum-select code="ReceiveType" v-model="entity.Type" :disabled="disabled"></enum-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="供应商" prop="SupId">
            <sup-select v-model="entity.SupId" :disabled="disabled"></sup-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="24">
          <a-form-model-item label="备注" prop="Remarks">
            <a-input v-model="entity.Remarks" :disabled="disabled" :style="{width:'100%'}" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
    <list-detail v-model="listDetail" :disabled="disabled"></list-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Receiving.Confirm')" @click="handleAudit(entity.Id,'Confirm')">确认</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 1 && disabled && hasPerm('TD_Receiving.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 1 && disabled && hasPerm('TD_Receiving.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
      <a-button type="primary" @click="handleSubmit" v-if="entity.Status === 0 && !disabled">保存</a-button>
    </div>
  </a-drawer>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import SupSelect from '../../../components/PB/SupplierSelect'
import ListDetail from '../TD_RecDetail/List'
import moment from 'moment'
export default {
  components: {
    EnumSelect,
    EnumName,
    SupSelect,
    ListDetail
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
      entity: { Id: '', Status: 0 },
      listDetail: [],
      rules: {
        OrderTime: [{ required: true, message: '请输入订单时间', trigger: 'change' }],
        RecTime: [{ required: true, message: '请输入收货时间', trigger: 'change' }],
        Type: [{ required: true, message: '请选择收货入库类型', trigger: 'change' }],
        SupId: [{ required: true, message: '请选择供应商', trigger: 'change' }]
      }
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = { Id: '', Status: 0, OrderTime: moment() }
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Receiving/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          this.entity = resJson.Data
          this.entity.OrderTime = moment(this.entity.OrderTime)
          this.entity.RecTime = moment(this.entity.RecTime)
          this.listDetail = [...resJson.Data.RecDetails]
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
        var RecDetails = [...this.listDetail]
        RecDetails.forEach(element => {
          element.Material = null
        })
        var entityData = { ...this.entity }
        entityData.RecDetails = RecDetails
        this.$http.post('/TD/TD_Receiving/SaveData', entityData).then(resJson => {
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
    handleAudit(id, type) {
      this.loading = true
      this.$http.post('/TD/TD_Receiving/Approval', { Id: id, AuditType: type }).then(resJson => {
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
