<template>
  <a-modal
    :title="title"
    width="60%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="计划编号" prop="Code">
              <a-input
                v-model="entity.Code"
                autocomplete="off"
                :disabled="$para('PlanCode')=='1'"
                placeholder="系统自动生成"
              >
                <a-icon slot="prefix" type="scan" />
              </a-input>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="Bom版本" prop="BomVerId">
              <a-input v-model="entity.BomVerId" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="物料" prop="MaterialId">
              <materila-select v-model="entity.MaterialId" @select="handleMaterialSelect"></materila-select>
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="批次号" prop="BatchNo">
              <a-input v-model="entity.BatchNo" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="数量" prop="Num">
              <a-input-number
                v-model="entity.Num"
                placeholder="数量"
                :style="{width:'100%'}"
                autocomplete="off"
              />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="计划日期" prop="PlanDate">
              <a-date-picker v-model="entity.PlanDate" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="开始日期" prop="StartDate">
              <a-date-picker v-model="entity.StartDate" :style="{width:'100%'}" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="完成日期" prop="FinishDate">
              <a-date-picker
                v-model="entity.FinishDate"
                :style="{width:'100%'}"
                autocomplete="off"
              />
            </a-form-model-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="备注" prop="Remark">
              <a-input v-model="entity.Remark" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="订单编号" prop="OrderNo">
              <a-input v-model="entity.OrderNo" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12"></a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="生产单元" prop="UnitCode">
              <a-input v-model="entity.UnitCode" autocomplete="off" />
            </a-form-model-item>
          </a-col>
          <a-col :span="12">
            <a-form-model-item label="上级系统Id" prop="RefId">
              <a-input v-model="entity.RefId" autocomplete="off" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'
import MaterilaSelect from '../../../components/Material/MaterialSelect'

export default {
  components: {
    MaterilaSelect,
  },
  props: {
    parentObj: Object,
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 },
      },
      visible: false,
      loading: false,
      entity: {},
      rules: {
        MaterialId: [{ required: true, message: '请选择物料', trigger: 'change' }],
        BomVerId: [{ required: true, message: '请输入bom版本', trigger: 'blur' }],
        BatchN: [{ required: true, message: '请输入批次号', trigger: 'blur' }],
        Num: [{ required: true, message: '请输入数量', trigger: 'blur' }],
        UnitCode: [{ required: true, message: '请输入生产单元', trigger: 'blur' }],
        PlanDate: [{ required: true, message: '请选择计划日期', trigger: 'change' }],
        StartDate: [{ required: true, message: '请选择计划开始日期', trigger: 'change' }],
        FinishDate: [{ required: true, message: '请选择计划完成日期', trigger: 'change' }],
      },
      material: null,
      title: '',
    }
  },
  methods: {
    moment,
    init() {
      this.visible = true
      this.entity = {}
      // this.material = entity.Material
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
      if (id) {
        this.loading = true
        this.$http.post('/PD/PD_Plan/GetTheData', { id: id }).then((resJson) => {
          this.loading = false
          resJson.Data.StartDate=moment(resJson.Data.StartDate)
          resJson.Data.FinishDate=moment(resJson.Data.FinishDate)
          resJson.Data.PlanDate=moment(resJson.Data.PlanDate)
          this.entity = resJson.Data
        })
      }
    },
    handleMaterialSelect(material) {
      console.log('handleMaterialSelect', material)
      this.material = material
      this.entity.Price = material.Price
    },
    handleSubmit() {
      this.$refs['form'].validate((valid) => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/PD/PD_Plan/SaveData', this.entity).then((resJson) => {
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
  },
}
</script>
