<template>
  <a-drawer title="入库" :width="1200" :maskClosable="false" placement="right" :visible="visible" @close="()=>{this.visible=false}" :body-style="{ paddingBottom: '80px' }">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="8">
          <a-form-model-item label="入库单号" prop="Code">
            <a-input v-model="entity.Code" :disabled="$para('GenerateInStorageCode')=='1'" placeholder="系统自动生成" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="入库时间" prop="InStorTime">
            <a-date-picker v-model="entity.InStorTime" />
          </a-form-model-item>
        </a-col>
        <a-col :span="8">
          <a-form-model-item label="入库类型" prop="InType">
            <enum-select code="InStorageType" v-model="entity.InType"></enum-select>
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
          <a-form-model-item label="供应商" prop="SupId">
            <sup-select v-model="entity.SupId"></sup-select>
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
import SupSelect from '../../../components/PB/SupplierSelect'
import ListDetail from '../TD_InStorDetail/List'
export default {
  components: {
    EnumSelect,
    EnumName,
    SupSelect,
    ListDetail
  },
  props: {
    parentObj: { type: Object, required: true }
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: { InType: '' },
      listDetail: [],
      rules: {}
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = { InType: '' }
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_InStorage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/TD/TD_InStorage/SaveData', this.entity).then(resJson => {
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
