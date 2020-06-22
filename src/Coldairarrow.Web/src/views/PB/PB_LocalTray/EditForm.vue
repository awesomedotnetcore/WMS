<template>
  <a-modal
    ref="form"    
    :title="title"
    width="70%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-transfer 
      :data-source="locationList"
      show-search
      :list-style="{width: '250px',height: '300px'}"
      :target-keys="targetKeys"
      :render="item => `${item.title}(${item.description})`"
      @change="handlelocationChange"
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
      locationList: [],
      targetKeys: [],
      typeId:''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.locationList = []
      this.targetKeys = []
    },
    openForm(id, title) {
     this.typeId = id
      this.init()
      this.title = title
      this.getLocationList()
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_LocalTray/GetDataListByTypeId?TypeId=' + id).then(resJson => {
          this.loading = false
          resJson.Data.forEach(element => {
            this.targetKeys.push(element.PB_Location.Id)
          })
        })
      }
    },
    handleSubmit() {
        this.loading = true
        this.$http.post('/PB/PB_LocalTray/SaveDatas', {id:this.typeId,keys:this.targetKeys}).then(resJson => {
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
    getLocationList() {
      var thisObj = this
      this.locationList = []
      this.loading = true
      this.$http.post('/PB/PB_Location/GetAllData').then(resJson => {
        thisObj.loading = false
        resJson.Data.forEach(element => {
          thisObj.locationList.push({
            key: element.Id,
            title: element.Name,
            description: element.Code,
            chosen: false
          })
        })
      })
    },
    handlelocationChange(selectedRowKeys) {
      this.targetKeys = selectedRowKeys
    }
  }
}
</script>
