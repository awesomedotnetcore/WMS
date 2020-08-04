<template>
  <a-dropdown>
    <a class="ant-dropdown-link" @click="e => e.preventDefault()">
      {{ User.RealName }}
    </a>
    <a-menu slot="overlay">
      <a-menu-item>
        <a @click="hadlerLogout">退出登录</a>
      </a-menu-item>
    </a-menu>
  </a-dropdown>
</template>

<script>
import UserSvc from '../api/Home/UserSvc'
import TokenCache from '../utils/TokenCache'
export default {
  data() {
    return {
      User: {}
    }
  },
  mounted() {
    this.getUser()
  },
  methods: {
    getUser() {
      UserSvc.GetCurUser().then(resJson => {
        this.User = resJson.Data
      })
    },
    hadlerLogout() {
      TokenCache.deleteToken()
      this.$router.push({ path: '/User/Login' })
    }
  }
}
</script>

<style>
</style>