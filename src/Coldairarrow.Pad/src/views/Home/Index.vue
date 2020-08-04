<template>
  <a-row :gutter="10">
    <a-col :span="6">
      <div class="app-card" @click="$router.push({ path: '/TD/InStorage' })">
        <a-avatar shape="square" :size="64" icon="login" />
        <div class="app-text">生产入库</div>
      </div>
    </a-col>
    <a-col :span="6">
      <div class="app-card" @click="$router.push({ path: '/TD/InStorageList' })">
        <a-avatar shape="square" :size="64" icon="ordered-list" />
        <div class="app-text">入库管理</div>
      </div>
    </a-col>
  </a-row>
</template>
<script>
export default {
  components: {
  },
  data() {
    return {

    };
  }
};
</script>
<style>
.app-card {
  margin: 20px;
  width: 100%;
  height: 100%;
  text-align: center;
}
.app-card .app-text {
  margin-top: 8px;
  color: #666;
}
</style>