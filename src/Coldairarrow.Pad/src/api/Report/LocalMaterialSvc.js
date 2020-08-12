import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/IT/IT_LocalMaterial/GetDataList', query)
    }
}