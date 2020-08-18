<template>
  <a-card title="收货" :bordered="false" :loading="loading">
    <a-button slot="extra" type="primary" ghost @click="handlerSubmit" :loading="loading">确定</a-button>
    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span :style="{marginRight:'10px'}">条码:{{ item.Material.BarCode }}</span>
            <span :style="{marginRight:'10px'}">数量：{{ item.RecNum }}</span>
          </div>
          <div slot="description">
            <span :style="{marginRight:'10px'}">名称:{{ item.Material.Name }}</span>
            <span :style="{marginRight:'10px'}">编码:{{ item.Material.Code }}</span>
            <span :style="{marginRight:'10px'}">规格:{{ item.Material.Spec }}</span>
          </div>
        </a-list-item-meta>
        <a-button slot="actions" type="dashed" shape="circle" icon="delete" @click="handlerDel(item)"></a-button>
      </a-list-item>
      <a-list-item>
        <a-button type="primary" block @click="e=>{this.visible=true}" icon="plus">新增物料</a-button>
      </a-list-item>
    </a-list>
    <a-modal title="新增物料" :visible="visible" @cancel="e=>{this.visible=false}" @ok="handlerAdd">
      <a-spin :spinning="modalLoading">
        <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
          <a-form-model-item prop="BarCode">
            <input-material v-model="entity.BarCode"></input-material>
          </a-form-model-item>
          <a-form-model-item prop="Num">
            <a-input-number v-model="entity.Num" :style="{width:'100%'}" :min="1" placeholder="物料数量" />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </a-card>
</template>

<script>
import moment from 'moment'
import InputMaterial from '../../components/InputMaterial'
import MaterialSvc from '../../api/PB/MaterialSvc'
import ReceiveSvc from '../../api/TD/ReceiveSvc'
export default {
  components: {
    InputMaterial
  },
  data() {
    return {
      visible: false,
      loading: false,
      modalLoading: false,
      entity: {},
      listData: [],
      tempId: 0,
      rules: {
        BarCode: [{ required: true, message: '请输入物料编码', trigger: 'blur' }],
        Num: [{ required: true, message: '请输入物料数量', trigger: 'blur' }]
      }
    }
  },
  methods: {
    moment,
    handlerSubmit() {
      var listData = this.listData.map((v) => { v.Material = null; return v; });
      var data = {
        OrderTime: moment().toJSON(),
        RecTime: moment().toJSON(),
        Type: 'Purchase',
        Status: 0,
        RecDetails: listData
      }
      this.loading = true
      ReceiveSvc.SaveData(data).then(resJson => {
        this.loading = false
        if (resJson.Success) {
          this.$message.success(resJson.Msg)
          this.$router.push({ path: '/InStorage/ReceiveList' })
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    },
    handlerDel(item) {
      var index = this.listData.indexOf(item)
      this.listData.splice(index, 1)
    },
    handlerAdd() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.modalLoading = true
        MaterialSvc.GetByBarcode(this.entity.BarCode).then(resJson => {
          this.modalLoading = false
          if (!resJson.Data || !resJson.Success) {
            this.$message.error('物料不存在')
            return
          } else {
            var material = resJson.Data
            var item = {
              Id: 'newid_' + (this.tempId + 1).toString(),
              MaterialId: material.Id,
              MeasureId: material.MeasureId,
              PlanNum: this.entity.Num,
              RecNum: this.entity.Num,
              Price: material.Price,
              Material: material
            }
            this.listData.push(item)
            this.visible = false
            this.tempId += 1
            this.entity = {}
          }
        })
      })
    }
  }
}
</script>

<style>
</style>