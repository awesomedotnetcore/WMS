<template>
  <a-modal
    :title="title"
    width="40%"
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
        <!-- <a-form-model-item label="原货位" prop="FromLocalId">
          <location-select :materialId="this.SelectMaterialId" v-model="entity.FromLocalId" @select="handleSrcLocalSelect"></location-select>
        </a-form-model-item> -->
        <!-- <a-form-model-item label="原托盘" prop="FromTrayId">
          <tray-select
            v-model="entity.FromTrayId"
            @select="handleSrcTraySelect"
            :materialId="this.SelectMaterialId"
            :locartalId="this.SelectSrcLocalId"
          ></tray-select>
        </a-form-model-item> -->
        <!-- <a-form-model-item label="原托盘分区" prop="FromZoneId">
          <zone-select :trayId="this.SelectSrcTrayId" v-model="entity.FromZoneId"></zone-select>
        </a-form-model-item> -->
        <!-- <a-form-model-item label="目标货位" prop="ToLocalId">
          <location-select v-model="entity.ToLocalId" @select="handleTarLocalSelect"></location-select>
        </a-form-model-item> -->
        <!-- <a-form-model-item label="目标托盘" prop="ToTrayId">
          <tray-select
            v-model="entity.ToTrayId"
            @select="handleTarTraySelect"
            :materialId="this.SelectMaterialId"
            :locartalId="this.SelectTarLocalId"
          ></tray-select>
        </a-form-model-item> -->
        <!-- <a-form-model-item label="目标托盘分区" prop="ToZoneId">
          <zone-select :trayId="this.SelectTarTrayId" v-model="entity.ToZoneId"></zone-select>
        </a-form-model-item> -->
        <a-form-model-item label="条码" prop="BarCode">
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
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import MaterilaSelect from '../../../components/Material/MaterialSelect'
import LocationSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
export default {
  components: {
    MaterilaSelect,
    LocationSelect,
    TraySelect,
    ZoneSelect
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
      SelectMaterialId: null,
      SelectSrcLocalId: null,
      SelectTarLocalId: null,
      SelectSrcTrayId: null,
      SelectTarTrayId: null
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
          this.SelectMaterialId = this.entity.MaterialId
          this.SelectSrcLocalId = this.entity.FromLocalId
          this.SelectTarLocalId = this.entity.ToLocalId
          this.SelectSrcTrayId = this.entity.FromTrayId
          this.SelectTarTrayId = this.entity.ToTrayId
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
      this.SelectMaterialId = val.Id
    },
    handleSrcLocalSelect(val) {
      // this.SelectSrcLocalId = val
    },
    handleSrcTraySelect(val) {
      // this.SelectSrcTrayId = val
    },
    handleTarLocalSelect(val) {
      // this.SelectTarLocalId = val
    },
    handleTarTraySelect(val) {
      // this.SelectTarTrayId = val
    }
  }
}
</script>
