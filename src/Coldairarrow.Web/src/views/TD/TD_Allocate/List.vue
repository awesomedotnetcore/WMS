<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button v-if="hasPerm('TD_Allocate.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('TD_Allocate.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
      <a-divider type="vertical" />
      <a-radio-group v-model="queryParam.Status" button-style="solid" @change="getDataList">
        <a-radio-button :value="null">全部</a-radio-button>
        <a-radio-button :value="0">待审核</a-radio-button>
        <a-radio-button :value="1">审核通过</a-radio-button>
        <a-radio-button :value="2">审核失败</a-radio-button>
      </a-radio-group>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="AllocateType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Code" placeholder="调拨/关联单号" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <storage-select v-model="queryParam.ToStorId" placeholder="调拨仓库"></storage-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-range-picker @change="onAllocateTimeChange" />
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
      <template slot="Type" slot-scope="text">
        <enum-name code="AllocateType" :value="text"></enum-name>
      </template>
      <template slot="Status" slot-scope="text, record">
        <a-tag v-if="record.Status===0" color="blue">待审核</a-tag>
        <a-tag v-else-if="record.Status===1" color="green">审核通过</a-tag>
        <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
        <a-tag v-else color="blue">待审核</a-tag>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleShow(record.Id)">查看</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Allocate.Edit')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_Allocate.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="record.Status===0 && hasPerm('TD_Allocate.Delete')" type="vertical" />
          <a v-if="record.Status===0 && hasPerm('TD_Allocate.Delete')" @click="handleDelete([record.Id])">删除</a>          
          <!-- <a v-if="record.Status===1 || hasPerm('TD_Allocate.Auditing')" @click="handleShow(record.Id)">{{ record.Status === 0?'审核':'查看' }}</a> -->
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :disabled="disabled" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import moment from 'moment'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EditForm from './EditForm'
import StorageSelect from '../../../components/Storage/AllStorageSelect'

const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '调拨单号', dataIndex: 'Code' },
  { title: '调拨时间', dataIndex: 'AllocateTime', customRender: filterDate },
  { title: '调拨类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' } },
  { title: '调拨仓库', dataIndex: 'ToStorage.Name' },
  { title: '调拨数量', dataIndex: 'AllocateNum' },
  { title: '总金额', dataIndex: 'Amount' },
  { title: '状态', dataIndex: 'Status', scopedSlots: { customRender: 'Status' } },
  { title: '制单人', dataIndex: 'CreateUser.RealName' },
  { title: '审核人', dataIndex: 'AuditUser.RealName' },
  { title: '审核时间', dataIndex: 'AuditeTime', customRender: filterDate },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    EnumSelect,
    StorageSelect
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
        .post('/TD/TD_Allocate/GetDataList', {
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
    onAllocateTimeChange(dates, dateStrings) {
      this.queryParam.AllocateTimeStart = dateStrings[0]
      this.queryParam.AllocateTimeEnd = dateStrings[1]
    },
    hanldleAdd() {
      this.disabled = false
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.disabled = false
      this.$refs.editForm.openForm(id)
    },
    handleShow(id) {
      this.disabled = true
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_Allocate/DeleteData', ids).then(resJson => {
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