<template>
  <a-row :gutter="10">
    <a-col :span="12">
      <a @click="$router.push({ path: '/TD/InStorage' })">
        <a-result title="生产入库">
          <template #icon>
            <img src="../../assets/receive.svg" width="100" />
          </template>
        </a-result>
      </a>
    </a-col>
    <a-col :span="12">
      <a @click="$router.push({ path: '/TD/Send' })">
        <a-result title="发货">
          <template #icon>
            <img src="../../assets/send.svg" width="100" />
          </template>
        </a-result>
      </a>
    </a-col>
    <a-col :span="12">
      <a href="javascript:;">
        <a-result title="入库">
          <template #icon>
            <img src="../../assets/in.svg" width="100" />
          </template>
        </a-result>
      </a>
    </a-col>
    <a-col :span="12">
      <a href="javascript:;">
        <a-result title="出库">
          <template #icon>
            <img src="../../assets/out.svg" width="100" />
          </template>
        </a-result>
      </a>
    </a-col>
  </a-row>
</template>

<script>
// const data = [
//   { title: '收货', icon: 'pic-left', path: '/TD/Receive' },
//   { title: '发货', icon: 'pic-right', path: '/TD/Send' },
//   { title: '入库', icon: 'login', path: '/TD/InStorage' },
//   { title: '出库', icon: 'logout', path: '/TD/OutStorage' }
// ];

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
.svg {
  width: 30px;
}
</style>