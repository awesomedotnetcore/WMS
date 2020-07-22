import http from '../../utils/axios-plugin'

export default {
    Login(user) {
        return http.post('/Base_Manage/Home/SubmitLogin', user)
    },
    GetCurUser(){
        return http.get('/Base_Manage/Base_User/GetCurUser')
    }
}