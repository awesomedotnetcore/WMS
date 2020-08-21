<template>
  <a-card title="空托盘出库">    
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form"> 
      <a-row :gutter="16">
        <a-col :span="18">
          <a-form-model-item prop="TrayTypeId">
              <a-select v-model="entity.TrayTypeId" placeholder="托盘类型">
                <a-select-option v-for="item in this.TrayTypeList" :key="item.Id">{{ item.Name }}</a-select-option>
              </a-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="4">
          <a-form-model-item >
            <a-button slot="extra" type="primary" @click="handlerSubmit" :loading="loading">出库</a-button>  
          </a-form-model-item>
        </a-col>
      </a-row>           
    </a-form-model>  

    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">  托盘号:{{ item.Code }}</span>            
          </div>
          <div slot="title">
            <span class="spanTag">  库位编号:{{ item.PB_Location.Code }}</span>
          </div>          
        </a-list-item-meta>
      </a-list-item>
    </a-list>  

  </a-card>
</template>

<script>
import OutStorageSvc from '../../api/TD/OutStorageSvc'
import TraySvc from '../../api/PB/TraySvc'

export default {
  components: {
   // EnumSelect
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        TrayTypeId: [{ required: true, message: '请选择托盘类型', trigger: 'change' }],
      },
      TrayTypeList: [],
      listData: []
    }
  },
  mounted() {
    this.getTrayTypeList()
  },
  methods: {
    getTrayTypeList() {
      var thisObj = this
      this.loading = true
      OutStorageSvc.OutTrayType().then(resJson => {
        this.loading = false
        thisObj.TrayTypeList = resJson.Data
      })
    },
    handlerSubmit() {      
        this.loading = true
        OutStorageSvc.OutAutoTray(this.entity).then(resJson => {          
          this.loading = false
          if (resJson.Success) {
            this.listData=[]            
            this.listData.push(resJson.Data)        
          } else {
            this.$message.error(resJson.Msg)
          }
        })
    }
  }
}
</script>