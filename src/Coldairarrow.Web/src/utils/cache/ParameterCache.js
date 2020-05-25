import { Axios } from "@/utils/plugin/axios-plugin"

let baseParameters = []
let inited = false

let ParameterCache = {
    inited() {
        return inited
    },
    init(callBack) {
        if (inited)
            callBack()
        else {
            Axios.post('/Base_Manage/Home/GetOperatorInfo').then(resJson => {
                baseParameters = resJson.Data.Permissions
                inited = true
                callBack()
            })
        }
    },
    hasPermission(thePermission) {
        return baseParameters.includes(thePermission)
    },
    clear() {
        inited = false
        baseParameters = []
    }
}

export default ParameterCache