import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/PB/PB_Material/QueryDataList', query)
    },
    GetTheData(id) {
        return http.post('/PB/PB_Material/GetTheData', { id: id })
    }
}