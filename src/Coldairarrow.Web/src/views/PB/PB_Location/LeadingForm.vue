<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    :confirmLoading="loading"
    :footer="null"
    @cancel="()=>{this.visible=false}"
  >    
  <template>
      <a-upload
        name="file"
        :multiple="true"
        :action="$rootUrl+'/PB/PB_Location/Import'"
        :headers="headers"
        @change="handleChange"
      >
        <a-button> <a-icon type="upload" /> 上传货位 </a-button>
      </a-upload>

      <div>
          <a href="">指定路径导出</a>
      </div>
    </template>

    
  </a-modal>
  
</template>

<script>
import Token from '../../../utils/cache/TokenCache'

export default {
  components: {
    Token,
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
     // entity: {},
      rules: {},
      title: '',
      headers: {
        authorization: 'Bearer '+Token.getToken(),
      },
    }
  },
  methods: {
    init() {
      this.visible = true
     // this.entity = {}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
    },
    handleChange(info) {
      if (info.file.status !== 'uploading') {
        console.log(info.file, info.fileList);
      }
      if (info.file.status === 'done') {
        this.$message.success(`${info.file.name}  文件上传成功！`);
      } else if (info.file.status === 'error') {
        this.$message.error(`${info.file.name} 文件上传失败！`);
      }
    },
  }
}
</script>
