<template>
  <a-card title="出库管理">
    <a-button slot="extra" shape="circle" icon="search" @click="e=>{this.visible=true}" />
    <a-drawer title="出库查询" :closable="true" :height="490" :visible="visible" placement="top" @close="e=>{this.visible=false}">
      <a-form-model layout="horizontal" :model="queryData.Search">
        <a-form-model-item>
          <a-input v-model="queryData.Search.Code" placeholder="出库单号"></a-input>
        </a-form-model-item>
        <a-form-model-item>
          <enum-select code="OutStorageType" v-model="queryData.Search.OutType"></enum-select>
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="出库日期(开始)" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.OutStorTimeStart" />
        </a-form-model-item>
        <a-form-model-item>
          <a-date-picker placeholder="出库日期(结束)" :style="{width:'100%'}" :inputReadOnly="true" v-model="queryData.Search.OutStorTimeEnd" />
        </a-form-model-item>
        <a-form-model-item>
          <a-radio-group v-model="queryData.Search.Status" button-style="solid">
            <a-radio-button :value="null">全部</a-radio-button>
            <a-radio-button :value="0">出库中</a-radio-button>
            <a-radio-button :value="1">完成</a-radio-button>
            <a-radio-button :value="2">失败</a-radio-button>
          </a-radio-group>
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
    <div :style="{textAlign:'center'}">
      <a-radio-group v-model="queryData.Search.Status" button-style="solid" @change="e=>{this.queryData.PageIndex=1;this.getList()}">
        <a-radio-button :value="null">全部</a-radio-button>
        <a-radio-button :value="0">出库中</a-radio-button>
        <a-radio-button :value="1">完成</a-radio-button>
        <a-radio-button :value="2">失败</a-radio-button>
      </a-radio-group>
    </div>
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading" :pagination="pagination">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span :style="{marginRight:'10px'}">{{ item.Code }}</span>
            <a-badge v-if="item.Status==0" status="processing" text="出库中" />
            <a-badge v-if="item.Status==1" status="success" text="完成" />
            <a-badge v-if="item.Status==2" status="error" text="失败" />
          </div>
          <span slot="description">时间：{{ moment(item.OutTime).format('YY/M/D H:m') }} 数量：{{ item.OutNum }}</span>
        </a-list-item-meta>
        <a-button type="link" slot="actions" @click="handlerShow(item)">查看</a-button>
      </a-list-item>
    </a-list>
  </a-card>
</template>

<script>
import moment from 'moment'
import OutStorageSvc from '../../api/TD/OutStorageSvc'
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
      OutStorageSvc.GetDataList(this.queryData).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          // this.listData = this.listData.concat(resJson.Data)
          this.listData = resJson.Data
          this.pagination.current = this.queryData.PageIndex
          this.pagination.total = resJson.Total
        } else {
          this.$message.error(resJson.Msg)
        }
      });
    },
    handlerShow(item) {
      this.$router.push({ path: `/OutStorage/Detail/${item.Id}` })
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