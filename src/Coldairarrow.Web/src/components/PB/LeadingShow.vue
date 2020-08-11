<template>
  <a-modal
    :title="title"
    width="25%"
    :bodyStyle="{height:'150px'}"
    :visible="visible"
    :confirmLoading="loading"
    :footer="null"
    @cancel="()=>{this.visible=false}"
  >    
  <template>
    <a-row :gutter="10">
      <a-col :md="14" :sm="24">
        <a-upload          
          name="file"
          :multiple="true"
          :action="$rootUrl+leading"
          :headers="headers"
          @change="handleChange"          
        >
        <a-button type="primary"> <a-icon type="upload" /> 上传数据 </a-button>
        </a-upload>
        </a-col>  

        <div>
          <a-button @click="()=>{this.visible=false}"><a-icon type="download" /> 
          <a :href="$rootUrl+templet">下载模板表</a>
          </a-button>
        </div>
    </a-row>
    </template>    
  </a-modal>  
</template>

<script>
import Token from '../../utils/cache/TokenCache'

export default {
  components: {
    Token,
  },
  props: {    
    parentObj: Object,
    leading: {type: String,  required: false},
    templet: {type: String,  required: false}
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
      headers: {
        authorization: 'Bearer '+Token.getToken(),
      },
    }
  },
  methods: {
    init() {
      this.visible = true
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
        if(info.file.response.Success === false ){
          this.$message.error(`${info.file.name}文件,${info.file.response.Msg},请检查文件内容！ `)
        }else 
          this.$message.success(`${info.file.name}  文件上传成功！`);
        this.visible = false

      } else if (info.file.status === 'error') {
        this.$message.error(`${info.file.name} 文件上传失败！`);
      }
    },
  }
}
</script>
