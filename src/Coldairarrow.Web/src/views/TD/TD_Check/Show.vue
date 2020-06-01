<template>
  <a-modal
    :title="title"
    width="80%"
    :visible="visible"
    :confirmLoading="loading"
    :footer="null"
    :closable=false
  >
    <a-spin :spinning="loading">
      <div style="background-color: #F5F5F5; padding: 12px;">
        <a-page-header :ghost="false" title="物料盘差" sub-title="盘点管理">
          <template slot="extra">
            <a-button key="1" type="primary">导出Excel</a-button>
            <a-button key="2" type="danger" @click="()=>{this.visible=false}">关闭</a-button>
          </template>
          <a-descriptions size="small" :column="3">
            <a-descriptions-item label="盘点时间">{{entity.CheckTime}}</a-descriptions-item>
            <a-descriptions-item label="关联单号">{{entity.RefCode}}</a-descriptions-item>
            <a-descriptions-item label="盘点类型">{{entity.Type}}</a-descriptions-item>
            <a-descriptions-item label="盘点区域">

            </a-descriptions-item>
            <a-descriptions-item label="盘点状态">{{entity.IsComplete}}</a-descriptions-item>
            <a-descriptions-item label="审核状态">{{entity.Status}}</a-descriptions-item>
          </a-descriptions>
        </a-page-header>
        <a-divider>物料盘差清单</a-divider>
        <a-card :bordered="false">
          <a-table :columns="columns" :data-source="data" size="small" />
        </a-card>
      </div>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'
const columns = [
  {
    title: 'Name',
    dataIndex: 'name'
  },
  {
    title: 'Age',
    dataIndex: 'age'
  },
  {
    title: 'Address',
    dataIndex: 'address'
  }
]
const data = [
  {
    key: '1',
    name: 'John Brown',
    age: 32,
    address: 'New York No. 1 Lake Park'
  },
  {
    key: '2',
    name: 'Jim Green',
    age: 42,
    address: 'London No. 1 Lake Park'
  },
  {
    key: '3',
    name: 'Joe Black',
    age: 32,
    address: 'Sidney No. 1 Lake Park'
  }
]

export default {
  components: {},
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
      data,
      columns,
      CheckArea: [],
      CheckMaterial: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = { Type: '' }
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Check/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
          this.entity.CheckTime = moment(this.entity.CheckTime)

          if (this.entity.Type == 'Area') {
            this.$http.post('/TD/TD_CheckArea/Query?checkId=' + id).then(resJson => {
              this.CheckArea = resJson.Data
            })
          }
        })
      }
    },
    handleRandom() {
      this.$refs.randomMaterial.handleRandom(this.entity.RandomPer)
    }
  }
}
</script>
