<template>
  <a-modal
    :title="title"
    width="70%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-transfer
      :data-source="materialList"
      show-search
      :list-style="{width: '250px',height: '300px'}"
      :target-keys="targetKeys"
      :render="item => `${item.title}(${item.description})`"
      @change="handleMaterialChange"
    ></a-transfer>
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
      rules: {},
      title: '',
      materialList: [],
      // selectedMaterials: [],
      targetKeys: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.materialList = []
      // this.selectedMaterials = []
      this.targetKeys = []
      // this.$nextTick(() => {
      //   thisObj.$refs['form'].clearValidate()
      // })
    },
    openForm(typeId, title) {
      var thisObj = this
      this.title = title
      this.typeId = typeId
      this.init()
      this.getMaterialList()
      if (typeId) {
        this.loading = true
        this.$http.post('/PB/PB_TrayMaterial/GetDataListByTypeId?typeId=' + typeId).then(resJson => {
          this.loading = false
          resJson.Data.forEach(element => {
            // thisObj.selectedMaterials.push({
            //   key: element.PB_Material.Id,
            //   title: element.PB_Material.Name,
            //   description: element.PB_Material.Code
            // })
            thisObj.targetKeys.push(element.PB_Material.Id)
          })
        })
      }
    },
    handleSubmit() {
      this.loading = true
      this.$http.post('/PB/PB_TrayMaterial/SaveDatas?typeId=' + this.typeId, this.targetKeys).then(resJson => {
        this.loading = false

        if (resJson.Success) {
          this.$message.success('操作成功!')
          this.visible = false

          this.parentObj.getDataList()
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    getMaterialList() {
      var thisObj = this
      this.materialList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetAllDataList').then(resJson => {
        thisObj.loading = false
        resJson.Data.forEach(element => {
          thisObj.materialList.push({
            key: element.Id,
            title: element.Name,
            description: element.Code,
            chosen: false
          })
        })
        // thisObj.selectedMaterials.forEach(e => {
        //   thisObj.materialList.forEach(m => {
        //     if (e.key === m.key) {
        //       m.chosen = true
        //       // thisObj.targetKeys.push(m.key)
        //     }
        //   })
        // })
      })
    },
    handleMaterialChange(selectedRowKeys) {
      this.targetKeys = selectedRowKeys
    }
  }
}
</script>
