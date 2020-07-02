<template>
  <a-modal
    ref="form"    
    :title="title"
    width="45%"
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
      targetKeys: [],
      areaId:''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.materialList = []
      this.targetKeys = []
    },
    openForm(id, title) {
      this.areaId = id
      this.init()
      this.title = title
      this.getMaterialList()
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_AreaMaterial/GetDataListByAreaId?areaId=' + id).then(resJson => {
          this.loading = false
          resJson.Data.forEach(element => {
            this.targetKeys.push(element.PB_Material.Id)
          })
        })
      }
    },
    handleSubmit() {
        this.loading = true
        this.$http.post('/PB/PB_AreaMaterial/SaveDatas', {id:this.areaId,keys:this.targetKeys}).then(resJson => {
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
      })
    },
    handleMaterialChange(selectedRowKeys) {
      this.targetKeys = selectedRowKeys
    }
  }
}
</script>
