import http from '../../utils/axios-plugin'

export default {
    GetByCode(code) {
        return http.get('/Base/Base_Enum/GetByCode?code=' + code)
    }
}