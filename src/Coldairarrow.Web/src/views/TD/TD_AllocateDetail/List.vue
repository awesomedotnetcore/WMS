<template>
  <a-drawer
    title="调拨明细"
    placement="right"
    :closable="true"
    @close="onDrawerClose"
    :visible="visible"
    width="90%"
    :getContainer="false"
  >
    <a-card :bordered="false">
      <div class="table-operator">
        <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
        <a-button
          type="primary"
          icon="minus"
          @click="handleDelete(selectedRowKeys)"
          :disabled="!hasSelected()"
          :loading="loading"
        >删除</a-button>
        <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        <a-button v-if="this.state == 0" type="primary" icon="check" @click="approveData()">审批</a-button>
        <a-button v-if="this.state == 0" type="primary" icon="close" @click="rejectData()">驳回</a-button>
      </div>

      <a-table
        ref="table"
        :columns="columns"
        :rowKey="row => row.Id"
        :dataSource="data"
        :pagination="pagination"
        :loading="loading"
        @change="handleTableChange"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
        :bordered="true"
        size="small"
      >
        <span slot="action" slot-scope="text, record">
          <template>
            <a @click="handleEdit(record.Id)">编辑</a>
            <a-divider type="vertical" />
            <a @click="handleDelete([record.Id])">删除</a>
          </template>
        </span>
      </a-table>

      <edit-form ref="editForm" :parentObj="this"></edit-form>
    </a-card>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'

const columnsApproved = [
  { title: '原货位', dataIndex: 'Src_Location.Name', width: '10%' },
  { title: '原托盘', dataIndex: 'Src_Tray.Name', width: '10%' },
  { title: '原托盘分区', dataIndex: 'Src_TrayZone.Name', width: '10%' },
  { title: '目标仓库', dataIndex: 'Tar_Storage.Name', width: '10%' },
  { title: '目标货位', dataIndex: 'Tar_Location.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '物料', dataIndex: 'PB_Material.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '调拨数量', dataIndex: 'LocalNum', width: '10%' }
]

const columns = [
  { title: '原货位', dataIndex: 'Src_Location.Name', width: '10%' },
  { title: '原托盘', dataIndex: 'Src_Tray.Name', width: '10%' },
  { title: '原托盘分区', dataIndex: 'Src_TrayZone.Name', width: '10%' },
  { title: '目标仓库', dataIndex: 'Tar_Storage.Name', width: '10%' },
  { title: '目标货位', dataIndex: 'Tar_Location.Name', width: '10%' },
  { title: '条码', dataIndex: 'BarCode', width: '10%' },
  { title: '物料', dataIndex: 'PB_Material.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '调拨数量', dataIndex: 'LocalNum', width: '10%' },
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
      columnsApproved,
      queryParam: {},
      selectedRowKeys: [],
      visible: false,
      allocateId: null,
      state: null
    }
  },
  computed: {
    computedColumns() {
      if (this.state == 0) {
        return columns
      } else {
        return columnsApproved
      }
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
        .post('/TD/TD_AllocateDetail/GetDataList', {
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
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_AllocateDetail/DeleteData', ids).then(resJson => {
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
    openDrawer(allocateId, state) {
      this.allocateId = allocateId
      this.state = state
      this.visible = true
      if (allocateId != null) {
        this.getDataList()
      }
    },
    onDrawerClose() {
      this.visible = false
      this.parentObj.getDataList()
    },
    approveData() {
      var thisObj = this
      this.$http.post('/TD/TD_Allocate/ApproveDatas', [this.moveId]).then(resJson => {
        if (resJson.Success) {
          thisObj.$message.success('操作成功!')
        } else {
          thisObj.$message.error(resJson.Msg)
        }
      })
    },
    rejectData() {
      var thisObj = this
      this.$http.post('/TD/TD_Allocate/RejectDatas', [this.moveId]).then(resJson => {
        if (resJson.Success) {
          thisObj.$message.success('操作成功!')
        } else {
          thisObj.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>