import http from '../../utils/axios-plugin'

export default {
    SaveData(data) {
        return http.post('/TD/TD_Send/SaveData', data)
    },
    GetDataList(query) {
        return http.post('/TD/TD_Send/GetDataList', query)
    },
    GetTheData(id) {
        return http.post('/TD/TD_Send/GetTheData', { id: id })
    },
    Approval(id, type) {
        return http.post('/TD/TD_Send/Approval', { Id: id, AuditType: type })
    },
    DeleteData(id) {
        return http.post('/TD/TD_Send/DeleteData', [id])
    }
}