<template>
  <a-card title="手动空托盘入库">
    <a-button slot="extra" type="primary" ghost @click="handlerSubmit" :loading="loading">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="TrayCode">
        <input-tray v-model="entity.TrayCode"></input-tray>
      </a-form-model-item>
      <a-form-model-item prop="LocalCode">
        <input-location v-model="entity.LocalCode"></input-location>
      </a-form-model-item>
    </a-form-model>

    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">  托盘号:{{ item.Code }}</span>            
          </div>       
          <div slot="title">
            <span class="spanTag">  货位编号:{{ item.PB_Location.Code }}</span>            
          </div> 
        </a-list-item-meta>
        <a-button type="link" slot="actions" >已入库</a-button>
      </a-list-item>
    </a-list> 

  </a-card>
</template>

<script>
import InputTray from '../../components/InputTray'
import InputLocation from '../../components/InputLocation'
import TraySvc from '../../api/PB/TraySvc'

export default {
  components: {
   InputTray,
   InputLocation
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        LocalCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }],
      },
      // queryData: {
      //   Search: { Keyword: null }
      // },
      listData: []
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
        TraySvc.InManualTray(this.entity).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.listData=[]  
          this.listData.push(resJson.Data) 
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>