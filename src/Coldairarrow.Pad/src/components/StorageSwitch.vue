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
      var defaultStorage = this.listData.find(w => w.IsDefault)
      if (defaultStorage) return defaultStorage
      else return { Name: '', Code: '' }
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