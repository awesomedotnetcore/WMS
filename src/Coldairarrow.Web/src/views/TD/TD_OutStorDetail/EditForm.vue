<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="物料" prop="MaterialId">
        <materila-select v-model="entity.MaterialId" @select="handleMaterialSelect"></materila-select>
        </a-form-model-item>
        <a-form-model-item label="货位" prop="LocalId">
          <location-select v-model="entity.LocalId" @select="handleLocalIdSelect"></location-select>
        </a-form-model-item>
        <a-form-model-item v-if="storage.IsTray" label="托盘" prop="TrayId">
          <tray-select v-model="entity.TrayId" @select="handleTraySelect" :materialId="entity.MaterialId" :locartalId="entity.LocalId"></tray-select>
        </a-form-model-item>
        <a-form-model-item v-if="storage.IsTray && storage.IsZone" label="托盘分区" prop="ZoneId">
          <zone-select :trayId="entity.TrayId" v-model="entity.ZoneId" @select="handleZoneSelect"></zone-select>
        </a-form-model-item>
        <a-form-model-item label="条码" prop="BarCode">
          <a-input v-model="entity.BarCode" autocomplete="off" />
        </a-form-model-item>        
        <a-form-model-item label="批次号" prop="BatchNo">
          <a-input v-model="entity.BatchNo" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="出库数量" prop="LocalNum">
          <a-input v-model="entity.LocalNum" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单价" prop="Price">
          <a-input v-model="entity.Price" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="总额" prop="TotalAmt">
          <a-input v-model="entity.TotalAmt" autocomplete="off" />
        </a-form-model-item>        
      </a-form-model>
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
  // props: {
  //   parentObj: Object
  // },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      entity: {MaterialId: '' },
      rules: {},
      storage: {},
      material: null,
      location: null,
      tray: null,
      trayZone: null
    }
  },
  mounted() {
    this.getCurStorage()
  },
  methods: {
    openForm(entity) {
      this.entity = entity
      this.visible = true
    },
    getCurStorage() {
      this.$http.get('/PB/PB_Storage/GetCurStorage')
        .then(resJson => {
          this.storage = resJson.Data
        })
    },    
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
    handleMaterialSelect(material) {
      console.log('handleMaterialSelect', material)
      this.material = material
    },
    handleLocalIdSelect(location) {
      console.log('handleLocalIdSelect', location)
      this.location = location
    },
    handleTraySelect(tray) {
      console.log('handleTraySelect', tray)
      this.tray = tray
    },
    handleZoneSelect(zone) {
      console.log('handleZoneSelect', zone)
      this.trayZone = zone
    }
  }
}
</script>
