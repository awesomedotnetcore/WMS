import { Axios } from "@/utils/plugin/axios-plugin"

let baseParameters = {}
let inited = false

let ParameterCache = {
    inited() {
        return inited
    },
    init() {
        if (!inited) {
            Axios.get('/Base/Base_Parameter/GetConfig').then(resJson => {
                baseParameters = { ...resJson.Data }
                inited = true
            })
        }
    },
    getPara(code) {
        return baseParameters[code]
    },
    clear() {
        inited = false
        baseParameters = {}
    }
}

export default ParameterCache