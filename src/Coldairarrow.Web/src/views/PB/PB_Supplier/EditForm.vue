<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="供应商编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" :disabled="$para('SupplierCode')=='1'" placeholder="系统自动生成">
            <a-icon slot="prefix" type="scan" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="供应商名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off">
            <a-icon slot="prefix" type="user" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="供应商类型" prop="Type">
          <enum-select code="SupplierType" v-model="entity.Type"></enum-select>
        </a-form-model-item>
        <a-form-model-item label="电话" prop="Phone">
          <a-input v-model="entity.Phone" autocomplete="off">
            <a-icon slot="prefix" type="phone" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="传真" prop="Fax">
          <a-input v-model="entity.Fax" autocomplete="off">
            <a-icon slot="prefix" type="printer" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="Email" prop="Email">
          <a-input v-model="entity.Email" autocomplete="off">
            <a-icon slot="prefix" type="mail" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="联系人" prop="ContactName">
          <a-input v-model="entity.ContactName" autocomplete="off">
            <a-icon slot="prefix" type="idcard" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="地址" prop="Address">
          <a-input v-model="entity.Address" autocomplete="off">
            <a-icon slot="prefix" type="compass" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off"></a-textarea>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
export default {
  components: {
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
        Name: [{ required: true, message: '请输入供应商名称', trigger: 'blur' }],
        Type: [{ required: true, message: '请选择供应商类型', trigger: 'change' }]
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
        this.$http.post('/PB/PB_Supplier/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Supplier/SaveData', this.entity).then(resJson => {
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
