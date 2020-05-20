<template>
  <a-card :bordered="false">
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
              <a-input v-model="queryParam.Code" placeholder="编码" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Name" placeholder="名称" />
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
        <enum-name code="StorageType" :value="text"></enum-name>
      </template>

      <span slot="IsTray" slot-scope="text, record">    
        <template>
        <a-button :type="record.IsTray?'primary':'default'">
        <a v-if="record.IsTray" @click="handleEnable(record,'IsTray',false)">启用</a>
        <a v-else @click="handleEnable(record,'IsTray',true)">停用</a>
        </a-button>
        </template>
      </span>

      <span slot="IsZone" slot-scope="text, record">    
        <template>
        <a-button :type="record.IsZone?'primary':'default'">
        <a v-if="record.IsZone" @click="handleEnable(record,'IsZone',false)">启用</a>
        <a v-else @click="handleEnable(record,'IsZone',true)">停用</a>
        </a-button>
        </template>
      </span>

      <span slot="disable" slot-scope="text, record">    
        <template>
        <a-button :type="record.disable?'primary':'default'">
        <a v-if="record.disable" @click="handleEnable(record,'disable',false)">启用</a>
        <a v-else @click="handleEnable(record,'disable',true)">停用</a>
        </a-button>
        </template>
      </span>

      <!-- <template slot="IsDefault"> 
      <a-radio-group :name="record.IsDefault" v-model="value" @change="onChange">
            <a-radio :style="radioStyle" :value="1">
              Option A
            </a-radio>
            </a-radio-group> 
      </template>  -->

      <span slot="IsDefault" slot-scope="text, record">    
        <template>
        <!-- <a-radio :type="record.IsDefault?'primary':'IsDefault'">
          <a v-if="record.IsDefault" @click="handleEnable(record,'IsDefault',false)">启用</a>
          <a v-else @click="handleEnable(record,'IsDefault',true)">停用</a>
        </a-radio> -->

            

          <!-- <a-radio-group v-model="value" @change="onChange">
            <a-radio :style="radioStyle" :value="1">
              Option A
            </a-radio>
          </a-radio-group> -->


        <a-button :type="record.IsDefault?'primary':'default'">
        <a v-if="record.IsDefault" @click="handleEnable(record,'IsDefault',false)">启用</a>
        <a v-else @click="handleEnable(record,'IsDefault',true)">停用</a>
        </a-button>
        </template>
      </span>


      <span slot="action" slot-scope="text, record">
        <template>
          <a  @click="handleEdit(record.Id)">编辑</a>
          <a-divider  type="vertical" />
          <a  @click="handleDelete([record.Id])">删除</a>
          <a-divider  type="vertical" />
          <a @click="openLanewayList(record)">设置巷道</a>
          <a-divider  type="vertical" />
          <a @click="openRackList(record)">设置货架</a>
        </template>
      </span>
    </a-table>
    
    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <laneway-List ref="lanewayList"></laneway-List>
    <rack-List ref="rackList"></rack-List>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import LanewayList from '../PB_Laneway/List'
import RackList from '../PB_Rack/List'

const filterYesOrNo = (value, row, index) => {
  if (value) return '是'
  else return '否'
}

const columns = [
  { title: '仓库编号', dataIndex: 'Code', width: '10%' },
  { title: '仓库名称', dataIndex: 'Name', width: '10%' },
  { title: '仓库类型', dataIndex: 'Type', width: '10%', scopedSlots: { customRender: 'Type' } },
  { title: '托盘管理', dataIndex: 'IsTray', width: '10%' , scopedSlots: { customRender: 'IsTray' }},
  { title: '托盘分区管理', dataIndex: 'IsZone', width: '10%' , scopedSlots: { customRender: 'IsZone' }},
  { title: '仓库状态', dataIndex: 'disable', width: '10%' , scopedSlots: { customRender: 'disable' }},
  //{ title: '默认仓库', dataIndex: 'IsDefault', width: '8%'  , customRender: filterYesOrNo},
  { title: '默认仓库', dataIndex: 'IsDefault', width: '8%' , scopedSlots: { customRender: 'IsDefault' }},
  { title: '备注', dataIndex: 'Remarks', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    LanewayList,
    RackList,
    EditForm,
    EnumName,    
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
    handleEnable(Storage,prop, enable) {
      var thisObj = this
      var entity = { ...Storage}
      entity[prop] = enable   
      this.$confirm({
        title: '确认' + (enable ? '停用' : '启用' ) + '吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/PB/PB_Storage/SaveData', entity).then(resJson => {
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
    onChange(e) {
      console.log('radio checked', e.target.value);
    },
  }
}
</script>