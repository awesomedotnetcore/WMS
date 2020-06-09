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
        <a-form-model-item label="调拨单号" prop="Code">
          <a-input v-model="entity.Code" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="调拨类型" prop="Type">
          <enum-select code="AllocateStorageType" v-model="entity.Type"></enum-select>
        </a-form-model-item>
        <!-- <a-form-model-item label="目标仓库" prop="ToStorId">
          <storage-select v-model="entity.ToStorId" @select="handleStorageSelect"></storage-select>
        </a-form-model-item> -->
        <a-form-model-item label="关联单号" prop="RefCode">
          <a-input v-model="entity.RefCode" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="调拨时间" prop="AllocateTime">
          <a-date-picker show-time v-model="entity.AllocateTime"/>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="Remarks">
          <a-input v-model="entity.Remarks" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
import StorageSelect from '../../../components/Storage/StorageSelect'
import EnumSelect from '../../../components/BaseEnum/BaseEnumSelect'

export default {
  components: {
    StorageSelect,
    EnumSelect
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
      title: ''
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
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/TD/TD_Allocate/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/TD/TD_Allocate/SaveData', this.entity).then(resJson => {
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
    getEquList() {},
    handleStorageSelect(data) {
      this.entity.ToStorId = data.Id
    }
  }
}
</script>
