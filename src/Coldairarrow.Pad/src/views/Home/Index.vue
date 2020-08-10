<template>
  <div>
    <a-card title="入库" size="small">
      <a-row :gutter="10">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/ProductIn' })">
            <a-icon type="login" class="app-icon" />
            <div class="app-text">生产入库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/List' })">
            <a-icon class="app-icon" type="ordered-list" />
            <div class="app-text">入库管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/InBlankTray' })">
            <a-icon class="app-icon" type="pic-left" />
            <div class="app-text">空托盘入库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/Receive' })">
            <a-icon class="app-icon" type="import" />
            <div class="app-text">采购收货</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
    <a-card title="出库" size="small">
      <a-row :gutter="10">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/ProductOut' })">
            <a-icon class="app-icon" type="logout" />
            <div class="app-text">生产出库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/List' })">
            <a-icon class="app-icon" type="ordered-list" />
            <div class="app-text">出库管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/OutBlankTray' })">
            <a-icon class="app-icon" type="pic-right" />
            <div class="app-text">空托盘出库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/Receive' })">
            <a-icon class="app-icon" type="export" />
            <div class="app-text">销售发货</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
    <a-card title="基本资料" size="small">
      <a-row :gutter="10">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/PB/MaterialList' })">
            <a-icon class="app-icon" type="border" />
            <div class="app-text">物料管理</div>
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
.app-card .app-icon {
  font-size:48px;
}
.app-card .app-text {
  margin-top: 8px;
  color: #666;
}
</style>