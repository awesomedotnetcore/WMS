<template>
  <a-card title="生产入库" :bordered="false">
    <a-button slot="extra" type="primary" size="small" @click="handlerSubmit">确定</a-button>
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form">
      <a-form-model-item prop="AGVCode">
        <input-code v-model="entity.AGVCode" placeholder="地码"></input-code>
      </a-form-model-item>
      <a-form-model-item prop="TrayCode">
        <input-code v-model="entity.TrayCode" placeholder="托盘号"></input-code>
      </a-form-model-item>
    </a-form-model>
    <a-card>
      <a-button slot="extra" type="primary" size="small" @click="openCodeReader">增加</a-button>
      <a-list item-layout="horizontal" :data-source="listData">
        <a-list-item slot="renderItem" slot-scope="item, index">
          <a-list-item-meta :description="index + ' ' + item">
            <a-tag slot="avatar">{{ index }}</a-tag>
            <a slot="title">{{ item }}</a>
          </a-list-item-meta>
        </a-list-item>
      </a-list>
      <code-reader ref="codeReader" :multiple="true" @success="handerReaderCode"></code-reader>
    </a-card>
  </a-card>
</template>

<script>
import InputCode from '../../components/InputBarcode'
import CodeReader from '../../components/BarcodeReader'
export default {
  components: {
    InputCode,
    CodeReader
  },
  data() {
    return {
      loading: false,
      entity: {},
      listData: [],
      rules: {
        AGVCode: [{ required: true, message: '请输入物料编码', trigger: 'blur' }],
        TrayCode: [{ required: true, message: '请输入托盘号', trigger: 'blur' }]
      }
    }
  },
  methods: {
    handlerSubmit() { },
    openCodeReader() {
      this.$refs.codeReader.openReader()
    },
    handerReaderCode(code) {
      this.listData.push(code)
    }
  }
}
</script>

<style>
</style>