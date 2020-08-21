<template>
  <a-card title="托盘管理">
    <a-input-search slot="extra" placeholder="托盘编码" v-model="queryData.Search.Keyword" :allowClear="true" @search="()=>{this.queryData.PageIndex=1;this.getList()}" />
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">名称:{{ item.Name }}</span>
            <span class="spanTag">编码:{{ item.Code }}</span>
          </div>
          <div slot="description">
            <span class="spanTag" v-if="item.TrayTypeId">类型:{{ item.PB_TrayType.Name }}</span>
            <span class="spanTag" v-if="item.LocalId">货位:{{ item.PB_Location.Code }}</span>
          </div>
        </a-list-item-meta>
        <a-button type="link" slot="actions" @click="handlerLMReport(item)">物料</a-button>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import TraySvc from '../../api/PB/TraySvc'
export default {
  components: {
  },
  data() {
    return {
      loading: false,
      pagination: { current: 1, pageSize: 10, size: 'small', total: 0, onChange: this.handlerChange },
      queryData: {
        PageIndex: 1, PageRows: 10, SortField: 'Code', SortType: 'asc',
        Search: { Keyword: null }
      },
      listData: []
    }
  },
  mounted() {
    this.getList()
  },
  methods: {
    moment,
    getList() {
      this.loading = true
      TraySvc.GetDataList(this.queryData).then(resJson => {
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
    handlerLMReport(tray) {
      this.$router.push({ path: '/Report/LocalMaterialReport', query: { trayId: tray.Id } })
    }
  }
}
</script>

<style>
.spanTag{
  margin: 0 5px 0 5px;
}
</style>