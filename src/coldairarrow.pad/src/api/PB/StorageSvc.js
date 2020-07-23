import http from '../../utils/axios-plugin'

export default {
    GetStorage() {
        return http.get('/Base/Base_UserStor/GetStorage')
    },
    SwitchStorage(id){
        return http.post('/Base/Base_UserStor/SwitchStorage', { id: id })
    }
}