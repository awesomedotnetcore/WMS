<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="出库/关联单号" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="OutStorageType" v-model="queryParam.OutType"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-range-picker @change="onOutStorTimeChange" />
            </a-form-item>
          </a-col>
          <a-col :md="2" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.LocalName" placeholder="货位" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.MaterialName" placeholder="物料/批次/条码" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :bordered="true" size="small">
      <template slot="OutType" slot-scope="text">
        <enum-name code="OutStorageType" :value="text"></enum-name>
      </template>
      <a-breadcrumb slot="Location" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">货位:{{ record.Location.Code }}</template>
            {{ record.Location.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.TrayId">
          <a-tooltip>
            <template slot="title">托盘:{{ record.Tray.Code }}</template>
            {{ record.Tray.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.ZoneId">
          <a-tooltip>
            <template slot="title">分区:{{ record.TrayZone.Code }}</template>
            {{ record.TrayZone.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
      <a-breadcrumb slot="Material" slot-scope="text, record">
        <a-breadcrumb-item>
          <a-tooltip>
            <template slot="title">物料:{{ record.Material.Code }}</template>
            {{ record.Material.Name }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.BatchNo">
          <a-tooltip>
            <template slot="title">批次</template>
            {{ record.BatchNo }}
          </a-tooltip>
        </a-breadcrumb-item>
        <a-breadcrumb-item v-if="record.BarCode">
          <a-tooltip>
            <template slot="title">条码</template>
            {{ record.BarCode }}
          </a-tooltip>
        </a-breadcrumb-item>
      </a-breadcrumb>
    </a-table>
  </a-card>
</template>

<script>
import moment from 'moment'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '出库单号', dataIndex: 'OutStorage.Code' },
  { title: '出库类型', dataIndex: 'OutStorage.OutType', scopedSlots: { customRender: 'OutType' } },
  { title: '出库时间', dataIndex: 'OutStorage.OutTime', customRender: filterDate },
  { title: '出库货位', dataIndex: 'Location', scopedSlots: { customRender: 'Location' } },
  { title: '物料', dataIndex: 'Material', scopedSlots: { customRender: 'Material' } },
  { title: '出库数量', dataIndex: 'OutNum' },
  { title: '单价', dataIndex: 'Price' },
  { title: '总额', dataIndex: 'TotalAmt' }
]

export default {
  components: {
    EnumName,
    EnumSelect
  },
  mounted() {
    this.getDataList()
  },
  data() {
    return {
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
      queryParam: {}
    }
  },
  methods: {
    moment,
    onOutStorTimeChange(dates, dateStrings) {
      this.queryParam.OutTimeStart = dateStrings[0]
      this.queryParam.OutTimeEnd = dateStrings[1]
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
        .post('/TD/TD_OutStorDetail/GetDataList', {
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