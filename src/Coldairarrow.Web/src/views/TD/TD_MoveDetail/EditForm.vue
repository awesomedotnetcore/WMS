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
          <materila-select v-model="entity.MaterialId" @select="getSrcLocalList"></materila-select>
        </a-form-model-item>
        <a-form-model-item label="原货位ID" prop="FromLocalId">
          <a-input v-model="entity.FromLocalId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原托盘分区" prop="FromZoneId">
          <a-input v-model="entity.FromZoneId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="原托盘ID" prop="FromTrayId">
          <a-input v-model="entity.FromTrayId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标货位ID" prop="ToLocalId">
          <a-input v-model="entity.ToLocalId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标托盘ID" prop="ToTrayId">
          <a-input v-model="entity.ToTrayId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="目标托盘分区" prop="ToZoneId">
          <a-input v-model="entity.ToZoneId" autocomplete="off" />
        </a-form-model-item>
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
export default {
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
      materialList: [],
      srcLocalList: [],
      srcTrayList: [],
      srcZoneList: [],
      tarLocalList: [],
      tarTrayList: [],
      tarZoneList: []
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.materialList = []
      this.srcLocalList = []
      this.srcTrayList = []
      this.srcZoneList = []
      this.tarLocalList = []
      this.tarTrayList = []
      this.tarZoneList = []
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
    getSrcLocalList() {
      this.srcLocalList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    },
    getSrcTrayList() {
      this.srcTrayList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    },
    getSrcZoneList() {
      this.srcZoneList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    },
    getTarLocalList() {
      this.tarLocalList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    },
    getTarTrayList() {
      this.tarTrayList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    },
    getTarZoneList() {
      this.tarZoneList = []
      this.loading = true
      this.$http.post('/PB/PB_Material/GetStorageMaterials').then(resJson => {
        this.loading = false

        this.materialList = resJson.Data
      })
    }
  }
}
</script>
