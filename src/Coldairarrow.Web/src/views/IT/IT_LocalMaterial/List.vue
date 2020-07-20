<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="5">
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.LocalName" placeholder="货位" />
            </a-form-item>
          </a-col>
          <a-col v-if="storage.IsTray" :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.TrayName" placeholder="托盘" />
            </a-form-item>
          </a-col>
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料" />
            </a-form-item>
          </a-col>
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="批次/条码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" @change="handleTableChange" :loading="loading" :bordered="true" size="small">
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">{{ record.Location.Code }}</template>
            {{ record.Location.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.TrayId">
          <a-tooltip>
            <template slot="title">{{ record.Tray.Code }}</template>
            {{ record.Tray.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.ZoneId">
          <a-tooltip>
            <template slot="title">{{ record.TrayZone.Code }}</template>
            {{ record.TrayZone.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
      <a-tooltip slot="NameCode" slot-scope="text" placement="right">
        <template slot="title">{{ text.Code }}</template>
        {{ text.Name }}
      </a-tooltip>
    </a-table>
  </a-card>
</template>

<script>
const columns = [
  { title: '货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'NameCode' } },
  { title: '单位', dataIndex: 'Measure.Name' },
  { title: '批次号', dataIndex: 'BatchNo' },
  { title: '条码', dataIndex: 'BarCode' },
  { title: '数量', dataIndex: 'Num' }
]

export default {
  components: {
  },
  mounted() {
    this.getDataList()
    this.getCurStorage()
  },
  data() {
    return {
      storage: {},
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'Id', order: 'desc' },
      loading: false,
      columns,
      queryParam: {},
      
    }
  },
  methods: {
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
        })
    },
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getDataList() {      
      this.selectedRowKeys = []

      this.loading = true
      this.$http
        .post('/IT/IT_LocalMaterial/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then(resJson => {
          this.loading = false
          this.data = resJson.Data
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    }
  }
}
</script>