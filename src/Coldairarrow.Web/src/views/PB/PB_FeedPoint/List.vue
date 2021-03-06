﻿<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="10">
        <a-col :md="20" :sm="24">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
      <a-button v-if="hasPerm('PB_FeedPoint.Leading')" type="primary" @click="hanldleLeading()">导入上下料点</a-button>  
      </a-row>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="名称/编码" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <storage-select v-model="queryParam.StorId"></storage-select>
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="PointType" v-model="queryParam.Type"></enum-select>
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
      <template slot="NameCode" slot-scope="obj">
        <a-tooltip>
          <template slot="title">
            {{ obj.Code }}
          </template>
            {{ obj.Name }}
        </a-tooltip>
      </template>
      <template slot="YesOrNo" slot-scope="text">
        <a-tag v-if="text" color="green">是</a-tag>
        <a-tag v-else color="pink">否</a-tag>
      </template>
      <template slot="EnumName" slot-scope="text">
        <enum-name code="PointType" :value="text"></enum-name>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="!record.IsEnable" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="!record.IsEnable" type="vertical" />
          <a v-if="!record.IsEnable" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="!record.IsEnable" type="vertical" />
          <a @click="handleEnable(record.Id,!record.IsEnable)">{{ record.IsEnable?'停用':'启用' }}</a>
          <a-divider type="vertical" />
          <a @click="openMaterialList(record.Id)">关联物料</a>
        </template>
      </span>
    </a-table>
    <material-list ref="materialList" :parentObj="this"></material-list>
    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <leading-show ref="leadingShow" :parentObj="this" leading="/PB/PB_FeedPoint/Import" templet="/PB/PB_FeedPoint/ExportToExcel"></leading-show>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import MaterialList from '../PB_MaterialPoint/List'
import StorageSelect from '../../../components/Storage/AllStorageSelect'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import LeadingShow from '../../../components/PB/LeadingShow'

const columns = [
  { title: '名称', dataIndex: 'Name' },
  { title: '编码', dataIndex: 'Code' },
  { title: '仓库', dataIndex: 'Storage', scopedSlots: { customRender: 'NameCode' } },
  { title: '巷道', dataIndex: 'Laneway', scopedSlots: { customRender: 'NameCode' } },
  { title: '类型', dataIndex: 'Type', scopedSlots: { customRender: 'EnumName' } },
  { title: '启用', dataIndex: 'IsEnable', scopedSlots: { customRender: 'YesOrNo' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    StorageSelect,
    MaterialList,
    EnumName,
    EnumSelect,
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
        .post('/PB/PB_FeedPoint/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新建')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑')
    },
    hanldleLeading() {
      this.$refs.leadingShow.openForm(null, '导入上下料点')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_FeedPoint/DeleteData', ids).then(resJson => {
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
    handleEnable(id, enable) {
      var thisObj = this
      this.$confirm({
        title: '确认' + (enable ? '启用' : '停用') + '此料点吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_FeedPoint/Enable?Id=' + id + '&enable=' + enable).then(resJson => {
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
    openMaterialList(pointId) {
      this.$refs.materialList.openDrawer(pointId)
    }
  }
}
</script>