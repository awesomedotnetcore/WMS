<template>
  <a-drawer title="入库" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="入库单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateInStorageCode')=='1' || disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="入库时间" prop="InStorTime">
            <a-date-picker v-model="entity.InStorTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="入库类型" prop="InType">
            <enum-select code="InStorageType" v-model="entity.InType" :disabled="disabled"></enum-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
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
        <a-col :span="8">
          <a-form-model-item label="备注" prop="Remarks">
            <a-input v-model="entity.Remarks" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
    <list-detail v-model="listDetail" :disabled="disabled" :receive="receive"></list-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_InStorage.Auditing') " @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_InStorage.Auditing') " @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
      <a-button :disabled="disabled" type="primary" @click="handleSubmit" v-if="entity.Status === 0">保存</a-button>
    </div>
  </a-drawer>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import SupSelect from '../../../components/PB/SupplierSelect'
import ListDetail from '../TD_InStorDetail/List'
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
      receive: false, // 是否收货入库
      entity: { Id: '', Status: 0 },
      listDetail: [],
      rules: {
        InStorTime: [{ required: true, message: '请输入入库时间', trigger: 'change' }],
        InType: [{ required: true, message: '请选择入库类型', trigger: 'change' }],
        SupId: [{ required: true, message: '请选择供应商', trigger: 'change' }]
      }
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = { Id: '', Status: 0, InStorTime: moment() }
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.receive = false
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_InStorage/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          this.entity = resJson.Data
          this.entity.InStorTime = moment(this.entity.InStorTime)
          this.listDetail = [...resJson.Data.InStorDetails]
        })
      }
    },
    openReceivingForm(id) {
      this.receive = true
      this.init()
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Receiving/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          var receive = resJson.Data
          this.entity = { Id: '', RecId: receive.Id, StorId: receive.StorId, InStorTime: moment(), InType: receive.Type, RefCode: receive.Code, Status: 0, SupId: receive.SupId }
          var listItem = []
          var tempId = 0
          receive.RecDetails.forEach(detail => {
            tempId += 1
            var item = { Id: 'newid_' + tempId.toString(), StorId: receive.StorId, MaterialId: detail.MaterialId, Material: detail.Material, Price: detail.Price, PlanNum: detail.PlanNum, RecNum: detail.RecNum, Num: detail.PlanNum - detail.RecNum, LocalId: null, TrayId: null, ZoneId: null }
            if (item.Num > 0) {
              listItem.push(item)
            }
          })
          this.listDetail = listItem
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
        var InStorDetails = []
        for (let index = 0; index < this.listDetail.length; index++) {
          const element = this.listDetail[index]
          if (!element.LocalId || element.Num === 0) continue
          element.InStorage = null
          element.Location = null
          element.Tray = null
          element.TrayZone = null
          element.Material = null
          InStorDetails.push({ ...element })
        }
        if (InStorDetails.length === 0) {
          this.$message.error('入库明细数据不正确')
          return
        }
        var entityData = { ...this.entity }
        entityData.InStorDetails = InStorDetails
        this.$http.post('/TD/TD_InStorage/SaveData', entityData).then(resJson => {
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
      this.$http.post('/TD/TD_InStorage/Audit', { Id: id, AuditType: type }).then(resJson => {
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
