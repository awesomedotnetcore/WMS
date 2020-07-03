<template>
  <a-drawer title="移库" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="7">
          <a-form-model-item label="移库单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateMoveCode')=='1' || disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="移库时间" prop="MoveTime">
            <a-date-picker v-model="entity.MoveTime" :style="{width:'100%'}" :disabled="disabled" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="移库类型" prop="Type">
            <enum-select code="MoveStorageType" v-model="entity.Type" :disabled="disabled"></enum-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="7">
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
    <move-detail :disabled="disabled" v-model="listDetail"></move-detail>
    <div :style="{ position:'absolute',right:0,bottom:0,width:'100%',borderTop:'1px solid #e9e9e9',padding:'10px 16px',background:'#fff',textAlign:'right',zIndex: 1}">
      <a-button :style="{ marginRight: '8px' }" @click="()=>{this.visible=false}">取消</a-button>
      <a-button type="primary" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Move.Auditing')" @click="handleAudit(entity.Id,'Approve')">通过</a-button>
      <a-button type="danger" :style="{ marginRight: '8px' }" v-if="entity.Id !== '' && entity.Status === 0 && disabled && hasPerm('TD_Move.Auditing')" @click="handleAudit(entity.Id,'Reject')">驳回</a-button>
      <a-button :disabled="disabled" type="primary" @click="handleSubmit" v-if="entity.Status === 0">保存</a-button>
    </div>
  </a-drawer>
</template>

<script>
import moment from 'moment'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import MoveDetail from '../TD_MoveDetail/List'
export default {
  components: {
    EnumSelect,
    MoveDetail
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
        MoveTime: [{ required: true, message: '请选择移库时间', trigger: 'change' }],
        Type: [{ required: true, message: '请选择移库类型', trigger: 'change' }]
      }
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = { Id: '', Status: 0, MoveTime: moment() }
      this.listDetail = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Move/GetTheData', { id: id }).then(resJson => {
          this.loading = false
          this.entity = resJson.Data
          this.entity.MoveTime = moment(this.entity.MoveTime)
          this.listDetail = [...resJson.Data.MoveDetails]
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
        var moveDetails = this.listDetail.map((v, i, arr) => {
          var result = { ...v }
          result.FromLocal = null
          result.FromTray = null
          result.FromZone = null
          result.Material = null
          result.Measure = null
          result.ToLocal = null
          result.ToTray = null
          result.ToZone = null
          return result
        })
        var entityData = { ...this.entity }
        entityData.MoveDetails = moveDetails

        this.$http.post('/TD/TD_Move/SaveData', entityData).then(resJson => {
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
      this.$http.post('/TD/TD_Move/Audit', { Id: id, AuditType: type }).then(resJson => {
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
