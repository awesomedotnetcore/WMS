<template>
  <a-card title="库存报表">
    <a-button slot="extra" shape="circle" icon="search" @click="e=>{this.visible=true}" />
    <a-drawer title="库存查询" :closable="true" :height="430" :visible="visible" placement="top" @close="e=>{this.visible=false}">
      <a-form-model layout="horizontal" :model="queryData.Search">
        <a-form-model-item>
          <a-input v-model="queryData.Search.LocalName" placeholder="货位名称/编码"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <a-input v-model="queryData.Search.TrayName" placeholder="托盘名称/编码"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <a-input v-model="queryData.Search.MaterialName" placeholder="物料名称/编码/条码"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <a-input v-model="queryData.Search.Code" placeholder="批次/唯一码"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <a-row>
            <a-col :span="12">
              <a-button block type="primary" @click="()=>{this.queryData.PageIndex=1;this.getList();this.visible=false}">查询</a-button>
            </a-col>
            <a-col :span="12">
              <a-button block @click="()=>{this.queryData.Search = {};this.getList();this.visible=false}">重置</a-button>
            </a-col>
          </a-row>
        </a-form-model-item>
      </a-form-model>
    </a-drawer>
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="vertical" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span :style="{marginRight:'10px'}">{{ item.Location.Code }}</span>
            <span :style="{marginRight:'10px'}">{{ item.Material.Name }}</span>
            <span>{{ item.Num }}{{ item.Measure.Name }}</span>
          </div>
          <span slot="description">
            <span :style="{marginRight:'10px'}">物料:{{ item.Material.Code }}</span>
            <span :style="{marginRight:'10px'}" v-if="item.Material.BarCode">条码:{{ item.Material.BarCode }}</span>
            <span :style="{marginRight:'10px'}" v-if="item.TrayId">托盘:{{ item.Tray.Code }}</span>
            <span v-if="item.BatchNo">批次:{{ item.BatchNo }}</span>
          </span>
        </a-list-item-meta>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import LocalMaterialSvc from '../../api/Report/LocalMaterialSvc'
export default {
  components: {
  },
  data() {
    return {
      visible: false,
      loading: false,
      pagination: { current: 1, pageSize: 10, size: 'small', total: 0, onChange: this.handlerChange },
      queryData: {
        PageIndex: 1, PageRows: 10, SortField: 'Id', SortType: 'desc',
        Search: {}
      },
      listData: []
    }
  },
  mounted() {
    if (this.$route.query.localId) {
      this.queryData.Search.LocalId = this.$route.query.localId
    }
    if (this.$route.query.trayId) {
      this.queryData.Search.TrayId = this.$route.query.trayId
    }
    if (this.$route.query.materialId) {
      this.queryData.Search.MaterialId = this.$route.query.materialId
    }
    this.getList()
  },
  methods: {
    moment,
    getList() {
      this.loading = true
      LocalMaterialSvc.GetDataList(this.queryData).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.listData = resJson.Data
          this.pagination.current = this.queryData.PageIndex
          this.pagination.total = resJson.Total
        } else {
          this.$message.error(resJson.Msg)
        }
      });
    },
    handlerChange(page, size) {
      this.queryData.PageIndex = page
      this.queryData.PageRows = size
      this.getList()
    }
  }
}
</script>

<style>
</style>