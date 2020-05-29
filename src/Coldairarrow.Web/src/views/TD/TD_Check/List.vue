<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-radio-group v-model="queryParam.IsComplete" :default-value="-1" button-style="solid" @change="getDataList">
                <a-radio-button :value="-1">
                  全部
                </a-radio-button>
                <a-radio-button :value="0">
                  待盘
                </a-radio-button>
                <a-radio-button :value="1">
                  已盘
                </a-radio-button>
              </a-radio-group>
            </a-form-item>
          </a-col>
          <a-col :md="5" :sm="24">
            <a-form-item>
              <a-radio-group v-model="queryParam.Status" :default-value="-1" button-style="solid" @change="getDataList">
                <a-radio-button :value="-1">
                  全部
                </a-radio-button>
                <a-radio-button :value="0">
                  待审核
                </a-radio-button>
                <a-radio-button :value="1">
                  审核通过
                </a-radio-button>
                <a-radio-button :value="2">
                  审核失败
                </a-radio-button>
              </a-radio-group>
            </a-form-item>
          </a-col>
          <a-col :md="3" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.RefCode" placeholder="关联单号" />
            </a-form-item>
          </a-col>
          <a-col :md="13" :sm="24">
            <a-button type="primary" @click="getDataList">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
            <a-button style="margin-left: 8px" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
            <a-button style="margin-left: 8px" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading" >删除</a-button>
            <a-button style="margin-left: 8px" type="primary" icon="redo" @click="getDataList()">刷新</a-button>
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
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange, getCheckboxProps: CheckboxProps }"
      :bordered="true"
      size="small"
    >
      <template slot="IsComplete" slot-scope="text, record">
         <a-tag v-if="record.IsComplete===true">已盘</a-tag>
         <a-tag v-else color="green">待盘</a-tag>
      </template>
      <template slot="Status" slot-scope="text, record">
        <span v-if="record.IsComplete===true">
          <a-tag v-if="record.Status===0" color="green">待审核</a-tag>
          <a-tag v-else-if="record.Status===1">审核通过</a-tag>
          <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
          <a-tag v-else color="green">待审核</a-tag>
        </span>
        <span v-else>
          -
        </span>
      </template>
      <template slot="Type" slot-scope="text">
        <enum-name code="CheckType" :value="text"></enum-name>
      </template>
      <span slot="action" slot-scope="text, record">
        <template>
          <a v-if="record.IsComplete===false" @click="handleEdit(record.Id)">编辑</a>
          <a-divider v-if="record.IsComplete===false" type="vertical" />
          <a v-if="record.Status!==1" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="record.Status!==1" type="vertical" />
          <a @click="handleCheck(record.Id)">盘差</a>
          <a-divider v-if="record.Status===0" type="vertical" />
          <a v-if="record.IsComplete===false && record.Status===0" @click="handleReCheck(record.Id)">复核</a>
          <a-divider v-if="record.IsComplete===false && record.Status===0" type="vertical" />
          <a v-if="record.IsComplete===true && record.Status===0" @click="handleAudit(record.Id)">审核</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import EnumName from '../../../components/BaseEnum/BaseEnumName'

const columns = [
  { title: '盘点类型', dataIndex: 'Type', width: '10%', scopedSlots: { customRender: 'Type' } },
  { title: '盘点时间', dataIndex: 'CheckTime', width: '10%' },  
  { title: '关联单号', dataIndex: 'RefCode', width: '10%' },
  { title: '盘差状态', dataIndex: 'IsComplete', width: '10%', scopedSlots: { customRender: 'IsComplete' }  },
  { title: '审核状态', dataIndex: 'Status', width: '10%', scopedSlots: { customRender: 'Status' }  },
  { title: '审核人', dataIndex: 'AuditUserId', width: '10%' },
  { title: '审核时间', dataIndex: 'AuditeTime', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm,
    EnumName
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
    CheckboxProps(record){
      return{
        props: {
          disabled: record.IsComplete===true
        },
      }
    },
    getDataList() {
      this.selectedRowKeys = []
      this.loading = true
      this.$http
        .post('/TD/TD_Check/QueryDataList', {
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
            thisObj.$http.post('/TD/TD_Check/DeleteData', ids).then(resJson => {
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
    handleCheck(id){

    },
    handleReCheck(id){

    },
    handleAudit(id){
      
    }
  }
}
</script>