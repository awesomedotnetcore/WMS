<template>
  <a-card title="手动空托盘出库">
    <a-button slot="extra" type="primary" ghost @click="handlerSubmit" :loading="loading">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="TrayCode">
        <input-tray v-model="entity.TrayCode"></input-tray>
      </a-form-model-item>
    </a-form-model>

    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">  托盘号:{{ item.Code }}</span>            
          </div>     
          <div slot="title">
            <span class="spanTag" v-if="item.LocalId" >库位编号:{{ item.PB_Location.Code }}</span>
            <span class="spanTag" v-else >库位编号: Null</span>
          </div>   
        </a-list-item-meta>
        <a-button type="link" slot="actions" >已出库</a-button>
      </a-list-item>
    </a-list> 

  </a-card>
</template>

<script>
import InputTray from '../../components/InputTray'
import TraySvc from '../../api/PB/TraySvc'

export default {
  components: {
   InputTray
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        LocalCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }],
      },
      listData: [],
      listinData: []
    }
  },
  mounted() {
   // this.getTrayTypeList()
  },
  methods: {    
      handlerSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        TraySvc.OutManualTray(this.entity).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.listData=[]  
          this.listData.push(resJson.Data) 
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    },
    
  }
}
</script>