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
        <a-form-model-item label="物料类型" prop="MaterialTypeId">
          <materialType-select v-model="entity.MaterialTypeId"></materialType-select>
        </a-form-model-item>        
        <a-form-model-item label="物料编码" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" :disabled="$para('MaterialCode')=='1'" placeholder="系统自动生成"><a-icon slot="prefix" type="scan" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" ><a-icon slot="prefix" type="scan" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="物料名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" ><a-icon slot="prefix" type="paper-clip" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="物料简称" prop="SimpleName">
          <a-input v-model="entity.SimpleName" autocomplete="off" ><a-icon slot="prefix" type="paper-clip" /></a-input>
        </a-form-model-item>        
        <a-form-model-item label="单位" prop="MeasureId">
          <measure-select v-model="entity.MeasureId"></measure-select>
        </a-form-model-item>
        <a-form-model-item label="物料规格" prop="Spec">
          <a-input v-model="entity.Spec" autocomplete="off" ><a-icon slot="prefix" type="profile" /></a-input>
        </a-form-model-item>
        <a-form-model-item label="上限下限范围" prop="Max">
          <a-input-number v-model="entity.Min" autocomplete="off"/>
          -
          <a-input-number v-model="entity.Max" autocomplete="off"/>
        </a-form-model-item>
        <a-form-model-item label="客户" prop="CusId">
          <customer-select v-model="entity.CusId"></customer-select>
        </a-form-model-item>
        <a-form-model-item label="供应商" prop="SupId">
          <supplier-select v-model="entity.SupId"></supplier-select>
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input-number v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off"></a-textarea>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import CustomerSelect from '../../../components/PB/CustomerSelect'
import MaterialTypeSelect from '../../../components/PB/MaterialTypeSelect'
import SupplierSelect from '../../../components/PB/SupplierSelect'
import MeasureSelect from '../../../components/PB/MeasureSelect'
export default {
  components: {
    MeasureSelect,
    CustomerSelect,
    MaterialTypeSelect,
    SupplierSelect
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
        this.$http.post('/PB/PB_Material/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Material/SaveData', this.entity).then(resJson => {
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
