<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button v-if="hasPerm('PB_StorArea.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('PB_StorArea.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <storage-select v-model="queryParam.StorageId"></storage-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.keyword" placeholder="货区编号或名称" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="StorAreaType" v-model="queryParam.AreaType"></enum-select>
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
      <!-- <span slot="IsCache" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsCache===false" color="red">否</a-tag>
          <a-tag v-else color="green">是</a-tag>
        </template>
      </span> -->

      <template slot="Type" slot-scope="text">
        <enum-name code="StorAreaType" :value="text"></enum-name>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="hasPerm('PB_StorArea.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('PB_StorArea.Delete')" type="vertical" />
          <a v-if="hasPerm('PB_StorArea.Delete')" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="hasPerm('PB_StorArea.MaterialList')" type="vertical" />
          <a v-if="hasPerm('PB_StorArea.MaterialList')" @click="openMaterialList(record.Id)">关联物料</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <material-list ref="materialList" :parentObj="this"></material-list>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import MaterialList from '../PB_AreaMaterial/List'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import StorageSelect from '../../../components/Storage/StorageSelect'

const columns = [
  { title: '仓库', dataIndex: 'PB_Storage.Name' },
  { title: '货区编号', dataIndex: 'Code' },
  { title: '货区名称', dataIndex: 'Name' },
  { title: '货区类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' } },
  // { title: '缓存区', dataIndex: 'IsCache', width: '10%', scopedSlots: { customRender: 'IsCache' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    MaterialList,
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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: { },
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
        .post('/PB/PB_StorArea/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新增货区')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑货区')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_StorArea/DeleteData', ids).then(resJson => {
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
    openMaterialList(areaId) {
      this.$refs.materialList.openDrawer(areaId)
    }
  }
}
</script>