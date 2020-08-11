import http from '../../utils/axios-plugin'

export default {
    SaveData(data) {
        return http.post('/TD/TD_Receiving/SaveData', data)
    },
    GetDataList(query) {
        return http.post('/TD/TD_Receiving/GetDataList', query)
    },
    GetTheData(id) {
        return http.post('/TD/TD_Receiving/GetTheData', { id: id })
    },
    Approval(id, type) {
        return http.post('/TD/TD_Receiving/Approval', { Id: id, AuditType: type })
    },
    DeleteData(id) {
        return http.post('/TD/TD_Receiving/DeleteData', [id])
    }
}