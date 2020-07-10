<template>
  <a-modal title="入库" width="60%" :visible="visible" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-row>
        <a-col :span="12">
          <a-form-model-item label="物料" prop="MaterialId">
            <materila-select v-model="entity.MaterialId" @select="handleMaterialSelect"></materila-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="货位" prop="LocalId">
            <location-select v-model="entity.LocalId" :storid="storage.Id" @select="handleLocalIdSelect"></location-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row v-if="storage.IsTray || storage.IsZone">
        <a-col :span="12">
          <a-form-model-item v-if="storage.IsTray" label="托盘" prop="TrayId">
            <tray-select v-model="entity.TrayId" @select="handleTraySelect" :materialId="entity.MaterialId" :locartalId="entity.LocalId"></tray-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item v-if="storage.IsTray && storage.IsZone" label="托盘分区" prop="ZoneId">
            <zone-select :trayId="entity.TrayId" v-model="entity.ZoneId" @select="handleZoneSelect"></zone-select>
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="12">
          <a-form-model-item label="批次号" prop="BatchNo">
            <a-input v-model="entity.BatchNo" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="条码" prop="BarCode">
            <a-input v-model="entity.BarCode" autocomplete="off" />
          </a-form-model-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="12">
          <a-form-model-item label="单价" prop="Price">
            <a-input-number v-model="entity.Price" :style="{width:'100%'}" autocomplete="off" />
          </a-form-model-item>
        </a-col>
        <a-col :span="12">
          <a-form-model-item label="入库数量" prop="Num">
            <a-input-number v-model="entity.Num" :style="{width:'100%'}" autocomplete="off" />
          </a-form-model-item>
        </a-col>
      </a-row>
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
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      storage: {},
      entity: { MaterialId: '' },
      rules: {
        MaterialId: [{ required: true, message: '请选择物料', trigger: 'change' }],
        LocalId: [{ required: true, message: '请选择货位', trigger: 'change' }],
        // TrayId: [{ required: true, message: '请选择托盘', trigger: 'change' }],
        // ZoneId: [{ required: true, message: '请选择托盘分区', trigger: 'change' }],
        Num: [{ required: true, message: '请输入入库数量', trigger: 'blur' }]
      },
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
      this.location = entity.Location
      this.material = entity.Material
      this.tray = entity.Tray
      this.trayZone = entity.TrayZone
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
      this.entity.Price = material.Price
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
