<template>
  <a-drawer
    title="关联托盘类型"
    placement="right"
    :closable="true"
    @close="onDrawerClose"
    :visible="visible"
    :width="1000"
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

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="类型编号或名称" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
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
          <a-divider type="vertical" />
          <a @click="openZoneList(record.Id)">分区管理</a>
          <a-divider type="vertical" />
          <a @click="openMaterialList(record.Id)">关联物料</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <zone-list ref="zoneList" :parentObj="this"></zone-list>
    <material-list ref="materialList" :parentObj="this"></material-list>
  </a-card>
  </a-drawer>
</template>

<script>
import EditForm from './EditForm'
import ZoneList from '../PB_TrayZone/List'
import MaterialList from '../PB_TrayMaterial/List'

const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}
const columns = [
  { title: '编号', dataIndex: 'Code', width: '10%' },
  { title: '名称', dataIndex: 'Name', width: '10%' },
  { title: '长', dataIndex: 'Length', width: '10%' },
  { title: '宽', dataIndex: 'Width', width: '10%' },
  { title: '高', dataIndex: 'High', width: '10%' },
  { title: '是否有分区', dataIndex: 'IsZone', width: '10%', customRender: filterYesOrNo },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    ZoneList,
    MaterialList
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
      selectedRowKeys: [],
      visible: false,
      childrenDrawer: false,
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
        .post('/PB/PB_TrayType/GetDataList', {
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
            thisObj.$http.post('/PB/PB_TrayType/DeleteData', ids).then(resJson => {
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
    openZoneList(typeId) {
      this.$refs.zoneList.openDrawer(typeId)
    },
    openMaterialList(typeId) {
      this.$refs.materialList.openDrawer(typeId)
    },
    onDrawerClose() {
      this.visible = false
    },
    onChildrenDrawerClose() {
      this.childrenDrawer = false;
    },
    openDrawer(typeId) {
      this.typeId = typeId
      this.visible = true
      if (typeId != null) {
        this.getDataList()
      }
    }
  }
}
</script>