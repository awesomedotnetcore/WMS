import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/PD/PD_Plan/GetDataList', query)
    },
    GetTheData(id) {
        return http.post('/PD/PD_Plan/GetTheData', { id: id })
    }
}