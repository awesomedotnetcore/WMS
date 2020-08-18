<template>
  <a-card title="测试页面">
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="MaterialCode">
        <input-material v-model="entity.MaterialCode"></input-material>
      </a-form-model-item>
      <a-form-model-item prop="TrayCode">
        <input-tray v-model="entity.TrayCode"></input-tray>
      </a-form-model-item>
    </a-form-model>
  </a-card>
</template>

<script>
import InputMaterial from '../../components/InputMaterial'
import InputTray from '../../components/InputTray'
export default {
  components: {
    InputMaterial,
    InputTray
  },
  data() {
    return {
      entity: {},
      rules: {}
    }
  }
}
</script>

<style>
</style>