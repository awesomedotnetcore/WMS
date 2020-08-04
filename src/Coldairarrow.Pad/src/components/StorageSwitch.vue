<template>
  <a-dropdown>
    <a class="ant-dropdown-link" @click="e => e.preventDefault()">
      {{ curStorage.Name }}
    </a>
    <a-menu slot="overlay">
      <a-menu-item v-for="item in listData" :key="item.Id">
        <a href="javascript:;" @click="handleSwitch(item)">
          <span>{{ item.Name }}({{ item.Code }})</span>
        </a>
      </a-menu-item>
    </a-menu>
  </a-dropdown>
</template>

<script>
import StorageSvc from '../api/PB/StorageSvc'
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
      var defaultStorage = this.listData.filter((item) => { return item.IsDefault })
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
      StorageSvc.GetStorage()
        .then(resJson => {
          this.listData = resJson.Data
        })
    },
    handleSwitch(storage) {
      if (this.curStorage !== storage) {
        StorageSvc.SwitchStorage(storage.Id).then(resJson => {
          if (resJson.Success) {
            this.curStorage.IsDefault = false
            storage.IsDefault = true
            this.$router.go(0)
          }
        })
      }
    }
  }
}
</script>

<style>
</style>