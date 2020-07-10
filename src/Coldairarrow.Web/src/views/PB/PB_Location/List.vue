<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="10">
        <a-col :md="20" :sm="24">
          <a-button v-if="hasPerm('PB_Location.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
          <a-button v-if="hasPerm('PB_Location.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
          <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
        <a-button v-if="hasPerm('PB_Location.Leading')" type="primary" @click="hanldleLeading()">导入货位</a-button>
      </a-row>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <all-storage-select v-model="queryParam.StorId"></all-storage-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="货位/巷道/货架" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.AreaName" placeholder="货区编码/名称" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }" :bordered="true" size="small">
      <span slot="IsForbid" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsForbid===false" color="red">
            停用
          </a-tag>
          <a-tag v-else color="green">
            启用
          </a-tag>
        </template>
      </span>

      <span slot="IsDefault" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsDefault===false" color="red">
            否
          </a-tag>
          <a-tag v-else color="green">
            是
          </a-tag>
        </template>
      </span>

      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="hasPerm('PB_Location.Edit')" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="hasPerm('PB_Location.Delete')" type="vertical" />
          <a v-if="hasPerm('PB_Location.Delete')" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <leading-form ref="leadingForm" :parentObj="this"></leading-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import UploadFile from '../../../components/CUploadFile/CUploadFile'
import LeadingForm from './LeadingForm'
import AllStorageSelect from '../../../components/Storage/AllStorageSelect'

const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}

const columns = [
  { title: '货位编号', dataIndex: 'Code'},
  { title: '货位名称', dataIndex: 'Name'},
  { title: '仓库', dataIndex: 'PB_Storage.Name'},
  { title: '货区', dataIndex: 'PB_StorArea.Name'},
  { title: '巷道', dataIndex: 'PB_Laneway.Name' },
  { title: '货架', dataIndex: 'PB_Rack.Name' },
  { title: '余量', dataIndex: 'OverVol' },
  { title: '状态', dataIndex: 'IsForbid', scopedSlots: { customRender: 'IsForbid' } },//是否禁用
  { title: '默认', dataIndex: 'IsDefault', scopedSlots: { customRender: 'IsDefault' } },//是否默认库位
  // { title: '故障码', dataIndex: 'ErrorCode', width: '6' },
  // { title: '备注', dataIndex: 'Remarks', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName,
    UploadFile,
    LeadingForm,
    AllStorageSelect
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
      entity: {},
      fileList: [],
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
        .post('/PB/PB_Location/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新增货位')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑货位')
    },
    hanldleLeading() {
      this.$refs.leadingForm.openForm(null, '导入货位')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Location/DeleteData', ids).then(resJson => {
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
    handleEnable(Location, prop, enable) {
      var thisObj = this
      var entity = { ...Location }
      entity[prop] = enable
      this.$confirm({
        title: '确认' + (enable ? '启用' : '停用') + '吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Location/SaveData', entity).then(resJson => {
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
  }
}
</script>