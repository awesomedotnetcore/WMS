<template>
  <a-card title="计划明细" :loading="loading">
    <a-descriptions :title="entity.Code" bordered :column="1">
      <a-descriptions-item label="计划编号">{{ entity.Code }}</a-descriptions-item>
      <a-descriptions-item label="物料">{{ entity.Material }}</a-descriptions-item>
      <a-descriptions-item label="Bom版本">{{ entity.BomVerId }}</a-descriptions-item>
      <a-descriptions-item label="批次号">{{ entity.BatchNo }}</a-descriptions-item>
      <a-descriptions-item label="数量">{{ entity.Spec }}</a-descriptions-item>
      <a-descriptions-item label="计划日期">{{ entity.PlanDate }}</a-descriptions-item>
      <a-descriptions-item label="计划开始日期">{{ entity.StartDate }}</a-descriptions-item>
      <a-descriptions-item label="计划结束日期">{{ entity.FinishDate }}</a-descriptions-item>      
      <a-descriptions-item label="生产单元">{{ entity.UnitCode }}</a-descriptions-item>
      <a-descriptions-item label="状态">{{ entity.Status }}</a-descriptions-item>
      <a-descriptions-item label="备注">{{ entity.Remark }}</a-descriptions-item>
    </a-descriptions>
  </a-card>
</template>

<script>
import moment from 'moment'
import PlanSvc from '../../api/PB/PlanSvc'
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
      PlanSvc.GetTheData(this.id).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.entity = resJson.Data
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