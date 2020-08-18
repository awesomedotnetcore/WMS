import http from '../../utils/axios-plugin'

export default {
    GetStorage() {
        return http.get('/Base/Base_UserStor/GetStorage')
    },
    GetCurStorage() {
        return http.get('/PB/PB_Storage/GetCurStorage')
    },
    SwitchStorage(id){
        return http.post('/Base/Base_UserStor/SwitchStorage', { id: id })
    }
}