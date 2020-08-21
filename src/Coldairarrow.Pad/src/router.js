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
                },
                {
                    path: '/Home/Test',
                    name: 'HomeTest',
                    component: () => import('@/views/Home/Test')
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
            redirect: '/InStorage/List',
            children: [
                {
                    path: '/InStorage/AutoIn',
                    name: 'InStorageAutoIn',
                    component: () => import('@/views/InStorage/AutoIn')
                },
                {
                    path: '/InStorage/ManualIn',
                    name: 'InStorageManualIn',
                    component: () => import('@/views/InStorage/ManualIn')
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
                    path: '/InStorage/InBlankTray',
                    name: 'InStorageInBlankTray',
                    component: () => import('@/views/InStorage/InBlankTray'),
                },
                {
                    path: '/InStorage/InManualTray',
                    name: 'InStorageInManualTray',
                    component: () => import('@/views/InStorage/InManualTray'),
                },
                {
                    path: '/InStorage/Receive',
                    name: 'InStorageReceive',
                    component: () => import('@/views/InStorage/Receive')
                },
                {
                    path: '/InStorage/ReceiveList',
                    name: 'InStorageReceiveList',
                    component: () => import('@/views/InStorage/ReceiveList')
                },
                {
                    path: '/InStorage/ReceiveDetail/:id',
                    name: 'InStorageReceiveDetail',
                    component: () => import('@/views/InStorage/ReceiveDetail'),
                    props: true
                }
            ]
        },
        {
            path: '/OutStorage',
            component: BaseLayout,
            redirect: '/OutStorage/ProductOut',
            children: [
                {
                    path: '/OutStorage/AutoOut',
                    name: 'OutStorageAutoOut',
                    component: () => import('@/views/OutStorage/AutoOut')
                },
                {
                    path: '/OutStorage/ManualOut',
                    name: 'OutStorageManualOut',
                    component: () => import('@/views/OutStorage/ManualOut')
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
                },
                {
                    path: '/OutStorage/OutManualTray',
                    name: 'OutStorageOutManualTray',
                    component: () => import('@/views/OutStorage/OutManualTray'),
                },
                {
                    path: '/OutStorage/Send',
                    name: 'OutStorageSend',
                    component: () => import('@/views/OutStorage/Send')
                },
                {
                    path: '/OutStorage/SendList',
                    name: 'OutStorageSendList',
                    component: () => import('@/views/OutStorage/SendList')
                },
                {
                    path: '/OutStorage/SendDetail/:id',
                    name: 'OutStorageSendDetail',
                    component: () => import('@/views/OutStorage/SendDetail'),
                    props: true
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
                },
                {
                    path: '/PB/PlanList',
                    name: 'PBPlanList',
                    component: () => import('@/views/PB/PlanList')
                },
                {
                    path: '/PB/PlanDetail/:id',
                    name: 'PBPlanDetail',
                    component: () => import('@/views/PB/PlanDetail'),
                    props: true
                },
                {
                    path: '/PB/TrayList',
                    name: 'PBTrayList',
                    component: () => import('@/views/PB/TrayList')
                },
                {
                    path: '/PB/LocalList',
                    name: 'PBLocalList',
                    component: () => import('@/views/PB/LocalList')
                }
            ]
        },
        {
            path: '/Report',
            component: BaseLayout,
            redirect: '/Report/LocalMaterialReport',
            children: [
                {
                    path: '/Report/LocalMaterialReport',
                    name: 'LocalMaterialReport',
                    component: () => import('@/views/Report/LocalMaterialReport')
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
