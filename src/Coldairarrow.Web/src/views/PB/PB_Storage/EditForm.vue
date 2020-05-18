<template>
  <a-modal
    :title="title"
    width="50%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="仓库编号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="仓库名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="仓库类型（平库,立库）(枚举)" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否启用托盘管理:" prop="IsTray">
          <a-select style="width: 180px" placeholder="请选择" buttonStyle="solid" v-model="entity.IsTray" autocomplete="off" @select="DataTypeChange">
            <a-select-option value="false" :key="false">否</a-select-option>
            <a-select-option value="true" :key="true">是</a-select-option>
          </a-select>&nbsp;&nbsp;
        </a-form-model-item>
        <a-form-model-item label="是否启用分区管理:" prop="IsZone">
          <a-select style="width: 180px" placeholder="请选择" buttonStyle="solid" v-model="entity.IsZone" autocomplete="off" @select="DataTypeChange">
            <a-select-option value="false" :key="false">否</a-select-option>
            <a-select-option value="true" :key="true">是</a-select-option>
          </a-select>&nbsp;&nbsp;
        </a-form-model-item>
        <a-form-model-item label="仓库是否启用:" prop="disable">
          <a-select style="width: 180px" placeholder="请选择" buttonStyle="solid" v-model="entity.disable" autocomplete="off" @select="DataTypeChange">
            <a-select-option value="false" :key="false">否</a-select-option>
            <a-select-option value="true" :key="true">是</a-select-option>
          </a-select>&nbsp;&nbsp;
        </a-form-model-item>
        <a-form-model-item label="是否默认仓库:" prop="IsDefault">
          <a-select style="width: 180px" placeholder="请选择" buttonStyle="solid" v-model="entity.IsDefault" autocomplete="off" @select="DataTypeChange">
            <a-select-option value="false" :key="false">否</a-select-option>
            <a-select-option value="true" :key="true">是</a-select-option>
          </a-select>&nbsp;&nbsp;
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-textarea v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
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
        this.$http.post('/PB/PB_Storage/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Storage/SaveData', this.entity).then(resJson => {
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
    DataTypeChange(val, option) {
      this.DataType = val
    }
  }
}
</script>
