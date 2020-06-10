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
        <!-- <a-form-model-item label="目标仓库" prop="ToStorId">
          <storage-select v-model="entity.ToStorId" @select="handleStorageSelect"></storage-select>
        </a-form-model-item>-->
        <a-form-model-item label="现有库存" prop="Remarks">
          <a-input v-model="localMaterial.Num" :disabled="true" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="调拨数量" prop="Remarks">
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
import StorageSelect from '../../../components/Storage/StorageSelect'

export default {
  components: {
    StorageSelect
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
      this.materialId = null
    },
    openForm(id, LocalId, TrayId, ZoneId, MaterialId, BatchNo, BarCode, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_AllocateDetail/GetTheData', { id: id }).then(resJson => {
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
      } else if (this.entity.ToStorId == this.localMaterial.StorId) {
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
    handleStorageSelect(data) {
      this.entity.ToStorId = data.Id
    }
  }
}
</script>
