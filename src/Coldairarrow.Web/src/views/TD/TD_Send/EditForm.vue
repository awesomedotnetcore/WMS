<template>
  <a-drawer title="出库" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
        <a-col :span="8">
          <a-form-model-item label="发货编号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateOutStorageCode')=='1'|| disabled" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="关联单号" prop="RefCode">
            <a-input v-model="entity.RefCode" autocomplete="off" :disabled="disabled"/>
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="发货类型" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
          </a-form-model-item>          
        </a-col>               
      </a-row>
        
      <a-row>
        <a-col :span="8">          
          <a-form-model-item label="单据日期" prop="OrderTime">
          <a-input v-model="entity.OrderTime" autocomplete="off" />
        </a-form-model-item>
        </a-col>          
        <a-col :span="8">
          <a-form-model-item label="发货日期" prop="SendTime">
            <a-date-picker v-model="entity.SendTime"  :style="{width:'100%'}" :disabled="disabled"/>
          </a-form-model-item>          
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="客户ID" prop="CusId">
          <a-input v-model="entity.CusId" autocomplete="off" />
          </a-form-model-item>          
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
          </a-form-model-item>
        </a-col>
      </a-row>      
      </a-form-model>
      <list-detail v-model="listDetail" :disabled="disabled"></list-detail>
  </a-drawer>
</template>

<script>
import ListDetail from '../TD_SendDetail/List'
export default {
  components: {
    // EnumSelect,
    // EnumName,
    ListDetail,
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
        this.$http.post('/TD/TD_Send/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_Send/SaveData', this.entity).then(resJson => {
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
