<template>
  <a-card title="空托盘出库">    
     <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">   
      <a-row :gutter="16">
        <a-col :span="18">
          <a-form-model-item > 
          <a-select placeholder="托盘类型" buttonStyle="solid" @select="DataTypeChange">
            <a-select-option value="0" key="0">标准托盘</a-select-option>
            <a-select-option value="1" key="1">原料托盘</a-select-option>
            <a-select-option value="2" key="2">异形托盘</a-select-option>
            <a-select-option value="3" key="3">其他托盘</a-select-option>
          </a-select> 
          </a-form-model-item>  
        </a-col>
        <a-col :span="4">
          <a-form-model-item >
            <a-button slot="extra" type="primary" >出库</a-button>  
          </a-form-model-item>
        </a-col>
      </a-row>        
    </a-form-model>   
  </a-card>
</template>

<script>


export default {
  components: {

  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {

      }
    }
  },
  methods: {
    handleMenuClick(e) {
      console.log('click', e);
    },
    DataTypeChange(val, option) {
      this.DataType = val
    },
    // handlerSubmit() {
    //   this.$refs['form'].validate(valid => {
    //     if (!valid) {
    //       return
    //     }
    //     this.loading = true
    //     OutStorageSvc.ProductOut(this.entity).then(resJson => {
    //       this.loading = false
    //       if (resJson.Success) {
    //         this.$message.success(resJson.Msg)            
    //       } else {
    //         this.$message.error(resJson.Msg)
    //       }
    //     })
    //   })
    // }
  }
}
</script>