<template>
  <a-drawer
    title="移库明细"
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
        <a-button type="primary" icon="check" @click="approveData()">审批</a-button>
        <a-button type="primary" icon="close" @click="rejectData()">驳回</a-button>
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
            <a
              @click="handleEdit(record.Id, record.FromLocalId, record.FromTrayId, record.FromZoneId, record.MaterialId, record.BatchNo, record.BarCode)"
            >编辑</a>
            <a-divider type="vertical" />
            <a @click="handleDelete([record.Id])">删除</a>
          </template>
        </span>
      </a-table>

      <edit-form ref="editForm" :parentObj="this"></edit-form>
      <add-form ref="addForm" :parentObj="this"></add-form>
    </a-card>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'
import AddForm from './AddForm'

const columns = [
  { title: '原货位', dataIndex: 'Src_Location.Name', width: '8%' },
  { title: '原托盘', dataIndex: 'Src_Tray.Name', width: '8%' },
  { title: '原托盘分区', dataIndex: 'Src_TrayZone.Name', width: '8%' },
  { title: '目标货位', dataIndex: 'Tar_Location.Name', width: '8%' },
  { title: '目标托盘', dataIndex: 'Tar_Tray.Name', width: '8%' },
  { title: '目标托盘分区', dataIndex: 'Tar_TrayZone.Name', width: '8%' },
  { title: '条码', dataIndex: 'BarCode', width: '8%' },
  { title: '物料', dataIndex: 'PB_Material.Name', width: '8%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '8%' },
  { title: '移库数量', dataIndex: 'LocalNum', width: '5%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  props: {
    parentObj: Object
  },
  components: {
    EditForm,
    AddForm
  },
  mounted() {},
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
      selectedRowKeys: [],
      visible: false,
      moveId: null,
      state: null
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
      var thisObj = this
      this.selectedRowKeys = []

      this.loading = true
      this.queryParam.Keyword = this.moveId
      this.$http
        .post('/TD/TD_MoveDetail/GetDataList', {
          PageIndex: thisObj.pagination.current,
          PageRows: thisObj.pagination.pageSize,
          SortField: thisObj.sorter.field || 'Id',
          SortType: thisObj.sorter.order,
          Search: thisObj.queryParam,
          ...thisObj.filters
        })
        .then(resJson => {
          thisObj.loading = false
          thisObj.data = resJson.Data
          const pagination = { ...thisObj.pagination }
          pagination.total = resJson.Total
          thisObj.pagination = pagination
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.addForm.openForm(this.moveId)
    },
    handleEdit(id, LocalId, TrayId, ZoneId, MaterialId, BatchNo, BarCode) {
      this.$refs.editForm.openForm(this.moveId, id, LocalId, TrayId, ZoneId, MaterialId, BatchNo, BarCode)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/TD/TD_MoveDetail/DeleteData', ids).then(resJson => {
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
    openDrawer(moveId, state) {
      this.moveId = moveId
      this.state = state
      this.visible = true
      if (moveId != null) {
        this.getDataList()
      }
    },
    onDrawerClose() {
      this.visible = false
      this.parentObj.getDataList()
    },
    approveData() {
      var thisObj = this
      this.$http.post('/TD/TD_Move/ApproveDatas', [this.moveId]).then(resJson => {
        if (resJson.Success) {
          thisObj.$message.success('操作成功!')
        } else {
          thisObj.$message.error(resJson.Msg)
        }
      })
    },
    rejectData() {
      var thisObj = this
      this.$http.post('/TD/TD_Move/RejectDatas', [this.moveId]).then(resJson => {
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