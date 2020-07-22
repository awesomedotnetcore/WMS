<template>
  <div>
    <div>收货</div>
    <a-list :grid="{ gutter: 16, column: 2 }">
      <a-list-item>
        <a-button type="link" @click="$router.go(-1)">返回</a-button>
        <a-button type="link" @click="$router.push({path:'/Home/Login'})">登录</a-button>
        <a-button type="link" @click="$router.push({path:'/Home/Index'})">首页</a-button>
        <a-button type="link" @click="$router.push({path:'/TD/Receive'})">收货</a-button>
        <a-button type="link" @click="$router.push({path:'/TD/Send'})">发货</a-button>
      </a-list-item>
    </a-list>
  </div>
</template>

<script>
export default {

}
</script>

<style>

</style>