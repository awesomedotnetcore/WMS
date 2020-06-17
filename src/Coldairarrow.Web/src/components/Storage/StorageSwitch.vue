<template>
  <div class="user-wrapper">
    <div class="content-box">
      <a-dropdown>
        <span class="action ant-dropdown-link user-dropdown-menu">
          <a-avatar size="small" icon="setting" />
          <span>{{ curStorage.Name }}({{ curStorage.Code }})</span>
        </span>
        <a-menu slot="overlay" class="user-dropdown-menu-wrapper">
          <a-menu-item v-for="item in listData" :key="item.Id">
            <a href="javascript:;" @click="handleSwitch(item)">
              <span>{{ item.Name }}({{ item.Code }})</span>
            </a>
          </a-menu-item>
        </a-menu>
      </a-dropdown>
    </div>
  </div>
</template>

<script>
export default {
  name: 'StorageSwitch',
  components: {
  },
  data() {
    return {
      listData: []
    }
  },
  computed: {
    curStorage() {
      var defaultStorage = this.listData.filter((item, index, arr) => { return item.IsDefault })
      if (defaultStorage.length > 0) return defaultStorage[0]
      if (this.listData.length === 0) return { Name: '', Code: '' }
      return this.listData[0]
    }
  },
  mounted() {
    this.getListData()
  },
  methods: {
    getListData() {
      this.$http.get('/Base/Base_UserStor/GetStorage')
        .then(resJson => {
          this.listData = resJson.Data
        })
    },
    handleSwitch(storage) {
      if (this.curStorage !== storage) {
        this.$http.post('/Base/Base_UserStor/SwitchStorage', { id: storage.Id })
          .then(resJson => {
            if (resJson.Success) {
              this.curStorage.IsDefault = false
              storage.IsDefault = true
              this.$router.go(0)
            }
            //this.getListData()
          })
      }
    }
  }
}
</script>
