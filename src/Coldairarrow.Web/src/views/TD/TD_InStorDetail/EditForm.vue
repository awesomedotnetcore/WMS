<template>
  <a-modal title="入库" width="40%" :visible="visible" @ok="handleSubmit" @cancel="()=>{this.visible=false}">
    <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
      <a-form-model-item label="物料" prop="MaterialId">
        <materila-select v-model="entity.MaterialId" @select="handleMaterialSelect"></materila-select>
      </a-form-model-item>
      <a-form-model-item label="货位" prop="LocalId">
        <location-select v-model="entity.LocalId" @select="handleLocalIdSelect"></location-select>
      </a-form-model-item>
      <a-form-model-item label="托盘" prop="TrayId">
        <tray-select v-model="entity.TrayId" @select="handleTraySelect" :materialId="entity.MaterialId" :locartalId="entity.LocalId"></tray-select>
      </a-form-model-item>
      <a-form-model-item label="托盘分区ID" prop="ZoneId">
        <a-input v-model="entity.ZoneId" autocomplete="off" />
      </a-form-model-item>
      <a-form-model-item label="批次号" prop="BatchNo">
        <a-input v-model="entity.BatchNo" autocomplete="off" />
      </a-form-model-item>
      <a-form-model-item label="条码" prop="BarCode">
        <a-input v-model="entity.BarCode" autocomplete="off" />
      </a-form-model-item>
      <a-form-model-item label="单价" prop="Price">
        <a-input v-model="entity.Price" autocomplete="off" />
      </a-form-model-item>
      <a-form-model-item label="入库数量" prop="Num">
        <a-input v-model="entity.Num" autocomplete="off" />
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
export default {
  components: {
    MaterilaSelect,
    LocationSelect,
    TraySelect
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      entity: {},
      rules: {}
    }
  },
  mounted() {
    this.entity = this.value
  },
  methods: {
    openForm(entity) {
      this.entity = entity
      this.visible = true
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) return
        this.$emit('Submit', this.entity)
        this.visible = false
      })
    },
    handleMaterialSelect(material) {
      console.log('handleMaterialSelect', material)
    },
    handleLocalIdSelect(location) {
      console.log('handleLocalIdSelect', location)
    },
    handleTraySelect(tray) {
      console.log('handleTraySelect', tray)
    }
  }
}
</script>
