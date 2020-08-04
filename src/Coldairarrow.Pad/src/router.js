import Vue from 'vue'
import Router from 'vue-router'
import UserLayout from './layouts/UserLayout'
import BaseLayout from './layouts/BaseLayout'
Vue.use(Router)
export default new Router({
    mode: 'history',
    scrollBehavior: () => ({ y: 0 }),
    routes: [
        {
            path: '/',
            component: BaseLayout,
            redirect: '/Home/Index',
            name: "Default"
        },
        {
            path: '/Home',
            component: BaseLayout,
            redirect: '/Home/Index',
            children: [
                {
                    path: '/Home/Index',
                    name: 'Index',
                    component: () => import('@/views/Home/Index')
                }
            ]
        },
        {
            path: '/User',
            component: UserLayout,
            redirect: '/User/Login',
            children: [
                {
                    path: '/User/Login',
                    name: 'Login',
                    component: () => import('@/views/Home/Login')
                }
            ]
        },
        {
            path: '/TD',
            component: BaseLayout,
            redirect: '/TD/InStorage',
            children: [
                {
                    path: '/TD/InStorage',
                    name: 'Receive',
                    component: () => import('@/views/TD/InStorage')
                },
                {
                    path: '/TD/InStorageList',
                    name: 'InStorageList',
                    component: () => import('@/views/TD/InStorageList')
                },
                {
                    path: '/TD/InStorageDetail/:id',
                    name: 'InStorageDetail',
                    component: () => import('@/views/TD/InStorageDetail'),
                    props: true
                }
            ]
        }
    ]
})
