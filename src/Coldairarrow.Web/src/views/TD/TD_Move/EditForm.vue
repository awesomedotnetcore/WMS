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
        <a-form-model-item label="移库单号" prop="Code">
          <a-input
            v-model="entity.Code"
            :disabled="$para('GenerateMoveCode')=='1'"
            :placeholder="$para('GenerateMoveCode')=='1'?'系统自动生成':'移库单号'"
            autocomplete="off"
          />
        </a-form-model-item>
        <a-form-model-item label="移库时间" prop="MoveTime">
          <a-date-picker show-time v-model="entity.MoveTime" :style="{width:'100%'}" />
        </a-form-model-item>
        <a-form-model-item label="移库类型" prop="Type">
          <enum-select code="MoveStorageType" v-model="entity.Type"></enum-select>
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import moment from 'moment'

export default {
  components: {
    EnumName,
    EnumSelect
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
        Type: [{ required: true, message: '请选择类型', trigger: 'change' }],
        MoveTime: [{ required: true, message: '请输入时间', trigger: 'change' }]
      },
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
      this.title = title

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Move/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.MoveTime = moment(this.entity.MoveTime)
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_Move/SaveData', this.entity).then(resJson => {
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
    }
  }
}
</script>
