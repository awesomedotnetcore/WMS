<template>
  <div>
    <a-card title="入库" size="small">
      <a-row :gutter="10">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/ProductIn' })">
            <a-avatar shape="square" :size="64" icon="login" />
            <div class="app-text">生产入库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/List' })">
            <a-avatar shape="square" :size="64" icon="ordered-list" />
            <div class="app-text">入库管理</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
    <a-card title="出库" size="small">
      <a-row :gutter="10">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/ProductOut' })">
            <a-avatar shape="square" :size="64" icon="logout" />
            <div class="app-text">生产出库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/List' })">
            <a-avatar shape="square" :size="64" icon="ordered-list" />
            <div class="app-text">出库管理</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
  </div>
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
  text-align: center;
}
.app-card .app-text {
  margin-top: 8px;
  color: #666;
}
</style>