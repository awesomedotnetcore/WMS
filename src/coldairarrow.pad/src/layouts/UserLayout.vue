<template>
  <a-layout>
    <a-layout-header class="layout-header">
      <div :style="{marginLeft:'0px'}">
        <logo-svg></logo-svg>
        <span>WMS</span>
      </div>
    </a-layout-header>
    <a-layout-content>
      <router-view />
    </a-layout-content>
  </a-layout>
</template>

<script>
import LogoSvg from '../assets/logo.svg'
export default {
  name: 'UserLayout',
  components: {
    LogoSvg
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