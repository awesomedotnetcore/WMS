import axios from 'axios'
import TokenCache from './TokenCache'
import DefaultConfig from '../config'
const Axios = axios.create({
    baseURL: DefaultConfig.ApiUrl,
    timeout: 10000
})

// 在发送请求之前做某件事
Axios.interceptors.request.use(config => {
    //携带token
    var token = TokenCache.getToken()
    if (token) {
        config.headers.Authorization = 'Bearer ' + token
    }
    return config
}, error => {
    return Promise.reject(error)
})

//返回状态判断(添加响应拦截器)
Axios.interceptors.response.use(res => {
    //授权失败
    if (!res.data.Success && res.data.ErrorCode == 401) {
        TokenCache.deleteToken()
        location.href = '/User/Login'
    }
    return res.data
}, error => {
    let errorMsg = ''
    if (error.message.includes('timeout')) {
        errorMsg = '请求超时!'
    } else {
        errorMsg = '请求异常!'
    }
    return Promise.resolve({ Success: false, Msg: errorMsg })
})
export default Axios