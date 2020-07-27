import Vue from 'vue'
import VueRouter from 'vue-router'
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
import router from './router'
import App from './App.vue'

Vue.config.productionTip = false
Vue.use(VueRouter)
Vue.use(Antd);
new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
