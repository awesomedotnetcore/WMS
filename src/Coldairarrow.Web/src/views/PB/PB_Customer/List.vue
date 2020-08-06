<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="10">
        <a-col :md="20" :sm="24">
          <a-button v-if="hasPerm('PB_Customer.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
          <a-button v-if="hasPerm('PB_Customer.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
          <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
          <a-button v-if="hasPerm('PB_Customer.Leading')" type="primary" @click="hanldleLeading()">导入客户</a-button>
      </a-row>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="CustomerType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.KeyWord" placeholder="客户编号或名称" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="Type" slot-scope="text">
        <enum-name code="CustomerType" :value="text"></enum-name>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="hasPerm('PB_Customer.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('PB_Customer.Delete')" type="vertical" />
          <a v-if="hasPerm('PB_Customer.Delete')" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="hasPerm('PB_Customer.AddressList')" type="vertical" />
          <a v-if="hasPerm('PB_Customer.AddressList')" @click="openAddressList(record.Id,record.Name)">地址</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <address-list ref="addressList" :parentObj="this"></address-list>
    <leading-show ref="leadingShow" :parentObj="this" leading="/PB/PB_Customer/Import" templet="/PB/PB_Customer/ExportToExcel"></leading-show>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import AddressList from '../../PB/PB_Address/List'
import LeadingShow from '../../../components/PB/LeadingShow'

const columns = [
  { title: '客户编号', dataIndex: 'Code'},
  { title: '客户名称', dataIndex: 'Name'},
  { title: '客户类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' } },
  { title: '电话', dataIndex: 'Phone' },
  { title: '传真', dataIndex: 'Fax' },
  { title: 'Email', dataIndex: 'Email' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    EnumSelect,
    AddressList,
    LeadingShow
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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: []
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
        .post('/PB/PB_Customer/QueryDataList', {
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.editForm.openForm(null,"新增客户")
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id,"编辑客户")
    },
    hanldleLeading() {
      this.$refs.leadingShow.openForm(null, '导入客户信息')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Customer/DeleteData', ids).then(resJson => {
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
    },
    openAddressList(id, name) {
      this.$refs.addressList.openDrawer(true, id, name)
    }
  }
}
</script>