<template>
  <a-card title="收货明细" :loading="loading">
    <div slot="extra">
      <a-button v-if="entity.Status===0" type="primary" ghost :style="{marginRight:'10px'}" @click="handleAudit('Confirm')" :loading="loading">确认</a-button>
      <a-button v-if="entity.Status===0" type="danger" ghost @click="handleDel" :loading="loading">删除</a-button>
      <a-button v-if="entity.Status===1" type="primary" ghost :style="{marginRight:'10px'}" @click="handleAudit('Approve')" :loading="loading">通过</a-button>
      <a-button v-if="entity.Status===1" type="danger" ghost @click="handleAudit('Reject')" :loading="loading">驳回</a-button>

      <a-button v-if="entity.Status===3 || entity.Status===5" type="primary" ghost :style="{marginRight:'10px'}" @click="$router.push({path:'/InStorage/ManualIn',query:{recId:id}})" :loading="loading">手动入库</a-button>
      <a-button v-if="entity.Status===3 || entity.Status===5" type="danger" ghost @click="$router.push({path:'/InStorage/AutoIn',query:{recId:id}})" :loading="loading">自动入库</a-button>
    </div>
    <a-descriptions :title="entity.Code" bordered :column="1">
      <a-descriptions-item label="时间">{{ moment(entity.RecTime).format('YY/M/D H:m') }}</a-descriptions-item>
      <a-descriptions-item label="状态">
        <a-badge v-if="entity.Status==0" status="processing" text="编制中" />
        <a-badge v-if="entity.Status==1" status="success" text="已确认" />
        <a-badge v-if="entity.Status==3" status="success" text="审核通过" />
        <a-badge v-if="entity.Status==4" status="error" text="审核失败" />
        <a-badge v-if="entity.Status==5" status="success" text="部分入库" />
        <a-badge v-if="entity.Status==6" status="success" text="全部入库" />
      </a-descriptions-item>
      <a-descriptions-item label="总数">{{ entity.TotalNum }}</a-descriptions-item>
      <a-descriptions-item label="物料">
        <a-descriptions v-for="item in entity.RecDetails" :key="item.Id" bordered :column="1" size="small" :style="{marginBottom:'10px'}">
          <a-descriptions-item label="名称">{{ item.Material.Name }}</a-descriptions-item>
          <a-descriptions-item label="编码">{{ item.Material.Code }}</a-descriptions-item>
          <a-descriptions-item label="条码">{{ item.Material.BarCode }}</a-descriptions-item>
          <a-descriptions-item label="数量">{{ item.RecNum }}{{ item.Measure.Name }}</a-descriptions-item>
          <a-descriptions-item label="入库">{{ item.InNum }}{{ item.Measure.Name }}</a-descriptions-item>
        </a-descriptions>
      </a-descriptions-item>
    </a-descriptions>
  </a-card>
</template>

<script>
import moment from 'moment'
import ReceiveSvc from '../../api/TD/ReceiveSvc'
export default {
  props: {
    id: { type: String, required: true }
  },
  data() {
    return {
      loading: false,
      entity: {}
    }
  },
  mounted() {
    this.getData()
  },
  methods: {
    moment,
    getData() {
      this.loading = true
      ReceiveSvc.GetTheData(this.id).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.entity = resJson.Data
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    handleAudit(type) {
      this.loading = true
      ReceiveSvc.Approval(this.id, type).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success(resJson.Msg)
          this.getData()
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    handleDel() {
      this.loading = true
      ReceiveSvc.DeleteData(this.id).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success(resJson.Msg)
          this.$router.go(-1)
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>

<style>
</style>