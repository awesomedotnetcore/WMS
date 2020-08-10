/* eslint-disable no-unused-vars */
import Vue from 'vue'
import Router from 'vue-router'
import TokenCache from './utils/TokenCache'
import UserLayout from './layouts/UserLayout'
import BaseLayout from './layouts/BaseLayout'
Vue.use(Router)
const router = new Router({
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
                    name: 'HomeIndex',
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
                    name: 'UserLogin',
                    component: () => import('@/views/Home/Login')
                }
            ]
        },
        {
            path: '/InStorage',
            component: BaseLayout,
            redirect: '/InStorage/ProductIn',
            children: [
                {
                    path: '/InStorage/ProductIn',
                    name: 'InStorageProductIn',
                    component: () => import('@/views/InStorage/ProductIn')
                },
                {
                    path: '/InStorage/List',
                    name: 'InStorageList',
                    component: () => import('@/views/InStorage/List')
                },
                {
                    path: '/InStorage/Detail/:id',
                    name: 'InStorageDetail',
                    component: () => import('@/views/InStorage/Detail'),
                    props: true
                },
                {
                    path: '/InStorage/Receive',
                    name: 'InStorageReceive',
                    component: () => import('@/views/InStorage/Receive')
                }
            ]
        },
        {
            path: '/OutStorage',
            component: BaseLayout,
            redirect: '/OutStorage/ProductOut',
            children: [
                {
                    path: '/OutStorage/ProductOut',
                    name: 'OutStorageProductOut',
                    component: () => import('@/views/OutStorage/ProductOut')
                },
                {
                    path: '/OutStorage/List',
                    name: 'OutStorageList',
                    component: () => import('@/views/OutStorage/List')
                },
                {
                    path: '/OutStorage/Detail/:id',
                    name: 'OutStorageDetail',
                    component: () => import('@/views/OutStorage/Detail'),
                    props: true
                },
                {
                    path: '/OutStorage/OutBlankTray',
                    name: 'OutStorageOutBlankTray',
                    component: () => import('@/views/OutStorage/OutBlankTray'),
                }
            ]
        },
        {
            path: '/PB',
            component: BaseLayout,
            redirect: '/PB/MaterialList',
            children: [
                {
                    path: '/PB/MaterialList',
                    name: 'PBMaterialList',
                    component: () => import('@/views/PB/MaterialList')
                },
                {
                    path: '/PB/MaterialDetail/:id',
                    name: 'PBMaterialDetail',
                    component: () => import('@/views/PB/MaterialDetail'),
                    props: true
                }
            ]
        }
    ]
})
router.beforeEach((to, from, next) => {
    var token = TokenCache.getToken()
    if (to.name !== 'UserLogin' && !token) next({ name: 'UserLogin' })
    next()
})
export default router
