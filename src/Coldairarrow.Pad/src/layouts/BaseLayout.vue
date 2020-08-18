<template>
  <a-layout>
    <a-layout-header class="layout-header">
      <a-row>
        <a-col :span="6">
          <a @click="goHome">
            <img src="../assets/logo.svg" width="50px" />
          </a>
        </a-col>
        <a-col :span="14">
          <storage-switch ref="storageSwitch"></storage-switch>
        </a-col>
        <a-col :span="4">
          <user-menu></user-menu>
        </a-col>
      </a-row>
    </a-layout-header>
    <a-layout-content>
      <router-view />
    </a-layout-content>
  </a-layout>
</template>

<script>
import UserMenu from '../components/UserMenu'
import StorageSwitch from '../components/StorageSwitch'
export default {
  name: 'BlankLayout',
  components: {
    UserMenu,
    StorageSwitch
  },
  methods: {
    goHome() {
      if (this.$route.name !== 'HomeIndex') {
        this.$router.push({ path: '/Home/Index' })
      }
    }
  }
}
</script>
<style scoped>
.layout-header {
  padding-left: 10px;
}
</style>