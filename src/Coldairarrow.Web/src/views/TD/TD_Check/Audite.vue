<template>
  <a-modal
    :title="title"
    width="80%"
    :visible="visible"
    :confirmLoading="loading"
    :footer="null"
    :closable="false"
  >
    <a-spin :spinning="loading">
      <div style="background-color: #F5F5F5; padding: 12px;">
        <a-page-header :ghost="false" title="物料盘差" sub-title="盘点管理">
          <template slot="extra">
            <a-button key="1" type="primary" @click="handlePass">同意</a-button>
            <a-button key="2" @click="handleBack">退回重盘</a-button>
            <a-button key="3" type="danger" @click="handleReject">不同意</a-button>            
            <a-button key="4" @click="()=>{this.visible=false}">关闭</a-button>
          </template>
          <a-descriptions size="small" :column="3">
            <a-descriptions-item label="盘点时间">{{entity.CheckTime}}</a-descriptions-item>
            <a-descriptions-item label="关联单号">{{entity.RefCode}}</a-descriptions-item>
            <a-descriptions-item label="审核时间"><span v-if="entity.Status===1 || entity.Status===2">{{entity.AuditeTime}}</span></a-descriptions-item>
            <a-descriptions-item label="盘点类型" >
                <enum-name code="CheckType" :value="entity.Type"></enum-name></a-descriptions-item>
            <a-descriptions-item label="盘点状态">
              <a-tag v-if="entity.IsComplete===true">已盘</a-tag>
              <a-tag v-else color="green">待盘</a-tag>
            </a-descriptions-item>
            <a-descriptions-item label="审核状态">
              <span v-if="entity.IsComplete===true">
                <a-tag v-if="entity.Status===0" color="green">待审核</a-tag>
                <a-tag v-else-if="entity.Status===1">审核通过</a-tag>
                <a-tag v-else-if="entity.Status===2" color="red">审核失败</a-tag>
                <a-tag v-else color="green">待审核</a-tag>
              </span>
              <span v-else>-</span>
            </a-descriptions-item>            
            <a-descriptions-item v-if="entity.Type==='Area'" label="盘点区域" :span="3">
                 <storarea-show v-model="CheckArea" :disabled="true"></storarea-show>
            </a-descriptions-item>            
          </a-descriptions>
        </a-page-header>
        <a-divider>物料盘差清单</a-divider>
        <a-card :bordered="false">
          <check-data ref="checkData" :checkId="entity.Id" :isCompleted="entity.IsComplete" :isCheckd="false"></check-data>
        </a-card>
      </div>
    </a-spin>
  </a-modal>
</template>

<script>
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import StorareaShow from '../../../components/PB/StorAreaShow'
import CheckData from './CheckDataList'
import moment from 'moment'

export default {
  components: {
      StorareaShow,
      EnumName,
      CheckData
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
      rules: {},
      title: '',
      CheckArea: [],
      CheckMaterial: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = { Type: '', IsComplete:false }
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Check/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.CheckTime = moment(this.entity.CheckTime).format('YYYY-MM-DD HH:mm:ss')
          if(this.entity.Status===1 || this.entity.Status===2)
          {
              this.entity.AuditeTime = moment(this.entity.AuditeTime).format('YYYY-MM-DD HH:mm:ss')
          }         

          if (this.entity.Type == 'Area') {
            this.$http.post('/TD/TD_CheckArea/Query?checkId=' + id).then(resJson => {
              this.CheckArea = resJson.Data
            })
          }
        })
      }
    },
    handlePass(){
        var base=this;
        this.$confirm({
            title: '确定同意吗?',
            content: h => <div style="color:red;">点击'确定'，将更新库存数据，完成此次盘点。</div>,
            okText: '确定',
            cancelText: '取消',
            onOk() {
                base.loading = true
                base.$http.post('/TD/TD_Check/Pass?Id='+base.entity.Id)
                .then(resJson => {
                    base.loading = false                
                    if(resJson.Success){
                        base.visible = false
                        base.parentObj.getDataList()
                    }else{
                        base.$error({
                            title: '错误',
                            content: resJson.Msg,
                        });
                    }
                })
            },
            class: 'test'
        })
    },
    handleReject(){
        var base=this;
        this.$confirm({
            title: '确定不同意吗?',
            content: h => <div style="color:red;">点击'确定'，结束此次盘点，库存数据不会更新。</div>,
            okText: '确定',
            cancelText: '取消',
            onOk() {
                base.loading = true
                base.$http.post('/TD/TD_Check/Reject?Id='+base.entity.Id)
                .then(resJson => {
                    base.loading = false                
                    if(resJson.Success){
                        base.visible = false
                        base.parentObj.getDataList()
                    }else{
                        base.$error({
                            title: '错误',
                            content: resJson.Msg,
                        });
                    }
                })
            },
            class: 'test'
        })
    },
    handleBack(){
        var base=this;
        this.$confirm({
            title: '确定退回重盘吗?',
            content: h => <div style="color:red;">点击'确定'，要求盘点人员重新盘差。</div>,
            okText: '确定',
            cancelText: '取消',
            onOk() {
                base.loading = true
                base.$http.post('/TD/TD_Check/Back?Id='+base.entity.Id)
                .then(resJson => {
                    base.loading = false                
                    if(resJson.Success){
                        base.visible = false
                        base.parentObj.getDataList()
                    }else{
                        base.$error({
                            title: '错误',
                            content: resJson.Msg,
                        });
                    }
                })
            },
            class: 'test'
        })
    }
  }
}
</script>
