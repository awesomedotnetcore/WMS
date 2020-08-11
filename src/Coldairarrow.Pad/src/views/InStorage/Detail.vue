<template>
  <a-card title="入库明细" :loading="loading">
    <div v-if="entity.Status==0" slot="extra">
      <a-button type="primary" ghost :style="{marginRight:'10px'}" @click="handleAudit('Approve')" :loading="loading">同意</a-button>
      <a-button type="danger" ghost @click="handleAudit('Reject')" :loading="loading">驳回</a-button>
    </div>
    <a-descriptions :title="entity.Code" bordered :column="1">
      <a-descriptions-item label="时间">{{ moment(entity.InStorTime).format('YY/M/D H:m') }}</a-descriptions-item>
      <a-descriptions-item label="状态">
        <a-badge v-if="entity.Status==0" status="processing" text="入库中" />
        <a-badge v-if="entity.Status==1" status="success" text="入库完成" />
        <a-badge v-if="entity.Status==2" status="error" text="入库失败" />
      </a-descriptions-item>
      <a-descriptions-item label="总数">{{ entity.TotalNum }}</a-descriptions-item>
      <a-descriptions-item label="物料">
        <a-descriptions v-for="item in entity.InStorDetails" :key="item.Id" bordered :column="1" size="small" :style="{marginBottom:'10px'}">
          <a-descriptions-item label="名称">{{ item.Material.Name }}</a-descriptions-item>
          <a-descriptions-item label="编码">{{ item.Material.Code }}</a-descriptions-item>
          <a-descriptions-item label="条码">{{ item.Material.BarCode }}</a-descriptions-item>
          <a-descriptions-item label="货位">{{ item.Location.Code }}</a-descriptions-item>
          <a-descriptions-item label="托盘">{{ item.Tray.Code }}</a-descriptions-item>
          <a-descriptions-item label="批次">{{ item.BatchNo }}</a-descriptions-item>
          <a-descriptions-item label="数量">{{ item.Num }}</a-descriptions-item>
        </a-descriptions>
      </a-descriptions-item>
    </a-descriptions>
  </a-card>
</template>

<script>
import moment from 'moment'
import InStorageSvc from '../../api/TD/InStorageSvc'
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
      InStorageSvc.GetTheData(this.id).then(resJson => {
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
      InStorageSvc.Audit(this.id, type).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success(resJson.Msg)
          this.getData()
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