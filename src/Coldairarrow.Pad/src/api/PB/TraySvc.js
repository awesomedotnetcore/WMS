import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/PB/PB_Tray/GetDataList', query)
    },
    OutManualTray(data) {
        return http.post('/PB/PB_Tray/OutManualTray', data)
    },
    QueryTray(data) {
        return http.post('/PB/PB_Tray/QueryTray', data)
    },
    InAutoTray(data) {
        return http.post('/PB/PB_Tray/InAutoTray', data)
    },
}