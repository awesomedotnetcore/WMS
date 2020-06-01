<template>
  <div>
    <a-row>
        <a-col :span="20">
          <a-select v-model="curValue" placeholder="客户/投料点" @select="handleSelected" v-bind="$attrs">
            <a-select-option v-for="item in customerList" :key="item.Id" :value="item.Id">{{ item.Name }}</a-select-option>
          </a-select>
        </a-col>
        <a-col :span="2">
          <a-button type="primary" @click="handleOpenChoose">
            <a-icon type="search" />
          </a-button>
        </a-col>
      </a-row>  
      <customer-choose ref="customerChoose" @onChoose="handleChoose"></customer-choose>
  </div>  
</template>
<script>
import CustomerChoose from './CustomerChoose'

export default {
  props: {
    value: { type: String, required: false }
  },
  components:{
    CustomerChoose
  },
  data() {
    return {
      curValue: '',
      customerList: []
    }
  },
  watch: {
    value(value) {
      this.getCustomerData(value)      
    }
  },
  methods: {
    getCustomerData(cid) {
      if (cid !== '' && cid !== undefined && cid !== null) {
        this.$http.post('/PB/PB_Customer/GetTheData', { id: cid }).then(resJson => {
          this.customerList.push(resJson.Data)
          this.curValue = this.value
        })
      }
    },
    handleSelected(val) {
      this.$emit('input', val)
    },
    handleOpenChoose(){
      this.$refs.customerChoose.openChoose()
    },
    handleChoose(selectedRows){
      this.customerList = [...selectedRows]
      var row = selectedRows[0]
      this.curValue = row.Id
      this.$emit('input', this.curValue)
      this.$emit('select', row)
    }
  }
}
</script>