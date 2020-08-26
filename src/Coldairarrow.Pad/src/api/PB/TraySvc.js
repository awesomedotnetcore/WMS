import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/PB/PB_Tray/GetDataList', query)
    },
    OutManualTray(data) {
        return http.post('/PB/PB_Tray/OutManualTray', data)
    },
    InManualTray(data) {
        return http.post('/PB/PB_Tray/InManualTray', data)
    },
    InAutoTray(data) {
        return http.post('/PB/PB_Tray/InAutoTray', data)
    },
}