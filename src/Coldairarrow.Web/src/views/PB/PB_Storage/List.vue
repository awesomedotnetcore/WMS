<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button v-if="hasPerm('PB_Storage.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
      <a-button v-if="hasPerm('PB_Storage.Delete')" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
      <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
    </div>

    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="仓库编号或名称" />
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
      <template slot="Type" slot-scope="text">
        <enum-name code="StorageType" :value="text"></enum-name>
      </template>

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
      <span slot="IsTray" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsTray===false" color="red">
          否
          </a-tag>
          <a-tag v-else color="green">
          是
          </a-tag>
        </template>
      </span>
      <span slot="IsZone" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsZone===false" color="red">
          否
          </a-tag>
          <a-tag v-else color="green">
          是
          </a-tag>
        </template>
      </span>
      <span slot="Disable" slot-scope="text, record">
        <template>
          <a-tag v-if="record.Disable===false" color="red">
          停用
          </a-tag>
          <a-tag v-else color="green">
          启用
          </a-tag>
        </template>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>              
          <a v-if="hasPerm('PB_Storage.Laneway')" @click="openLanewayList(record)">设置巷道</a>
          <a-divider v-if="hasPerm('PB_Storage.Laneway')" type="vertical" />
          <a v-if="hasPerm('PB_Storage.Rack')" @click="openRackList(record)">设置货架</a>
          <a-divider v-if="hasPerm('PB_Storage.Config')" type="vertical" />
          <a v-if="hasPerm('PB_Storage.Config')"  @click="handleConfig(record.Id)">配置</a>
          <a-divider v-if="record.Disable===false || hasPerm('PB_Storage.Edit')" type="vertical" />
          <a v-if="record.Disable===false || hasPerm('PB_Storage.Edit')" @click="handleEdit(record.Id)">编辑</a>          
          <a-divider v-if="record.Disable===false || hasPerm('PB_Storage.Delete')" type="vertical" />
          <a v-if="record.Disable===false || hasPerm('PB_Storage.Delete') ">删除</a>                   
        </template>
      </span>
    </a-table>
    
    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <config-form ref="configForm" :parentObj="this"></config-form>
    <laneway-List ref="lanewayList"></laneway-List>
    <rack-List ref="rackList"></rack-List>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import ConfigForm from './ConfigFrom'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import LanewayList from '../PB_Laneway/List'
import RackList from '../PB_Rack/List'


const columns = [
  { title: '仓库编号', dataIndex: 'Code'},
  { title: '仓库名称', dataIndex: 'Name' },
  { title: '仓库类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' } },
  { title: '托盘管理', dataIndex: 'IsTray',  scopedSlots: { customRender: 'IsTray' }},
  { title: '托盘分区', dataIndex: 'IsZone', scopedSlots: { customRender: 'IsZone'}},
  { title: '仓库状态', dataIndex: 'Disable' , scopedSlots: { customRender: 'Disable' }},
  { title: '默认仓库', dataIndex: 'IsDefault', scopedSlots: { customRender: 'IsDefault' } },
  // { title: '备注', dataIndex: 'Remarks'},
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    LanewayList,
    RackList,
    EditForm,
    EnumName,  
    ConfigForm  
  },
  mounted() {
    this.getDataList()
  },
  data() {
    return {
      value: 1,
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
        .post('/PB/PB_Storage/GetDataList', {
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
      this.$refs.editForm.openForm(null, '新增仓库')
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id, '编辑仓库')
    },
    handleConfig(id) {
      this.$refs.configForm.openForm(id, '编辑配置')
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Storage/DeleteData', ids).then(resJson => {
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
    openLanewayList(value) {
      this.$refs.lanewayList.openDrawer(value)
    },
    openRackList(value) {
      this.$refs.rackList.openDrawer(value)
    },   
    handleDefault(Storage,prop, enable) {
      var thisObj = this
      var entity = { ...Storage}
      entity[prop] = enable   
      this.$confirm({
        title: '确认将此仓库设置为默认仓库吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Storage/SaveDataDefault', entity).then(resJson => {
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
    handleModifyIsTray(id){
      this.$http
        .post('/PB/PB_Storage/ModifyIsTray?id='+id)
        .then(resJson => {
          this.getDataList()
        })
    },
    handleModifyIsZone(id){
      this.$http
        .post('/PB/PB_Storage/ModifyIsZone?id='+id)
        .then(resJson => {
          this.getDataList()
        })
    },
    handleModifyDisable(id){
      this.$http
        .post('/PB/PB_Storage/ModifyDisable?id='+id)
        .then(resJson => {
          this.getDataList()
        })
    },
  }
}
</script>