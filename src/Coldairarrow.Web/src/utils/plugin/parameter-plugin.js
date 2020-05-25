import ParameterCache from "@/utils/cache/ParameterCache"

export default {
    install(Vue) {
        Object.defineProperty(Vue.prototype, '$para', { value: ParameterCache.getPara })
    }
}