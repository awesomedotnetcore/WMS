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
        <a-form-model-item label="盘点编码" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" :disabled="$para('CheckCode')=='1'" placeholder="系统自动生成">
                <a-icon slot="prefix" type="scan" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="盘点时间" prop="CheckTime">
          <a-date-picker v-model="entity.CheckTime" :style="{width:'100%'}" autocomplete="off"/>
        </a-form-model-item>     
        <a-form-model-item label="盘点类型" prop="Type">
          <enum-select code="CheckType" v-model="entity.Type" :allowClear="true"></enum-select>
        </a-form-model-item>              
        <a-form-model-item v-if="entity.Type==='Area'" label="货区" prop="CheckArea">
          <storarea-select v-model="CheckArea"></storarea-select>
        </a-form-model-item>
        <a-form-model-item v-else-if="entity.Type==='Random'" label="抽取百分比" prop="RandomPer">
          <a-input-number v-model="entity.RandomPer" :defaultValue="1" :min="1" :max="100" :formatter="value => `${value}%`" :parser="value => value.replace('%', '')"/>
          <a-button type="danger" :disabled="this.entity.RandomPer==null" @click="handleRandom">抽取</a-button>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <material-list v-if="entity.Type==='Material'" :checkId="entity.Id" ref="materialList"></material-list>
    <random-material v-if="entity.Type==='Random'" :checkId="entity.Id" ref="randomMaterial"></random-material> 
  </a-modal>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import StorareaSelect from '../../../components/PB/StorAreaSelect'
import MaterialList from './MaterialList'
import RandomMaterial from './RandomMaterial'
import moment from 'moment'
export default {
  components:{
    EnumSelect,
    StorareaSelect,
    MaterialList,
    RandomMaterial
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
        CheckTime: [{ required: true, message: '请输入盘点时间', trigger: 'change' }],
        Type: [{ required: true, message: '请选择盘点类型', trigger: 'change' }],
      },
      title: '',
      CheckArea:[],
      CheckMaterial:[]
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {Type : '', CheckTime: moment().locale('zh-cn').format('YYYY-MM-DD')}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()
      this.title = title
      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Check/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.CheckTime = moment(this.entity.CheckTime).format('YYYY-MM-DD');

          if(this.entity.Type=='Area'){
            this.$http.post('/TD/TD_CheckArea/Query?checkId='+id).then(resJson => {
              this.CheckArea = resJson.Data
            })
          }
        })
      }
    },
    handleSubmit() {
      if(this.entity.Type=='Material'){
        this.CheckArea=[]
        this.CheckMaterial=this.$refs.materialList.getAllKeys()
      }else if(this.entity.Type=='Area'){
        this.CheckMaterial=[]
      }else if(this.entity.Type=='Random'){
        this.CheckArea=[]
        this.CheckMaterial=this.$refs.randomMaterial.getAllKeys()
      }else{
        this.CheckArea=[]
        this.CheckMaterial=[]
      }

      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_Check/PushData', {
            Data:this.entity, 
            AreaIdList: this.CheckArea, 
            MaterialIdList: this.CheckMaterial
          }).then(resJson => {
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
    handleRandom(){
      this.$refs.randomMaterial.handleRandom(this.entity.RandomPer)
    }
  }
}
</script>
