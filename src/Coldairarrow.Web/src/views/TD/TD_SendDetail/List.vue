<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-button type="primary" icon="plus" @click="hanldleAdd()">添加</a-button>
      <a-button
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.Id"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <template slot="PlanNum" slot-scope="text, record">
        <a-input-number  size="small" :value="text" :max="record.PlanNum" :min="1" @change="e=>handleValChange(e,'PlanNum',record)"></a-input-number>
      </template>

      <span slot="action" slot-scope="text, record">
        <template>
          <!-- <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" /> -->
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <!-- <edit-form ref="editForm" :parentObj="this"></edit-form> -->
    <material-report ref="materialReport" @choose="handlerDetailSubmit"></material-report>
  </a-card>
</template>

<script>
//import EditForm from './EditForm'
import MaterialReport from './MaterialReport'

const columns = [
  // { title: '发货Id', dataIndex: 'SendId', width: '10%' },
  // { title: '仓库ID', dataIndex: 'StorId', width: '10%' },
  { title: '物料ID', dataIndex: 'Material.Name' },
  { title: '单位ID', dataIndex: 'Measure.Name'},
  { title: '批次号', dataIndex: 'BatchNo'},
  { title: '库存数量', dataIndex: 'LocalNum'},
  { title: '出库数量', dataIndex: 'PlanNum' , scopedSlots: { customRender: 'PlanNum' }},
  { title: '单价', dataIndex: 'Price'},
  // { title: '发货数量', dataIndex: 'SendNum', width: '10%' },
  // { title: '总价', dataIndex: 'Amount', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
  //  EditForm,
    MaterialReport
  },
  props: {
    value: { type: Array, required: true },
    disabled: { type: Boolean, required: false, default: false }
  },
  watch: {
    value(val) {
      this.data = [...this.value]
    }
  },
  mounted() {
    this.data = [...this.value]
    this.getCurStorage()
    // this.getDataList()
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
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
          if (this.storage) {
            this.columns = columns
          }
        })
    },
    // handleTableChange(pagination, filters, sorter) {
    //   this.pagination = { ...pagination }
    //   this.filters = { ...filters }
    //   this.sorter = { ...sorter }
    //   this.getDataList()
    // },
    // getDataList() {
    //   this.selectedRowKeys = []

    //   this.loading = true
    //   this.$http
    //     .post('/TD/TD_SendDetail/GetDataList', {
    //       PageIndex: this.pagination.current,
    //       PageRows: this.pagination.pageSize,
    //       SortField: this.sorter.field || 'Id',
    //       SortType: this.sorter.order,
    //       Search: this.queryParam,
    //       ...this.filters
    //     })
    //     .then(resJson => {
    //       this.loading = false
    //       this.data = resJson.Data
    //       const pagination = { ...this.pagination }
    //       pagination.total = resJson.Total
    //       this.pagination = pagination
    //     })
    // },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.materialReport.openChoose()
      //this.$refs.editForm.openForm()
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
            thisObj.$http.post('/TD/TD_SendDetail/DeleteData', ids).then(resJson => {
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
    handlerDetailSubmit(rows) {
      console.log('handlerDetailSubmit', rows)

      rows.forEach(element => {
        this.tempId += 1
        var item = { ...element }
        // delete item.Id
        // delete item.Material
        // delete item.MaterialId
        item.Id = 'newid_' + this.tempId
        item.Material = { ...element.Material }
        // item.MaterialId = element.MaterialTypeName
        // item.MeasureId = element.MeasureName
        item.MaterialId = element.MaterialId
        item.MeasureId = element.MeasureId
        item.BatchNo = element.BatchNo
        item.LocalNum = element.SumCount
        item.PlanNum = 1
        item.Price = element.Price
        this.data.push(item)
      })
      this.$emit('input', [...this.data])
    },
    handleValChange(val, name, item) {
      item[name] = val
      this.$emit('input', [...this.data])
    }
  }
}
</script>