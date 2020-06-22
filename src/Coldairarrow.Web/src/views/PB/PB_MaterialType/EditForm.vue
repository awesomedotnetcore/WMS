<template>
  <a-modal :title="title" width="40%" :visible="visible" :confirmLoading="loading" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="上级物料分类" prop="ParentId">
          <a-tree-select :style="{width:'100%'}" allowClear :dropdownStyle="{ maxHeight: '400px', overflow: 'auto' }" :treeData="ParentIdTreeData" placeholder="请选择上级物料分类" treeDefaultExpandAll v-model="entity.ParentId"></a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="物料分类编码" prop="Code">
          <a-input v-model="entity.Code" :disabled="$para('MaterialTypeCode')=='1'" placeholder="系统自动生成" autocomplete="off">
            <a-icon slot="prefix" type="scan" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="物料分类名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off">
            <a-icon slot="prefix" type="paper-clip" />
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
      rules: {
        Name: [{ required: true, message: '请输物料分类名称', trigger: 'blur' }]
      },
      title: '',
      ParentIdTreeData: []
    }
  },
  methods: {
    init() {
      this.$http.post('/PB/PB_MaterialType/GetTreeDataList').then(resJson => {
        if (resJson.Success) {
          this.ParentIdTreeData = resJson.Data
        }
      })
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
        this.$http.post('/PB/PB_MaterialType/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_MaterialType/SaveData', this.entity).then(resJson => {
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
