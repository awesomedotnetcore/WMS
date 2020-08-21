<template>
  <a-card title="物料明细" :loading="loading">
    <a-button slot="extra" type="primary" ghost @click="handlerLMReport(entity)" :loading="loading">库存</a-button>
    <a-descriptions :title="entity.Code" bordered :column="1">
      <a-descriptions-item label="名称">{{ entity.Name }}</a-descriptions-item>
      <a-descriptions-item label="编码">{{ entity.Code }}</a-descriptions-item>
      <a-descriptions-item label="条码" v-if="entity.BarCode">{{ entity.BarCode }}</a-descriptions-item>
      <a-descriptions-item label="简称" v-if="entity.SimpleName">{{ entity.SimpleName }}</a-descriptions-item>
      <a-descriptions-item label="规格" v-if="entity.Spec">{{ entity.Spec }}</a-descriptions-item>
      <a-descriptions-item label="类型">{{ entity.MaterialType.Name }}</a-descriptions-item>
      <a-descriptions-item label="单位">{{ entity.Measure.Name }}</a-descriptions-item>
      <a-descriptions-item label="最大" v-if="entity.Max">{{ entity.Max }}</a-descriptions-item>
      <a-descriptions-item label="最小" v-if="entity.Min">{{ entity.Min }}</a-descriptions-item>
      <a-descriptions-item label="客户" v-if="entity.CusId">{{ entity.Customer.Name }}</a-descriptions-item>
      <a-descriptions-item label="供应商" v-if="entity.SupId">{{ entity.Supplier.Name }}</a-descriptions-item>
      <a-descriptions-item label="时间">{{ moment(entity.CreateTime).format('YY/M/D H:m') }}</a-descriptions-item>
    </a-descriptions>
  </a-card>
</template>

<script>
import moment from 'moment'
import MaterialSvc from '../../api/PB/MaterialSvc'
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
      MaterialSvc.GetTheData(this.id).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.entity = resJson.Data
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    handlerLMReport(material) {
      this.$router.push({ path: '/Report/LocalMaterialReport', query: { materialId: material.Id } })
    }
  }
}
</script>

<style>
</style>