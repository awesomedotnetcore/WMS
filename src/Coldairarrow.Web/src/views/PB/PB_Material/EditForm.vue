<template>
  <a-modal :title="title" width="50%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="名称" prop="Name">
              <a-input v-model="entity.Name" autocomplete="off">
                <a-icon slot="prefix" type="paper-clip" />
              </a-input>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="简称" prop="SimpleName">
              <a-input v-model="entity.SimpleName" autocomplete="off">
                <a-icon slot="prefix" type="paper-clip" />
              </a-input>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="编码" prop="Code">
              <a-input v-model="entity.Code" autocomplete="off" :disabled="$para('MaterialCode')=='1'" placeholder="系统自动生成">
                <a-icon slot="prefix" type="scan" />
              </a-input>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="条码" prop="BarCode">
              <a-input v-model="entity.BarCode" autocomplete="off">
                <a-icon slot="prefix" type="scan" />
              </a-input>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="类型" prop="MaterialTypeId">
              <materialType-select v-model="entity.MaterialTypeId"></materialType-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="规格" prop="Spec">
              <a-input v-model="entity.Spec" autocomplete="off">
                <a-icon slot="prefix" type="profile" />
              </a-input>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="下限" prop="Max">
              <a-input-number v-model="entity.Min" placeholder="下限" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="上限" prop="Max">
              <a-input-number v-model="entity.Max" placeholder="上限" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="单价" prop="Price">
              <a-input-number v-model="entity.Price" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="单位" prop="MeasureId">
              <measure-select v-model="entity.MeasureId"></measure-select>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="客户" prop="CusId">
              <customer-select v-model="entity.CusId"></customer-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="供应商" prop="SupId">
              <supplier-select v-model="entity.SupId"></supplier-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12"></a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="仓库" prop="StorId">
              <storage-select v-model="entity.StorId" ></storage-select>
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off"></a-textarea>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import StorageSelect from '../../../components/Storage/StorageSelect'
import CustomerSelect from '../../../components/PB/CustomerSelect'
import MaterialTypeSelect from '../../../components/PB/MaterialTypeSelect'
import SupplierSelect from '../../../components/PB/SupplierSelect'
import MeasureSelect from '../../../components/PB/MeasureSelect'
export default {
  components: {
    StorageSelect,
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
      rules: {
        StorId: [{ required: true, message: '请选择仓库', trigger: 'change' }],
        Name: [{ required: true, message: '请输入物料名称', trigger: 'blur' }],
        MaterialTypeId: [{ required: true, message: '请选择物料类型', trigger: 'change' }],
        MeasureId: [{ required: true, message: '请选择物料单位', trigger: 'change' }],
        Price: [{ required: true, message: '请输入物料单价', trigger: 'change' }]
      },
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {Price: 0 }
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
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
