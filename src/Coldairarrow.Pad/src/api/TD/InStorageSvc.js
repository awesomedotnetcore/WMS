import http from '../../utils/axios-plugin'

export default {
    AutoInByTary(data) {
        return http.post('/TD/TD_InStorage/AutoInByTary', data)
    },
    ManualIn(data) {
        return http.post('/TD/TD_InStorage/ManualIn', data)
    },
    GetDataList(query) {
        return http.post('/TD/TD_InStorage/GetDataList', query)
    },
    GetTheData(id) {
        return http.post('/TD/TD_InStorage/GetTheData', { id: id })
    },
    Audit(id, type) {
        return http.post('/TD/TD_InStorage/Audit', { Id: id, AuditType: type })
    }
}