<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" v-if="hasPerm('TD_Receiving.Add')" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" v-if="hasPerm('TD_Receiving.Delete')" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
      <a-divider type="vertical" />
      <a-radio-group v-model="queryParam.Status" button-style="solid" @change="getDataList">
        <a-radio-button :value="null">全部</a-radio-button>
        <a-radio-button :value="0">编制中</a-radio-button>
        <a-radio-button :value="1">已确认</a-radio-button>
        <a-radio-button :value="3">审核通过</a-radio-button>
        <a-radio-button :value="4">审核失败</a-radio-button>
        <a-radio-button :value="5">部分入库</a-radio-button>
        <a-radio-button :value="6">全部入库</a-radio-button>
      </a-radio-group>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="单号/供应商" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="ReceiveType" v-model="queryParam.InType"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-range-picker @change="onOrderTimeChange" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = { Status: null})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="ReceiveType" slot-scope="text">
        <enum-name code="ReceiveType" :value="text"></enum-name>
      </template>
      <template slot="Status" slot-scope="text, record">
        <a-tag v-if="record.Status===0" color="blue">编制中</a-tag>
        <a-tag v-else-if="record.Status===1" color="blue">已确认</a-tag>
        <a-tag v-else-if="record.Status===3" color="green">审核通过</a-tag>
        <a-tag v-else-if="record.Status===4" color="red">审核失败</a-tag>
        <a-tag v-else-if="record.Status===5" color="green">部分入库</a-tag>
        <a-tag v-else-if="record.Status===6" color="green">全部入库</a-tag>
        <a-tag v-else color="blue">编制中</a-tag>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="record.Status===0 && hasPerm('TD_Receiving.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Receiving.Edit')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_Receiving.Delete')" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Receiving.Delete')" type="vertical" />
          <a v-if="record.Status===0" @click="handleApproval(record.Id)">确认</a>
          <a-divider v-if="record.Status===0" type="vertical" />
          <a v-if="(record.Status===3 || record.Status===5) && hasPerm('TD_Receiving.InStorage')" @click="handleInStorage(record.Id)">入库</a>
          <a-divider v-if="(record.Status===3 || record.Status===5) && hasPerm('TD_Receiving.InStorage')" type="vertical" />
          <a v-if="record.Status>0" @click="handleApproval(record.Id)">{{ record.Status>=3?'查看':'审批' }}</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this"></edit-form>
    <in-storage ref="inStorage" :parentObj="this"></in-storage>
  </a-card>
</template>

<script>
import moment from 'moment'
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import InStorage from '../TD_InStorage/EditForm'
const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '收货单号', dataIndex: 'Code' },
  { title: '订单日期', dataIndex: 'OrderTime', customRender: filterDate },
  { title: '收货日期', dataIndex: 'RecTime', customRender: filterDate },
  { title: '收货类型', dataIndex: 'Type', scopedSlots: { customRender: 'ReceiveType' } },
  { title: '关联单号', dataIndex: 'RefCode' },
  { title: '状态', dataIndex: 'Status', scopedSlots: { customRender: 'Status' } },
  { title: '供应商', dataIndex: 'Supplier.Name' },
  { title: '收货数量', dataIndex: 'TotalNum' },
  { title: '入库数量', dataIndex: 'InNum' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    EnumSelect,
    InStorage
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
      sorter: { field: 'CreateTime', order: 'desc' },
      loading: false,
      columns,
      queryParam: { Status: null },
      selectedRowKeys: [],
      disabled: false
    }
  },
  methods: {
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
        .post('/TD/TD_Receiving/GetDataList', {
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
    },
    onOrderTimeChange(dates, dateStrings) {
      this.queryParam.OrderTimeStart = dateStrings[0]
      this.queryParam.OrderTimeEnd = dateStrings[1]
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      var ids = []
      selectedRows.forEach((val, index, arr) => {
        if (val.Status === 0) ids.push(val.Id)
      })
      this.selectedRowKeys = ids
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.disabled = false
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.disabled = false
      this.$refs.editForm.openForm(id)
    },
    handleApproval(id) {
      this.disabled = true
      this.$refs.editForm.openForm(id)
    },
    handleInStorage(id) {
      this.$refs.inStorage.openReceivingForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_Receiving/DeleteData', ids).then(resJson => {
              resolve()

              if (resJson.Success) {
                thisObj.$message.success('操作成功!')

                thisObj.getDataList()
              } else {
                thisObj.$message.error(resJson.Msg)
              }
            })
          })
        }
      })
    }
  }
}
</script>