import http from '../../utils/axios-plugin'

export default {
    GetDataList(query) {
        return http.post('/PB/PB_Material/QueryDataList', query)
    },
    GetTheData(id) {
        return http.post('/PB/PB_Material/GetTheData', { id: id })
    },
    GetByBarcode(barcode) {
        return http.get('/PB/PB_Material/GetByBarcode?barcode=' + barcode)
    },
    GetByCode(code) {
        return http.get('/PB/PB_Material/GetByCode?code=' + code)
    }
}