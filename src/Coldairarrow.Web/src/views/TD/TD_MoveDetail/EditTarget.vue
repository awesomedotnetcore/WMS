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
      <a-form-model-item label="目标货位" prop="ToLocalId">
        <location-select v-model="ToLocalId" @select="handleLocalSelect"></location-select>
      </a-form-model-item>
      <a-form-model-item label="目标托盘" prop="ToTrayId">
        <tray-select
          v-model="ToTrayId"
          @select="handleTraySelect"
          :materialId="this.materialId"
          :locartalId="this.ToLocalId"
        ></tray-select>
      </a-form-model-item>
      <a-form-model-item label="目标托盘分区" prop="ToZoneId">
        <zone-select :trayId="this.ToTrayId" @select="handleZoneSelect" v-model="ToZoneId"></zone-select>
      </a-form-model-item>
    </a-spin>
  </a-modal>
</template>

<script>
import LocationSelect from '../../../components/Location/LocationSelect'
import TraySelect from '../../../components/Tray/TraySelect'
import ZoneSelect from '../../../components/Tray/ZoneSelect'
export default {
  components: {
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
      ToLocalId: '',
      ToLocalName: '',
      ToTrayId: '',
      ToTrayName: '',
      ToZoneId: '',
      ToZoneName: '',
      moveDetailId: '',
      materialId: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.ToLocalId = null
      this.ToTrayId = null
      this.ToZoneId = null
      this.materialId = null
    },
    openForm(id, materialId, locationId, trayId, zoneId) {
      this.init()
      this.moveDetailId = id
      this.materialId = materialId
      this.ToLocalId = locationId
      this.ToTrayId = trayId
      this.ToZoneId = zoneId
    },
    handleSubmit() {
      this.visible = false
      this.parentObj.handleUpdataTargetInfo(
        this.moveDetailId,
        this.ToLocalId,
        this.ToLocalName,
        this.ToTrayId,
        this.ToTrayName,
        this.ToZoneId,
        this.ToZoneName
      )
    },
    handleLocalSelect(data) {
      this.ToLocalId = data.Id
      this.ToLocalName = data.Name
    },
    handleTraySelect(data) {
      this.ToTrayId = data.Id
      this.ToTrayName = data.Name
    },
    handleZoneSelect(data) {
      this.ToZoneId = data.Id
      this.ToZoneName = data.Name
    }
  }
}
</script>
