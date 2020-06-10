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
        <a-form-model-item label="目标仓库" prop="ToStorId">
          <storage-select v-model="ToStorageId" @select="handleStorageSelect"></storage-select>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import StorageSelect from '../../../components/Storage/StorageSelect'
import LocationSelect from '../../../components/Location/LocationSelect'
export default {
  components: {
    StorageSelect,
    LocationSelect
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
      ToStorageId: '',
      ToStorageName: '',
      moveDetailId: '',
      materialId: '',
      localMaterialId: '',
      FromStorageId: '',
      FromLocalId: '',
      FromTrayId: '',
      FromZoneId: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.ToLocalId = null
      this.ToStorageId = null
      this.materialId = null
      this.localMaterialId = null
    },
    openForm(id, materialId, FromStorageId, FromLocalId, FromTrayId, FromZoneId, ToStorageId) {
      this.init()
      this.localMaterialId = id
      this.materialId = materialId
      this.ToStorageId = ToStorageId
      this.FromStorageId = FromStorageId
      this.FromLocalId = FromLocalId
      this.FromTrayId = FromTrayId
      this.FromZoneId = FromZoneId
    },
    handleSubmit() {
      if (this.FromStorageId == this.ToStorageId) {
        this.$message.error('地址错误：目标地址与原地址一致!')
      } else {
        this.visible = false
        this.parentObj.handleUpdataTargetInfo(
          this.localMaterialId,
          this.ToStorageId,
          this.ToStorageName
        )
      }
    },
    handleLocalSelect(data) {
      this.ToLocalId = data.Id
      this.ToLocalName = data.Name
    },
    handleStorageSelect(data) {
      this.ToStorageId = data.Id
      this.ToStorageName = data.Name
    }
  }
}
</script>
