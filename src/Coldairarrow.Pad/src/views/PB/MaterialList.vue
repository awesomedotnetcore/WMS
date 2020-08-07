<template>
  <a-card title="物料管理">
    <a-input-search slot="extra" placeholder="物料编码/名称" v-model="queryData.Search.Keyword" :allowClear="true" @search="()=>{this.queryData.PageIndex=1;this.getList()}" />
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">{{ item.Name }}</span>
            <span class="spanTag">{{ item.Code }}</span>
          </div>
          <div slot="description">
            <span class="spanTag" v-if="item.MaterialTypeId"><a-icon type="caret-right" />{{ item.MaterialType.Name }}</span>
            <span class="spanTag" v-if="item.BarCode"><a-icon type="barcode" />{{ item.BarCode }}</span>
            <span class="spanTag" v-if="item.SimpleName"><a-icon type="info-circle" />{{ item.SimpleName }}</span>
            <span class="spanTag" v-if="item.Spec"><a-icon type="double-right" />{{ item.Spec }}</span>
          </div>
        </a-list-item-meta>
        <a-button type="link" slot="actions" @click="handlerShow(item)">查看</a-button>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import MaterialSvc from '../../api/PB/MaterialSvc'
export default {
  components: {
  },
  data() {
    return {
      loading: false,
      pagination: { current: 1, pageSize: 10, size: 'small', total: 0, onChange: this.handlerChange },
      queryData: {
        PageIndex: 1, PageRows: 10, SortField: 'Id', SortType: 'desc',
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
      MaterialSvc.GetDataList(this.queryData).then(resJson => {
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
    handlerShow(item) {
      // this.$router.push({ path: `/InStorage/Detail/${item.Id}` })
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
.spanTag{
  margin: 0 5px 0 5px;
}
</style>