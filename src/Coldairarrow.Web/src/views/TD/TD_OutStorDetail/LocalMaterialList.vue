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
              type="number"
              @change="onCellChange(record, 'LocalNum', $event)"
            />
          </template>
          <!-- <span slot="action" slot-scope="text, record">
            <template>
              <a
                @click="handleSelectTarget(record.Id, record.MaterialId, record.ToLocalId, record.ToTrayId, record.ToZoneId)"
              >目标选择</a>
            </template>
          </span> -->
        </a-table>
      </a-form-model>
    </a-spin>

    <!-- <edit-target ref="editTarget" :parentObj="this"></edit-target> -->
  </a-modal>
</template>

<script>
import MaterilaSelect from '../../../components/Material/MaterialSelect'
import LocationSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
import EditableCell from '../../../components/EditableCell/EditableCell'
// import EditTarget from './EditTarget'
const columns = [
  { title: '货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '托盘分区', dataIndex: 'TrayZone.Name', width: '10%' },
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '库存数量', dataIndex: 'Num', width: '8%' },
  { title: '出库数量', dataIndex: 'LocalNum', width: '8%', scopedSlots: { customRender: 'LocalNum' } },
  // { title: '目标货位', dataIndex: 'ToLocalName', width: '10%' },
  // { title: '目标托盘', dataIndex: 'ToTrayName', width: '10%' },
  // { title: '目标分区', dataIndex: 'ToZoneName', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    MaterilaSelect,
    LocationSelect,
    TraySelect,
    ZoneSelect,
    EditableCell,
    // EditTarget
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
      material: null,
      location: null,
      tray: null,
      trayZone: null
      // moveId: null,
      // moveDetailId: null
    }
  },
  methods: {
    // init() {
    //   this.visible = true
    //   this.selectedRowKeys = []
    //   this.data = []
    //   this.$nextTick(() => {
    //     this.$refs['form'].clearValidate()
    //   })
    // },
    openForm(entity) {
          this.entity = entity
          this.visible = true
        },
    // openForm(moveId, title) {
    //   this.init()
    //   this.moveId = moveId
    // },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) return
        this.entity.Location = { ...this.location }
        this.entity.Material = { ...this.material }
        this.entity.Tray = { ...this.tray }
        this.entity.TrayZone = { ...this.trayZone }
        this.$emit('submit', { ...this.entity })
        this.visible = false
      })
    },
    // handleSubmit() {
    //   this.$refs['form'].validate(valid => {
    //     if (!valid) {
    //       return
    //     }
    //     this.loading = true
    //     this.$http.post('/TD/TD_MoveDetail/SaveDatas', this.selectedRows).then(resJson => {
    //       this.loading = false

    //       if (resJson.Success) {
    //         this.$message.success('操作成功!')
    //         this.visible = false

    //         this.parentObj.getDataList()
    //       } else {
    //         this.$message.error(resJson.Msg)
    //       }
    //     })
    //   })
    // },
    handleMaterialSelect(val) {
      this.queryParam.keyword = val.Id
      this.loading = true
      var thisObj = this
      this.$http
        .post('/IT/IT_LocalMaterial/GetDataListByMaterialId', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then(resJson => {
          this.loading = false
          var addData = {}
          resJson.Data.forEach(element => {
            addData = element
            addData.MoveDetailId = thisObj.MoveDetailId
            addData.FromLocalId = element.LocalId
            addData.FromTrayId = element.TrayId
            addData.FromZoneId = element.ZoneId
            addData.MoveId = thisObj.moveId
            if (this.getDataMatch == 0) {
              addData.ToLocalId = ''
              addData.ToLocalName = ''
              addData.ToTrayId = ''
              addData.ToTrayName = ''
              addData.ToZoneId = ''
              addData.ToZoneName = ''
              addData.LocalNum = 0
              addData.Amount = 0
            } else {
              this.getDataMatch = 0
            }
            thisObj.data.push(addData)
          })
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
    onCellChange(data, dataIndex, value) {
      data[dataIndex] = value
    },
    // handleSelectTarget(id, materialId, locationId, trayId, zoneId) {
    //   this.$refs.editTarget.openForm(id, materialId, locationId, trayId, zoneId)
    // },
    handleUpdataTargetInfo(id, locationId, locationName, trayId, trayName, zoneId, zoneName) {
      this.data.forEach(i => {
        if (i.Id == id) {
          i.ToLocalId = locationId
          i.ToLocalName = locationName
          i.ToTrayId = trayId
          i.ToTrayName = trayName
          i.ToZoneId = zoneId
          i.ToZoneName = zoneName
        }
      })
    }
  }
}
</script>
