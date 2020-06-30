<template>
  <a-drawer :title="title" placement="right" :closable="true" :maskClosable="false" @close="onDrawerClose" :visible="visible" :width="1224" :getContainer="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>
    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="IsEnable" slot-scope="text, record">
        <template>
          <a-switch checked-children="启用" un-checked-children="停用" @click="handleModifyEnable(record.Id)" v-model="record.IsEnable" />
        </template>
      </span>
      <span slot="IsDefault" slot-scope="text, record">
        <template>
          <a-switch checked-children="开" un-checked-children="关" @click="handleModifyDefault(record.Id)" v-model="record.IsDefault" />
        </template>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'

const columns = [
  { title: '编号', dataIndex: 'Code'},
  { title: '名称', dataIndex: 'Name'},
  { title: '地址', dataIndex: 'Address'},
  { title: '备注', dataIndex: 'Remarks' },
  { title: '状态', dataIndex: 'IsEnable', scopedSlots: { customRender: 'IsEnable' } },
  { title: '默认', dataIndex: 'IsDefault',  scopedSlots: { customRender: 'IsDefault' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
  },
  mounted() {
    this.getDataList()
  },
  data() {
    return {
      visible: false,
      title: '',
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
        .post('/PB/PB_Address/QueryDataList', {
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
      this.$refs.editForm.openForm(null, this.queryParam.CusId, this.queryParam.SupId)
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, this.queryParam.CusId, this.queryParam.SupId)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Address/DeleteData', ids).then(resJson => {
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
    handleModifyDefault(id) {
      this.$http
        .post('/PB/PB_Address/ModifyDefault?id=' + id)
        .then(resJson => {
          this.getDataList()
        })
    },
    handleModifyEnable(id) {
      this.$http
        .post('/PB/PB_Address/ModifyEnable?id=' + id)
        .then(resJson => {
          this.getDataList()
        })
    },
    openDrawer(isCus, id, name) {
      this.title = name + '地址设置'
      if (isCus) {
        this.queryParam.CusId = id
        this.queryParam.SupId = ''
      } else {
        this.queryParam.SupId = id
        this.queryParam.CusId = ''
      }

      this.visible = true
      this.getDataList()
    },
    onDrawerClose() {
      this.visible = false
    }
  }
}
</script>