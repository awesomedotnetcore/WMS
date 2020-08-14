<template>
  <a-card title="收货管理">
    <a-button slot="extra" shape="circle" icon="search" @click="e=>{this.visible=true}" />
    <a-drawer title="收货查询" :closable="true" :height="490" :visible="visible" placement="top" @close="e=>{this.visible=false}">
      <a-form-model layout="horizontal" :model="queryData.Search">
        <a-form-model-item>
          <a-input v-model="queryData.Search.Keyword" placeholder="收货单号"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <enum-select code="ReceiveType" v-model="queryData.Search.Type"></enum-select>
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="收货日期(开始)" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.OrderTimeStart" />
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="收货日期(结束)" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.OrderTimeEnd" />
        </a-form-model-item>
        <a-form-model-item>
          <a-select v-model="queryData.Search.Status" :style="{width:'100%'}">
            <a-select-option :value="null">全部</a-select-option>
            <a-select-option :value="0">编制中</a-select-option>
            <a-select-option :value="1">已确认</a-select-option>
            <a-select-option :value="3">审核通过</a-select-option>
            <a-select-option :value="4">审核失败</a-select-option>
            <a-select-option :value="5">部分入库</a-select-option>
            <a-select-option :value="6">全部入库</a-select-option>
          </a-select>
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
    <div>
      <a-select v-model="queryData.Search.Status" @select="()=>{this.queryData.PageIndex=1;this.getList()}" :style="{width:'100%'}">
        <a-select-option :value="null">全部</a-select-option>
        <a-select-option :value="0">编制中</a-select-option>
        <a-select-option :value="1">已确认</a-select-option>
        <a-select-option :value="3">审核通过</a-select-option>
        <a-select-option :value="4">审核失败</a-select-option>
        <a-select-option :value="5">部分入库</a-select-option>
        <a-select-option :value="6">全部入库</a-select-option>
      </a-select>
    </div>
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span :style="{marginRight:'10px'}">{{ item.Code }}</span>
            <a-badge v-if="item.Status==0" status="processing" text="编制中" />
            <a-badge v-if="item.Status==1" status="success" text="已确认" />
            <a-badge v-if="item.Status==3" status="success" text="审核通过" />
            <a-badge v-if="item.Status==4" status="error" text="审核失败" />
            <a-badge v-if="item.Status==5" status="success" text="部分入库" />
            <a-badge v-if="item.Status==6" status="success" text="全部入库" />
          </div>
          <span slot="description">时间：{{ moment(item.RecTime).format('YY/M/D H:m') }} 数量：{{ item.TotalNum }}</span>
        </a-list-item-meta>
        <a-button type="link" slot="actions" @click="handlerShow(item)">查看</a-button>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import ReceiveSvc from '../../api/TD/ReceiveSvc'
import EnumSelect from '../../components/BaseEnumSelect'
export default {
  components: {
    EnumSelect
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
      ReceiveSvc.GetDataList(this.queryData).then(resJson => {
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
      this.$router.push({ path: `/InStorage/ReceiveDetail/${item.Id}` })
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