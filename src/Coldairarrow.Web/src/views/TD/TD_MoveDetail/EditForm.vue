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
        <a-form-model-item label="目标货位" prop="ToLocalId">
          <location-select v-model="entity.ToLocalId" @select="handleLocalSelect"></location-select>
        </a-form-model-item>
        <a-form-model-item label="目标托盘" prop="ToTrayId">
          <tray-select
            v-model="entity.ToTrayId"
            @select="handleTraySelect"
            :materialId="entity.MaterialId"
            :locartalId="entity.ToLocalId"
          ></tray-select>
        </a-form-model-item>
        <a-form-model-item label="目标托盘分区" prop="ToZoneId">
          <zone-select
            :trayId="entity.ToTrayId"
            @select="handleZoneSelect"
            v-model="entity.ToZoneId"
          ></zone-select>
        </a-form-model-item>
        <a-form-model-item label="现有库存" prop="Remarks">
          <a-input v-model="localMaterial.Num" :disabled="true" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="移库数量" prop="Remarks">
          <a-input-number
            v-model="entity.LocalNum"
            :max="localMaterial.Num"
            :min="0"
            autocomplete="off"
          />
        </a-form-model-item>
      </a-form-model>
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
      localMaterial: {},
      rules: {},
      title: '',
      moveId: null,
      materialId: '',
      LocalId: null,
      TrayId: null,
      ZoneId: null,
      MaterialId: null,
      BatchNo: null,
      BarCode: null
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.moveId = null
      this.materialId = null
    },
    openForm(moveId, id, LocalId, TrayId, ZoneId, MaterialId, BatchNo, BarCode, title) {
      this.init()
      this.moveId = moveId

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_MoveDetail/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
        this.loading = true
        this.$http
          .post('/IT/IT_LocalMaterial/GetTheLocalMaterial', {
            LocalId: LocalId,
            TrayId: TrayId,
            ZoneId: ZoneId,
            MaterialId: MaterialId,
            BatchNo: BatchNo,
            BarCode: BarCode
          })
          .then(resJson => {
            this.loading = false

            this.localMaterial = resJson.Data
          })
      }
    },
    handleSubmit() {
      this.loading = true
      if (this.entity.LocalNum > this.localMaterial.Num) {
        this.$message.error('超过现有库存!')
        this.loading = false
      } else if (
        this.entity.ToLocalId == this.localMaterial.LocalId &&
        this.entity.ToTrayId == this.localMaterial.TrayId &&
        this.entity.ToZoneId == this.localMaterial.ZoneId
      ) {
        this.$message.error('地址错误：目标地址与原地址一致!')
        this.loading = false
      } else {
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
      }
    },
    handleLocalSelect(data) {
      this.entity.ToLocalId = data.Id
    },
    handleTraySelect(data) {
      this.entity.ToTrayId = data.Id
    },
    handleZoneSelect(data) {
      this.entity.ToZoneId = data.Id
    }
  }
}
</script>
