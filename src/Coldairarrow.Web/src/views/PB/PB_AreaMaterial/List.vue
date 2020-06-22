<template>
<a-drawer
    title="关联物料清单"
    placement="right"
    :closable="true"
    @close="onDrawerClose"
    :visible="visible"
    :width="900"
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
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.MaterialId"
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
          <!-- <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" /> -->
          <a @click="handleDelete([record.MaterialId])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</a-drawer>
</template>

<script>
import EditForm from './EditForm'

const columns = [
  { title: '物料编码', dataIndex: 'PB_Material.Code', width: '20%' },
  { title: '物料名称', dataIndex: 'PB_Material.Name', width: '20%' },
  { title: '物料规格', dataIndex: 'PB_Material.Spec' },
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
      sorter: { field: 'MaterialId', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      visible: false,
      areaId: null,
      ids: []
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
      this.queryParam.Keyword = this.areaId

      this.loading = true
      this.$http
        .post('/PB/PB_AreaMaterial/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'MaterialId',
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
      this.$refs.editForm.openForm(this.areaId,"添加关联物料")
    },
    // handleEdit(id) {
    //   this.$refs.editForm.openForm(id)
    // },
    handleDelete(ids) {
      this.ids = ids
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_AreaMaterial/DeleteData?AreaId=' + thisObj.areaId, thisObj.ids)
            .then(resJson => {
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
    openDrawer(areaId) {
      this.areaId = areaId
      this.visible = true
      if (areaId != null) {
        this.getDataList()
      }
    },
    onDrawerClose() {
      this.visible = false
    }
  }
}
</script>