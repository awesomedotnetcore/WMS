import http from '../../utils/axios-plugin'

export default {
    AutoInByTary(data) {
        return http.post('/TD/TD_InStorage/AutoInByTary', data)
    },
    ComplatedInByTray(id) {
        return http.post('/TD/TD_InStorage/ComplatedInByTray?id=' + id)
    }
}