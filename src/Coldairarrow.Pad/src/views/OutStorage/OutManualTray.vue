<template>
  <a-card title="手动空托盘出库">
    <a-button slot="extra" type="primary" ghost @click="handlerSubmit" :loading="loading">确定</a-button>
    <a-form-model layout="horizontal" :model="queryData.Search" :rules="rules" ref="form">
      <!-- <a-form-model-item prop="LocalCode">
        <input-location v-model="entity.LocalCode"></input-location>
      </a-form-model-item> -->
      <a-form-model-item prop="TrayCode">
        <input-tray v-model="queryData.Search.Keyword"></input-tray>
      </a-form-model-item>
    </a-form-model>

    <a-list :data-source="listData" :rowKey="item=>item.Id" item-layout="horizontal" :loading="loading">
      <a-list-item slot="renderItem" slot-scope="item">
        <a-list-item-meta>
          <div slot="title">
            <span class="spanTag">  托盘号:{{ item.Code }}</span>            
          </div>       
        </a-list-item-meta>
      </a-list-item>
    </a-list> 

  </a-card>
</template>

<script>
//import InputLocation from '../../components/InputLocation'
import InputTray from '../../components/InputTray'
import TraySvc from '../../api/PB/TraySvc'

export default {
  components: {
  // InputLocation,
   InputTray
  },
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        LocalCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }],
      },
      queryData: {
        Search: { Keyword: null }
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
        TraySvc.GetDataList(this.queryData).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            this.listData = resJson.Data
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    },
    
  }
}
</script>