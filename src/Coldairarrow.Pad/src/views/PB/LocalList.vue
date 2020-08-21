<template>
  <a-card title="货位管理">
    <a-input-search slot="extra" placeholder="名称/编码" v-model="queryData.Search.Keyword" :allowClear="true" @search="()=>{this.queryData.PageIndex=1;this.getList()}" />
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">{{ item.PB_Storage.Name }}</span>
            <span class="spanTag">{{ item.Code }}</span>
          </div>
          <div slot="description">
            <span class="spanTag" v-if="item.AreaId">货区:{{ item.PB_StorArea.Name }}</span>
            <span class="spanTag" v-if="item.LanewayId">巷道:{{ item.PB_Laneway.Name }}</span>
            <span class="spanTag" v-if="item.RackId">货架:{{ item.PB_Rack.Name }}</span>
          </div>
        </a-list-item-meta>
        <a-button type="link" slot="actions" @click="handlerLMReport(item)">物料</a-button>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import StorageSvc from '../../api/PB/StorageSvc'
import LocalSvc from '../../api/PB/LocalSvc'
export default {
  components: {
  },
  data() {
    return {
      loading: false,
      pagination: { current: 1, pageSize: 10, size: 'small', total: 0, onChange: this.handlerChange },
      queryData: {
        PageIndex: 1, PageRows: 10, SortField: 'Code', SortType: 'asc',
        Search: { Keyword: null, StorId: null }
      },
      listData: [],
      storage: {}
    }
  },
  mounted() {
    this.getStorage()
  },
  methods: {
    moment,
    getStorage() {
      StorageSvc.GetCurStorage().then(jsonRes => {
        this.storage = jsonRes.Data
        this.queryData.Search.StorId = this.storage.Id
        this.getList()
      })
    },
    getList() {
      this.loading = true
      LocalSvc.GetDataList(this.queryData).then(resJson => {
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
    },
    handlerLMReport(local) {
      this.$router.push({ path: '/Report/LocalMaterialReport', query: { localId: local.Id } })
    }
  }
}
</script>

<style>
.spanTag {
  margin: 0 5px 0 5px;
}
</style>