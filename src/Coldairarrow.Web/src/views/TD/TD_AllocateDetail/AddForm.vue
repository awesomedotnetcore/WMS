<template>
  <a-modal
    :title="title"
    width="90%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="物料" prop="MaterialId">
          <materila-select v-model="entity.MaterialId" @select="handleMaterialSelect"></materila-select>
        </a-form-model-item>
        <a-table
          ref="table"
          :columns="columns"
          :rowKey="row => row.Id"
          :dataSource="data"
          :pagination="pagination"
          :loading="loading"
          :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
          :bordered="true"
          size="small"
        >
          <template slot="LocalNum" slot-scope="text, record">
            <editable-cell
              :text="text"
              :max="record.Num"
              :min="0"
              type="number"
              @change="onCellChange(record, 'LocalNum', $event)"
            />
          </template>
          <span slot="action" slot-scope="text, record">
            <template>
              <a
                @click="handleSelectTarget(record.Id, record.MaterialId, record.FromStorId, record.FromLocalId, record.FromTrayId, record.FromZoneId, record.ToLocalId, record.ToTrayId, record.ToZoneId)"
              >目标选择</a>
            </template>
          </span>
        </a-table>
      </a-form-model>
    </a-spin>

    <edit-target ref="editTarget" :parentObj="this"></edit-target>
  </a-modal>
</template>

<script>
import MaterilaSelect from '../../../components/Material/MaterialSelect'
import LocationSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
import EditableCell from '../../../components/EditableCell/EditableCell'
import EditTarget from './EditTarget'

const columns = [
  { title: '原货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '原托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '原分区', dataIndex: 'TrayZone.Name', width: '10%' },
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '5%' },
  { title: '移库数量', dataIndex: 'LocalNum', width: '5%', scopedSlots: { customRender: 'LocalNum' } }
  // { title: '目标仓库', dataIndex: 'ToStorageName', width: '10%' }
  // { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    MaterilaSelect,
    LocationSelect,
    TraySelect,
    ZoneSelect,
    EditableCell,
    EditTarget
  },
  props: {
    parentObj: Object
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: {},
      rules: {},
      title: '',
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      sorter: { field: 'Id', order: 'asc' },
      filters: {},
      queryParam: {},
      columns,
      data: [],
      selectedRowKeys: [],
      tarGetInfo: [],
      updataMatch: 0,
      getDataMatch: 0,
      selectedRows: [],
      AllocateId: null,
      AllocateDetailId: null,
      toStorId: null,
      toStorName: null
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.selectedRowKeys = []
      this.data = []
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(allocateId, id) {
      this.init()
      this.AllocateId = allocateId
      // this.toStorId = toStorId
      // this.toStorName = toStorName
    },
    handleSubmit() {
      var thisObj = this
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        thisObj.loading = true
        if (thisObj.selectedRows.length > 0) {
          thisObj.$http.post('/TD/TD_AllocateDetail/SaveDatas', thisObj.selectedRows).then(resJson => {
            thisObj.loading = false

            if (resJson.Success) {
              thisObj.$message.success('操作成功!')
              thisObj.visible = false

              thisObj.parentObj.getDataList()
            } else {
              thisObj.$message.error(resJson.Msg)
            }
          })
        } else {
          thisObj.loading = false
          thisObj.$confirm({
            title: '未选择保存数据，是否关闭页面？',
            onOk() {
              thisObj.visible = false
            }
          })
        }
      })
    },
    handleMaterialSelect(val) {
      this.queryParam.keyword = val.Id
      this.loading = true
      var thisObj = this
      this.$http
        .post('/IT/IT_LocalMaterial/GetDataListByMaterialId', {
          PageIndex: thisObj.pagination.current,
          PageRows: thisObj.pagination.pageSize,
          SortField: thisObj.sorter.field || 'Id',
          SortType: thisObj.sorter.order,
          Search: thisObj.queryParam,
          ...thisObj.filters
        })
        .then(resJson => {
          thisObj.loading = false
          var addData = {}
          resJson.Data.forEach(element => {
            addData = element
            addData.AllocateId = thisObj.AllocateId
            addData.AllocateDetailId = thisObj.AllocateDetailId
            addData.FromStorId = element.StorId
            addData.FromLocalId = element.LocalId
            addData.FromTrayId = element.TrayId
            addData.FromZoneId = element.ZoneId
            if (this.getDataMatch == 0) {
              // addData.ToStorId = this.toStorId
              // addData.ToStorName = this.toStorName
              // addData.ToLocalId = ''
              // addData.ToLocalName = ''
              addData.LocalNum = 0
              addData.Amount = 0
            } else {
              thisObj.getDataMatch = 0
            }
            thisObj.data.push(addData)
          })
          const pagination = { ...thisObj.pagination }
          pagination.total = resJson.Total
          thisObj.pagination = pagination
        })
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    onCellChange(data, dataIndex, value) {
      data[dataIndex] = value
    },
    handleSelectTarget(id, materialId, FromStorageId, FromLocalId, FromTrayId, FromZoneId, ToStorageId) {
      this.$refs.editTarget.openForm(id, materialId, FromStorageId, FromLocalId, FromTrayId, FromZoneId, ToStorageId)
    },
    handleUpdataTargetInfo(id, ToStorageId, ToStorageName) {
      this.data.forEach(i => {
        if (i.Id == id) {
          i.ToStorId = ToStorageId
          i.ToStorName = ToStorageName
        }
      })
    }
  }
}
</script>
