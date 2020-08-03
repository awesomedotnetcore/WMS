<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="10">
        <a-col :md="20" :sm="24">
        <a-button v-if="hasPerm('PB_Tray.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
        <a-button v-if="hasPerm('PB_Tray.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
        <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>  
        </a-col>
      <a-button v-if="hasPerm('PB_Tray.Leading')" type="primary" @click="hanldleLeading()">导入托盘</a-button>
     </a-row>
    </div>
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="托盘名称/编码" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.TypeName" placeholder="类型名称/编码" />
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
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="hasPerm('PB_Tray.Status')" @click="changeStatus(record.Id, record.Status)">{{ record.Status === 0?'启用':'停用' }}</a>
          <a-divider v-if="hasPerm('PB_Tray.Edit')" type="vertical" />
          <a v-if="hasPerm('PB_Tray.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('PB_Tray.Delete')" type="vertical" />
          <a v-if="hasPerm('PB_Tray.Delete')" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <leading-show ref="leadingShow" :parentObj="this"></leading-show>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import LeadingShow from '../../../components/PB/LeadingShow'
                                                
const filterYesOrNo = (value, row, index) => {
  if (value) return '启用'
  else return '停用'
}
const columns = [
  { title: '托盘号', dataIndex: 'Code'},
  { title: '托盘名称', dataIndex: 'Name' },
  { title: '托盘类型', dataIndex: 'PB_TrayType.Name' },
  { title: '货位', dataIndex: 'PB_Location.Name' },
  { title: '启用日期', dataIndex: 'StartTime' },
  { title: '托盘状态', dataIndex: 'Status', customRender: filterYesOrNo },  
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
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
        .post('/PB/PB_Tray/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新增')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑')
    },
    hanldleLeading() {
      this.$refs.leadingShow.openForm(null, '导入托盘','/PB/PB_Tray/Import','/PB/PB_Tray/ExportToExcel')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Tray/DeleteData', ids).then(resJson => {
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
    changeStatus(Id, Status) {
      var thisObj = this
      if (Status == 1) {
        this.$http.post('/PB/PB_Tray/DisableTheData', { Id: Id }).then(resJson => {
          if (resJson.Success) {
            thisObj.$message.success('操作成功!')

            thisObj.getDataList()
          } else {
            thisObj.$message.error(resJson.Msg)
          }
        })
      } else {
        this.$http.post('/PB/PB_Tray/EnableTheData', { Id: Id }).then(resJson => {
          if (resJson.Success) {
            thisObj.$message.success('操作成功!')

            thisObj.getDataList()
          } else {
            thisObj.$message.error(resJson.Msg)
          }
        })
      }
    }
  }
}
</script>