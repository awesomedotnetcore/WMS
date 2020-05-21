<template>
  <a-drawer title="条码规则" placement="right" :closable="true" :maskClosable="false" @close="onDrawerClose" :visible="visible" :width="1024" :getContainer="false">
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
              <enum-select code="BarCodeRuleType" v-model="queryParam.Type"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="getDataList">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <template slot="Type" slot-scope="text">
        <enum-name code="BarCodeRuleType" :value="text"></enum-name>
      </template>
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
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
const columns = [
  { title: '类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' }, width: '10%' },
  { title: '排序', dataIndex: 'Sort', width: '10%' },
  { title: '规则', dataIndex: 'Rule', width: '10%' },
  { title: '长度', dataIndex: 'length', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumSelect,
    EnumName
  },
  mounted() {
    this.getDataList()
  },
  data() {
    return {
      visible: false,
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: { TypeId: '' },
      sorter: { field: 'Sort', order: 'asc' },
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
        .post('/PB/PB_BarCodeRule/GetDataList', {
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
      this.$refs.editForm.openForm(this.filters.TypeId)
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(this.filters.TypeId, id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_BarCodeRule/DeleteData', ids).then(resJson => {
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
    openDrawer(typeId) {
      this.filters.TypeId = typeId
      this.visible = true
      this.getDataList()
    },
    onDrawerClose() {
      this.visible = false
    }
  }
}
</script>