<template>
  <a-drawer title="报损" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="报损单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateBadCode')=='1' || disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="报损时间" prop="BadTime">
            <a-date-picker v-model="entity.BadTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="报损类型" prop="Type">
            <enum-select code="BadType" v-model="entity.Type" :disabled="disabled"></enum-select>
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
          <a-form-model-item label="备注" prop="Remarks">
            <a-textarea v-model="entity.Remarks" autocomplete="off" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
      </a-row>
    </a-form-model>
    <bad-detail :disabled="disabled" v-model="listDetail"></bad-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Bad.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Bad.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
      <a-button :disabled="disabled" type="primary" @click="handleSubmit" v-if="entity.Status === 0">保存</a-button>
    </div>
  </a-drawer>
</template>

<script>
import moment from 'moment'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import BadDetail from '../TD_BadDetail/List'
export default {
  components: {
    EnumSelect,
    BadDetail
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
      entity: { Id: '', Type: '', Status: 0 },
      listDetail: [],
      rules: {
        BadTime: [{ required: true, message: '请选择报损时间', trigger: 'change' }],
        Type: [{ required: true, message: '请选择报损类型', trigger: 'change' }]
      }
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = { Id: '', Status: 0, BadTime: moment() }
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Bad/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          this.entity = resJson.Data
          this.entity.BadTime = moment(this.entity.BadTime)
          this.listDetail = [...resJson.Data.BadDetails]
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
        var badDetails = this.listDetail.map((v, i, arr) => {
          var result = { ...v }
          result.FromLocal = null
          result.Tray = null
          result.TrayZone = null
          result.Material = null
          result.Measure = null
          return result
        })
        var entityData = { ...this.entity }
        entityData.BadDetails = badDetails

        this.$http.post('/TD/TD_Bad/SaveData', entityData).then(resJson => {
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
      this.$http.post('/TD/TD_Bad/Audit', { Id: id, AuditType: type }).then(resJson => {
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
