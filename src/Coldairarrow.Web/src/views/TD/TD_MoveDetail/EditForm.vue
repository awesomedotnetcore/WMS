<template>
  <a-modal
    :title="title"
    width="85%"
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
          <template slot="Value" slot-scope="text, record">
            <editable-cell
              :text="text"
              type="number"
              @change="onCellChange(record, 'Value', $event)"
            />
          </template>
        </a-table>
        <!-- <a-form-model-item label="原货位" prop="FromLocalId">
          <location-select :materialId="this.SelectMaterialId" v-model="entity.FromLocalId" @select="handleSrcLocalSelect"></location-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="原托盘" prop="FromTrayId">
          <tray-select
            v-model="entity.FromTrayId"
            @select="handleSrcTraySelect"
            :materialId="this.SelectMaterialId"
            :locartalId="this.SelectSrcLocalId"
          ></tray-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="原托盘分区" prop="FromZoneId">
          <zone-select :trayId="this.SelectSrcTrayId" v-model="entity.FromZoneId"></zone-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="目标货位" prop="ToLocalId">
          <location-select v-model="entity.ToLocalId" @select="handleTarLocalSelect"></location-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="目标托盘" prop="ToTrayId">
          <tray-select
            v-model="entity.ToTrayId"
            @select="handleTarTraySelect"
            :materialId="this.SelectMaterialId"
            :locartalId="this.SelectTarLocalId"
          ></tray-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="目标托盘分区" prop="ToZoneId">
          <zone-select :trayId="this.SelectTarTrayId" v-model="entity.ToZoneId"></zone-select>
        </a-form-model-item>-->
        <!-- <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="批次号" prop="BatchNo">
          <a-input v-model="entity.BatchNo" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="移库数量" prop="LocalNum">
          <a-input v-model="entity.LocalNum" autocomplete="off" />
        </a-form-model-item>-->
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import MaterilaSelect from '../../../components/Material/MaterialSelect'
import LocationSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
import EditableCell from '../../../components/EditableCell/EditableCell'
const columns = [
  { title: '原货位', dataIndex: 'Location.Name', width: '10%' },
  { title: '原托盘', dataIndex: 'Tray.Name', width: '10%' },
  { title: '原分区', dataIndex: 'TrayZone.Name', width: '10%' },
  { title: '物料', dataIndex: 'Material.Name', width: '10%' },
  { title: '单位', dataIndex: 'Measure.Name', width: '5%' },
  { title: '批次号', dataIndex: 'BatchNo', width: '10%' },
  { title: '数量', dataIndex: 'Num', width: '5%' },
  { title: '移库数量', dataIndex: 'LocalNum', width: '5%', scopedSlots: { customRender: 'Value' } },
  { title: '目标货位', dataIndex: 'ToLocalName', width: '10%' },
  { title: '目标托盘', dataIndex: 'ToTrayName', width: '10%' },
  { title: '目标分区', dataIndex: 'ToZoneName', width: '10%' }
]

export default {
  components: {
    MaterilaSelect,
    LocationSelect,
    TraySelect,
    ZoneSelect,
    EditableCell
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
      selectedRowKeys: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(moveId, id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_MoveDetail/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      } else {
        this.entity.MoveId = moveId
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_MoveDetail/SaveData', this.entity).then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false

            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    },
    handleMaterialSelect(val) {
      this.queryParam.keyword = val.Id
      this.selectedRowKeys = []
      this.data = []
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
            addData.ToLocalId = null
            addData.ToLocalName = null
            addData.ToTrayId = null
            addData.ToTrayName = null
            addData.ToZoneId = null
            addData.ToZoneName = null
            addData.LocalNum = 0
            addData.Amount = 0
            thisObj.data.push(addData)
          })
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    onCellChange(data, dataIndex, value) {
      data[dataIndex] = value
    }
  }
}
</script>
