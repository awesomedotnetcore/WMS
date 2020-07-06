<template>
  <a-drawer title="调拨" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="调拨单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateAllocateCode')=='1' || disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="调拨时间" prop="AllocateTime">
            <a-date-picker v-model="entity.AllocateTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="调拨类型" prop="Type">
            <enum-select code="AllocateType" v-model="entity.Type" :disabled="disabled"></enum-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="调拨仓库" prop="ToStorId">
            <storage-select v-model="entity.ToStorId" placeholder="调拨仓库" :disabled="disabled"></storage-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="备注" prop="Remarks">
            <a-textarea v-model="entity.Remarks" autocomplete="off" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
    <allocate-detail :disabled="disabled" v-model="listDetail"></allocate-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Allocate.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Allocate.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
      <a-button :disabled="disabled" type="primary" @click="handleSubmit" v-if="entity.Status === 0">保存</a-button>
    </div>
  </a-drawer>
</template>

<script>
import moment from 'moment'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import AllocateDetail from '../TD_AllocateDetail/List'
import StorageSelect from '../../../components/Storage/AllStorageSelect'
export default {
  components: {
    EnumSelect,
    AllocateDetail,
    StorageSelect
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
        AllocateTime: [{ required: true, message: '请选择调拨时间', trigger: 'change' }],
        ToStorId: [{ required: true, message: '请选择调拨仓库', trigger: 'change' }],
        Type: [{ required: true, message: '请选择调拨类型', trigger: 'change' }]
      }
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = { Id: '', Status: 0, AllocateTime: moment() }
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Allocate/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          this.entity = resJson.Data
          this.entity.AllocateTime = moment(this.entity.AllocateTime)
          this.listDetail = [...resJson.Data.AllocateDetails]
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
        var allocateDetails = this.listDetail.map((v, i, arr) => {
          var result = { ...v }
          result.FromLocal = null
          result.FromTray = null
          result.FromZone = null
          result.Material = null
          result.Measure = null
          return result
        })
        var entityData = { ...this.entity }
        entityData.AllocateDetails = allocateDetails

        this.$http.post('/TD/TD_Allocate/SaveData', entityData).then(resJson => {
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
      this.$http.post('/TD/TD_Allocate/Audit', { Id: id, AuditType: type }).then(resJson => {
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
