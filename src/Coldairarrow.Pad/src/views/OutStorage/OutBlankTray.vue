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

          <!-- <a-form-model-item>
          <enum-select code="TrayType" v-model="entity.TrayTypeCode" ></enum-select>
          {{entity.TrayTypeCode}}
        </a-form-model-item> -->


          <!-- <a-select placeholder="托盘类型" v-model="entity.TrayType"  buttonStyle="solid" @select="DataTypeChange">
            <a-select-option value="0" key="0">标准托盘</a-select-option>
            <a-select-option value="1" key="1">原料托盘</a-select-option>
            <a-select-option value="2" key="2">异形托盘</a-select-option>
            <a-select-option value="3" key="3">其他托盘</a-select-option>
          </a-select>  -->
          <!-- </a-form-model-item>   -->
        </a-col>
        <a-col :span="4">
          <a-form-model-item >
            <a-button slot="extra" type="primary" @click="handlerSubmit" :loading="loading">出库</a-button>  
          </a-form-model-item>
        </a-col>
      </a-row>        
    </a-form-model>   
  </a-card>
</template>

<script>
import OutStorageSvc from '../../api/TD/OutStorageSvc'
//import EnumSelect from '../../components/BaseEnumSelect'

export default {
  components: {
   // EnumSelect
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        TrayType: [{ required: true, message: '请选择托盘类型', trigger: 'change' }],
      },
      TrayTypeList: []
     // queryData: {
        //PageIndex: 1, PageRows: 10, SortField: 'Id', SortType: 'desc',
       // Search: { }
     // },
      //TrayTypeList: []
    }
  },
  mounted() {
    this.getTrayTypeList()
  },
  methods: {
    // handleMenuClick(e) {
    //   console.log('click', e);
    // },
    getTrayTypeList() {
      var thisObj = this
      this.loading = true
      OutStorageSvc.OutTrayType().then(resJson => {
        this.loading = false
        thisObj.TrayTypeList = resJson.Data
      })
    },
    handlerSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }        
        this.loading = true
        OutStorageSvc.OutAutoTray(this.entity).then(resJson => {
          
          this.loading = false
          if (resJson.Success) {
            this.$message.success(resJson.Msg)            
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>