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
        <!-- <a-form-model-item label="所属仓库" prop="StorId" >
          <a-input v-model="entity.StorId" autocomplete="off" />          
        </a-form-model-item> -->
        <!-- <storage-select code="Storage" v-model="entity.StorId"></storage-select> -->
        <a-form-model-item label="巷道编号" prop="Code">
          <a-input v-model="entity.Code" :disabled="$para('GenerateLanewayCode')=='1'" placeholder="系统自动生成" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="巷道名称" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import StorageSelect from '../../../components/Storage/StorageSelect' 

export default {
  components: {
    StorageSelect
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
        Name: [{ required: true, message: '请输入巷道名称', trigger: 'blur' }]
      },
      title: '',
      storid: ''
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
    openForm(id,storid,title) {
      this.init()
      this.title = title
     // this.storid = storid
      this.entity.StorId = storid

     // console.log('ID名称',storid)
      if (id) {
        this.loading = true
        this.$http.post('/PB/PB_Laneway/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/PB/PB_Laneway/SaveData', this.entity).then(resJson => {
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
