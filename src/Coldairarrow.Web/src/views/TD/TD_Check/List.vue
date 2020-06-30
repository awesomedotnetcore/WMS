<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row>
          <a-col :span="24">
            <a-form-item>
              <a-button v-if="hasPerm('TD_Check.Add')" style="margin-left: 8px" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
              <a-button v-if="hasPerm('TD_Check.Delete')" style="margin-left: 8px" type="primary" icon="minus" @click="handleDelete(selectedRowKeys)" :disabled="!hasSelected()" :loading="loading">删除</a-button>
              <a-button style="margin-left: 8px" type="primary" icon="redo" @click="getDataList()">刷新</a-button>
              <a-divider type="vertical" />
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
              <a-divider type="vertical" />
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
                <a-radio-button :value="3">
                  退回
                </a-radio-button>
              </a-radio-group>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <enum-select code="CheckType" v-model="queryParam.Type" :allowClear="true"></enum-select>
            </a-form-item>
          </a-col>
          <a-col :md="5" :sm="24">
            <a-form-item>
              <a-range-picker v-model="queryParam.RangeDate" :ranges="{ 
                  '本星期': [moment().startOf('week'), moment().endOf('week')]
                  ,'本月': [moment().startOf('month'), moment().endOf('month')]                  
                  ,'本季度': [moment().startOf('quarter'), moment().endOf('quarter')] 
                  ,'本年度': [moment().startOf('year'), moment().endOf('year')]
                  ,'上月': [moment().startOf('month').add(-1,'month'), moment().endOf('month').add(-1,'month')]
                  ,'上季度': [moment().startOf('quarter').add(-1,'quarter'), moment().endOf('quarter').add(-1,'quarter')]
                  ,'上年度': [moment().startOf('year').add(-1,'year'), moment().endOf('year').add(-1,'year')]
                }" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.RefCode" placeholder="盘点编码/关联单号" />
            </a-form-item>
          </a-col>
          <a-col :md="4" :sm="24">
            <a-button style="margin-left: 8px" type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange, getCheckboxProps: CheckboxProps }" :bordered="true" size="small">
      <template slot="IsComplete" slot-scope="text, record">
        <a-tag v-if="record.IsComplete===true" color="green">已盘</a-tag>
        <a-tag v-else color="blue">待盘</a-tag>
      </template>
      <template slot="Status" slot-scope="text, record">
        <span v-if="record.IsComplete===true">
          <a-tag v-if="record.Status===0" color="blue">待审核</a-tag>
          <a-tag v-else-if="record.Status===1" color="green">审核通过</a-tag>
          <a-tag v-else-if="record.Status===2" color="red">审核失败</a-tag>
          <a-tag v-else-if="record.Status===3" color="red">退回</a-tag>
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
          <a v-if="record.IsComplete===false && hasPerm('TD_Check.Edit')" @click="handleEdit(record.Id)">编辑</a>

          <a-divider v-if="record.IsComplete===false && hasPerm('TD_Check.Delete')" type="vertical" />
          <a v-if="record.IsComplete===false && hasPerm('TD_Check.Delete')" @click="handleDelete([record.Id])">删除</a>
          <a-divider v-if="record.IsComplete===false && hasPerm('TD_Check.Delete')" type="vertical" />

          <a v-if="hasPerm('TD_Check.Check')" @click="handleCheck(record.Id)">盘差</a>

          <a-divider v-if="record.IsComplete===false && hasPerm('TD_Check.ReCheck')" type="vertical" />
          <a v-if="record.IsComplete===false && hasPerm('TD_Check.ReCheck')" @click="handleReCheck(record.Id)">复核</a>
          
          <a-divider v-if="(record.IsComplete===true && record.Status===0) && hasPerm('TD_Check.Auditing')" type="vertical" />
          <a v-if="(record.IsComplete===true && record.Status===0) && hasPerm('TD_Check.Auditing')" @click="handleAudit(record.Id)">审核</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <show ref="show" :parentObj="this"></show>
    <check ref="check" :parentObj="this"></check>
    <audite ref="audite" :parentObj="this"></audite>
  </a-card>
</template>

<script>
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'
import EditForm from './EditForm'
import Show from './Show'
import Check from './Check'
import Audite from './Audite'
import EnumName from '../../../components/BaseEnum/BaseEnumName'
import moment from 'moment'

const filterDate = (value, row, index) => {
  if (value) {
    return moment(value).format('YYYY-MM-DD')
  } else {
    return ' '
  }
}
const columns = [
  { title: '盘点编码', dataIndex: 'Code', },
  { title: '盘点类型', dataIndex: 'Type',  scopedSlots: { customRender: 'Type' } },
  { title: '盘点时间', dataIndex: 'CheckTime', customRender: filterDate},
  { title: '关联单号', dataIndex: 'RefCode' },
  { title: '盘差状态', dataIndex: 'IsComplete', scopedSlots: { customRender: 'IsComplete' } },
  { title: '审核状态', dataIndex: 'Status',  scopedSlots: { customRender: 'Status' } },
  { title: '审核人', dataIndex: 'AuditUser.RealName'},//AuditUserId
  { title: '审核时间', dataIndex: 'AuditeTime'},
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EnumSelect,
    EditForm,
    EnumName,
    Show,
    Check,
    Audite
  },
  mounted() {
    this.getDataList()
  },
  data() {
    return {
      dateFormat: 'YYYY-MM-DD',
      monthFormat: 'YYYY-MM',
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'CheckTime', order: 'desc' },
      loading: false,
      columns,
      queryParam: {
        RangeDate: [moment().startOf('month'), moment().endOf('month')]
      },
      selectedRowKeys: []
    }
  },
  methods: {
    moment,
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    CheckboxProps(record) {
      return {
        props: {
          disabled: record.IsComplete === true
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
      this.$refs.editForm.openForm(null,"新增盘点")
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id,"编辑盘点")
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
    handleCheck(id) {
      this.$refs.show.openForm(id)
    },
    handleReCheck(id) {
      this.$refs.check.openForm(id)
    },
    handleAudit(id) {
      this.$refs.audite.openForm(id)
    },
  }
}
</script>