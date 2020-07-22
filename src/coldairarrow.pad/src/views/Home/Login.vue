<template>
  <a-spin :spinning="loading">
    <a-card title="登录" :bordered="false">
      <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
        <a-form-model-item prop="userName">
          <a-input v-model="entity.userName" placeholder="用户名">
            <a-icon slot="prefix" type="user" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item prop="password">
          <a-input-password v-model="entity.password" placeholder="密码">
            <a-icon slot="prefix" type="lock" />
          </a-input-password>
        </a-form-model-item>
        <a-form-model-item>
          <a-button type="primary" block @click="handlerSubmit">确定</a-button>
        </a-form-model-item>
      </a-form-model>
    </a-card>
  </a-spin>
</template>

<script>
import TokenCache from '../../utils/TokenCache'
import UserSvc from '../../api/Home/UserSvc'
export default {
  data() {
    return {
      loading: false,
      entity: {},
      rules: {
        userName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
        password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
      }
    }
  },
  methods: {
    handlerSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        UserSvc.Login(this.entity).then(resJson => {
          this.loading = false
          if (resJson.Success) {
            TokenCache.setToken(resJson.Data)
            this.$router.push({ path: '/' })
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>

<style>
</style>