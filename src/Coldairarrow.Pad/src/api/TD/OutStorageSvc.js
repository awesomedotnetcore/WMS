import http from '../../utils/axios-plugin'

export default {
    ProductOut(data) {
        return http.post('/TD/TD_OutStorage/ProductOut', data)
    },
    GetDataList(query) {
        return http.post('/TD/TD_OutStorage/GetDataList', query)
    },
    GetTheData(id) {
        return http.post('/TD/TD_OutStorage/GetTheData', { id: id })
    },
    Audit(id, type) {
        return http.post('/TD/TD_OutStorage/Audit', { Id: id, AuditType: type })
    },
    OutAutoTray(data) {
        return http.post('/TD/TD_OutStorage/OutAutoTray', data)
    },
    OutTrayType() {
        return http.post('/PB/PB_TrayType/GetAllData')
    }

    
}