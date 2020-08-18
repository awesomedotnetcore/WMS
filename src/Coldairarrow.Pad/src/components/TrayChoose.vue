<template>
  <a-modal :visible="visible" :footer="null" @cancel="e=>{this.visible=false}">
    <a-row slot="title">
      <a-col :span="6">托盘选择</a-col>
      <a-col :span="16">
        <a-input-search placeholder="编码/名称" size="small" v-model="queryData.Search.Keyword" :allowClear="true" @search="()=>{this.queryData.PageIndex=1;this.getList()}" />
      </a-col>
    </a-row>
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
        <a-button type="link" slot="actions" @click="handlerChoose(item)">选择</a-button>
      </a-list-item>
    </a-list>
  </a-modal>
</template>

<script>
import moment from 'moment'
import TraySvc from '../api/PB/TraySvc'
export default {
  components: {
  },
  data() {
    return {
      visible: false,
      loading: false,
      pagination: { current: 1, pageSize: 5, size: 'small', total: 0, onChange: this.handlerChange },
      queryData: { PageIndex: 1, PageRows: 5, SortField: "Code", SortType: "asc", Search: { Keyword: null } },
      listData: []
    }
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
    openChoose() {
      this.visible = true
      this.getList()
    },
    handlerChoose(item) {
      this.$emit('select', item)
      this.visible = false
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
.spanTag {
  margin: 0 5px 0 5px;
}
</style>