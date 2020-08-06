<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="10">
        <a-col :md="20" :sm="24">
      <a-button v-if="hasPerm('PB_Material.Add')"  type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('PB_Material.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
      <a-button v-if="hasPerm('PB_Material.Leading')" type="primary" @click="hanldleLeading()">导入物料</a-button>
      </a-row>
      
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <materialType-select v-model="queryParam.TypeId"></materialType-select>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="物料名称/编号/条码" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.ContactName" placeholder="客户/供应商名称或编码" />
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
          <a v-if="hasPerm('PB_Material.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('PB_Material.Delete')" type="vertical" />
          <a v-if="hasPerm('PB_Material.Delete')" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <leading-show ref="leadingShow" :parentObj="this" leading="/PB/PB_Material/Import" templet="/PB/PB_Material/ExportToExcel"></leading-show>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import MaterialTypeSelect from '../../../components/PB/MaterialTypeSelect'
import LeadingShow from '../../../components/PB/LeadingShow'

const columns = [
  { title: '物料名称', dataIndex: 'Name'},
  { title: '物料编码', dataIndex: 'Code'},
  { title: '物料简称', dataIndex: 'SimpleName' },
  { title: '物料类别', dataIndex: 'MaterialType.Name' },
  { title: '条码', dataIndex: 'BarCode' },
  { title: '物料规格', dataIndex: 'Spec' },
  { title: '单位', dataIndex: 'Measure.Name' },
  { title: '上限数量', dataIndex: 'Max'},
  { title: '下限数量', dataIndex: 'Min' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    MaterialTypeSelect,
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
        .post('/PB/PB_Material/QueryDataList', {
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
      this.$refs.leadingShow.openForm(null, '导入物料')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Material/DeleteData', ids).then(resJson => {
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
    }
  }
}
</script>