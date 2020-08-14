<template>
  <a-card title="计划表">
    <a-button slot="extra" shape="circle" icon="search" @click="e=>{this.visible=true}" />
    <a-drawer title="入库查询" :closable="true" :height="490" :visible="visible" placement="top" @close="e=>{this.visible=false}">
      <a-form-model layout="horizontal" :model="queryData.Search">
        <a-form-model-item>
          <a-input v-model="queryData.Search.Code" placeholder="计划编号"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="计划开始日期" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.StartDate" />
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="计划结束日期" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.FinishDate" />
        </a-form-model-item>
        <a-form-model-item>
          <a-row>
            <a-col :span="12">
              <a-button block type="primary" @click="()=>{this.queryData.PageIndex=1;this.getList();this.visible=false}">查询</a-button>
            </a-col>
            <a-col :span="12">
              <a-button block @click="()=>{this.queryData.Search = { Status: null};this.getList();this.visible=false}">重置</a-button>
            </a-col>
          </a-row>
        </a-form-model-item>
      </a-form-model>
    </a-drawer>      
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
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
import PlanSvc from '../../api/PB/PlanSvc'
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
        Search: { Status: null }
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
      PlanSvc.GetDataList(this.queryData).then(resJson => {
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
      this.$router.push({ path: `/PB/PlanDetail/${item.Id}` })
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