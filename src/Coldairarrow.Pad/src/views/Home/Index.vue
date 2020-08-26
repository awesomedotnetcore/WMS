<template>
  <div>
    <a-card title="入库" size="small">
      <a-row :gutter="[5,10]">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/AutoIn' })">
            <a-icon type="login" class="app-icon" />
            <div class="app-text">自动入库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/ManualIn' })">
            <a-icon type="download" class="app-icon" />
            <div class="app-text">手动入库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/List' })">
            <a-icon class="app-icon" type="audit" />
            <div class="app-text">入库管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/InBlankTray' })">
            <a-icon class="app-icon" type="pic-left" />
            <div class="app-text">自动入空托</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/InManualTray' })">
            <a-icon class="app-icon" type="menu-unfold" />
            <div class="app-text">手动入空托</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/Receive' })">
            <a-icon class="app-icon" type="import" />
            <div class="app-text">采购收货</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/InStorage/ReceiveList' })">
            <a-icon class="app-icon" type="profile" />
            <div class="app-text">收货管理</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
    <a-card title="出库" size="small">
      <a-row :gutter="[5,10]">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/AutoOut' })">
            <a-icon class="app-icon" type="logout" />
            <div class="app-text">自动出库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/ManualOut' })">
            <a-icon class="app-icon" type="upload" />
            <div class="app-text">手动出库</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/List' })">
            <a-icon class="app-icon" type="audit" />
            <div class="app-text">出库管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/OutBlankTray' })">
            <a-icon class="app-icon" type="pic-right" />
            <div class="app-text">自动出空托</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/OutManualTray' })">
            <a-icon class="app-icon" type="menu-fold" />
            <div class="app-text">手动出空托</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/Send' })">
            <a-icon class="app-icon" type="export" />
            <div class="app-text">销售发货</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/OutStorage/SendList' })">
            <a-icon class="app-icon" type="profile" />
            <div class="app-text">发货管理</div>
          </div>
        </a-col>
      </a-row>
    </a-card>
    <a-card title="基本资料" size="small">
      <a-row :gutter="[5,10]">
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/Report/LocalMaterialReport' })">
            <a-icon class="app-icon" type="dot-chart" />
            <div class="app-text">库存报表</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/PB/MaterialList' })">
            <a-icon class="app-icon" type="gold" />
            <div class="app-text">物料管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/PB/TrayList' })">
            <a-icon class="app-icon" type="border-outer" />
            <div class="app-text">托盘管理</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/PB/LocalList' })">
            <a-icon class="app-icon" type="table" />
            <div class="app-text">货位管理</div>
          </div>
        </a-col>
        <!-- <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/PB/PlanList' })">
            <a-icon class="app-icon" type="profile" />
            <div class="app-text">计划表</div>
          </div>
        </a-col>
        <a-col :span="6">
          <div class="app-card" @click="$router.push({ path: '/Home/Test' })">
            <a-icon class="app-icon" type="profile" />
            <div class="app-text">测试页面</div>
          </div>
        </a-col> -->
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
  font-size: 48px;
  color: #1890ff;
}
.app-card .app-text {
  margin-top: 8px;
  color: #666;
}
</style>