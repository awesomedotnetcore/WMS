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
        <a-form-model-item label="盘点时间" prop="CheckTime">
          <a-date-picker v-model="entity.CheckTime" show-time autocomplete="off"/>
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="盘点类型" prop="Type">
          <enum-select code="CheckType" v-model="entity.Type" :allowClear="true"></enum-select>
        </a-form-model-item>
        <a-form-model-item v-if="entity.Type==='Area'" label="货区" prop="RefCode">
          <storarea-select v-model="CheckArea"></storarea-select>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>

import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import StorareaSelect from '../../../components/PB/StorAreaSelect'
import moment from 'moment'
export default {
  components:{
    EnumSelect,
    StorareaSelect
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
      CheckArea:[]
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
        this.$http.post('/TD/TD_Check/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.CheckTime = moment(this.entity.CheckTime)
        })

        this.$http.post('/TD/TD_CheckArea/Query?checkId='+id).then(resJson => {
          this.CheckArea = resJson.Data
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_Check/PushData', {Data:this.entity, Ids: this.CheckArea}).then(resJson => {
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
