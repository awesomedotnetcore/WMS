<template>
  <a-drawer title="字典值" placement="right" :closable="true" :maskClosable="false" @close="onDrawerClose" :visible="visible" :width="1024" :getContainer="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="字典值/名称/编码" />
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
          <a v-if="!record.IsSystem" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="!record.IsSystem" type="vertical" />
          <a v-if="!record.IsSystem" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this" :enumObj="enumData"></edit-form>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'
const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}
const columns = [
  { title: '字典名称', dataIndex: 'Name', width: '20%' },
  { title: '字典值', dataIndex: 'Value', width: '20%' },
  { title: '字典编码', dataIndex: 'Code', width: '20%' },
  { title: '系统必须', dataIndex: 'IsSystem', customRender: filterYesOrNo, width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
  },
  mounted() {
  },
  data() {
    return {
      enumData: {},
      visible: false,
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
        .post('/Base/Base_EnumItem/GetDataList', {
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
        if (!val.IsSystem) ids.push(val.Id)
      })
      this.selectedRowKeys = ids
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.editForm.openForm(null,"新增")
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id,"编辑")
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/Base/Base_EnumItem/DeleteData', ids).then(resJson => {
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
    openDrawer(record) {
      this.filters.EnumId = record.Id
      this.enumData = record
      this.visible = true
      this.getDataList()
    },
    onDrawerClose() {
      this.visible = false
    }
  }
}
</script>