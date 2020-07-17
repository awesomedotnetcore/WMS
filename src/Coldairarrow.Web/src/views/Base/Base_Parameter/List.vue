<template>
  <a-card :bordered="false">
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="10">
          <a-col :md="4" :sm="24">
            <a-form-item>
              <a-input v-model="queryParam.Keyword" placeholder="参数编号/名称" />
            </a-form-item>
          </a-col>
          <a-col :md="6" :sm="24">
            <a-button type="primary" @click="() => {this.pagination.current = 1; this.getDataList()}">查询</a-button>
            <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <a-table ref="table" :columns="columns" :rowKey="row => row.Id" :dataSource="data" :pagination="pagination" :loading="loading" @change="handleTableChange" :bordered="true" size="small">
      <template slot="Type" slot-scope="text">
        <enum-name code="SystemType" :value="text"></enum-name>
      </template>
      <template slot="CusVal" slot-scope="text, record">
        <div class="editable-cell">
          <div v-if="record.editable" class="editable-cell-input-wrapper">
            <a-select v-if="record.ValConfig.Type==='select'" :value="record.Val" :style="{width:'70%'}" size="small" @select="e=>handleValChange(record,e)">
              <a-select-option v-for="item in record.ValConfig.Data" :key="item.Value" :value="item.Value">{{ item.Name }}</a-select-option>
            </a-select>
            <a-input v-else :value="record.Val" :style="{width:'70%'}" size="small"></a-input>
            <a-icon type="check" class="editable-cell-icon-check" @click="handleUpdate(record)" />
          </div>
          <div v-else class="editable-cell-text-wrapper">
            <span v-if="record.ValConfig.Type==='select'">
              {{ record.ValConfig.Data.find(n=>n.Value===record.Val).Name }}
            </span>
            <span v-else>{{ text }}</span>
            <a-icon type="edit" class="editable-cell-icon" @click="e=>{record.editable=true}" />
          </div>
        </div>

      </template>
      <span slot="IsSystem" slot-scope="text, record">
        <template>
          <a-tag v-if="record.IsSystem===false" color="red">否</a-tag>
          <a-tag v-else color="green">是</a-tag>
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
  { title: '参数类型', dataIndex: 'Type', scopedSlots: { customRender: 'Type' } },
  { title: '参数编号', dataIndex: 'Code' },
  { title: '参数名称', dataIndex: 'Name' },
  { title: '参数值', dataIndex: 'Val', width: '10%', scopedSlots: { customRender: 'CusVal' } },
  { title: '描述', dataIndex: 'Remarks' },
  { title: '系统必须', dataIndex: 'IsSystem', scopedSlots: { customRender: 'IsSystem' } }
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
      queryParam: {}
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
        .post('/Base/Base_Parameter/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then(resJson => {
          this.loading = false
          resJson.Data.forEach(item => {
            item.ValConfig = JSON.parse(item.ValConfig)
            item.editable = false
          })
          this.data = resJson.Data
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    },
    handleValChange(item, val) {
      item.Val = val
    },
    handleUpdate(record) {
      record.editable = false
      var entity = { Id: record.Id, Code: record.Code, Val: record.Val }
      this.$http.post('/Base/Base_Parameter/SaveData', entity).then(resJson => {
        if (resJson.Success) {
          this.$message.success('参数修改成功!')
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>