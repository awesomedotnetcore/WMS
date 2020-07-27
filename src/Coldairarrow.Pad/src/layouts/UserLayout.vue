<template>
  <a-layout>
    <a-layout-header class="layout-header">
      <div :style="{marginLeft:'0px'}">
        <img src="../assets/logo.svg" class="logo" />
        <span>WMS</span>
      </div>
    </a-layout-header>
    <a-layout-content>
      <router-view />
    </a-layout-content>
  </a-layout>
</template>

<script>
export default {
  name: 'UserLayout',
  components: {
  }
}
</script>
<style scoped>
.layout-header {
  padding-left: 10px;
}
.layout-header .logo {
  height: 50px;
  padding-bottom: 10px;
  padding-right: 5px;
}
.layout-header span {
  font-size: 25px;
  color: white;
  font-weight: 500;
}
</style>