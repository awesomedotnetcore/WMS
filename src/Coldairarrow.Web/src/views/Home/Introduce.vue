<template>
  <div>
    <a-row>
      <a-col :span="8">
        <a-card :hoverable="true" title="物料统计" :loading="loading.Material">
          <a-button slot="extra" type="dashed" shape="circle" size="small" icon="reload" @click="getMaterialData" />
          <a-statistic title="物料总数" :value="MaterialSum.Total" />
          <a-statistic title="当前仓库" :value="MaterialSum.Storage" />
        </a-card>
      </a-col>
      <a-col :span="8">
        <a-card :hoverable="true" title="入库统计" :loading="loading.InStorage">
          <a-button slot="extra" type="dashed" shape="circle" size="small" icon="reload" @click="getInStorageData" />
          <a-statistic title="今日入库" :value="InStorSum.Total" />
          <a-statistic title="当前仓库" :value="InStorSum.Storage" />
        </a-card>
      </a-col>
      <a-col :span="8">
        <a-card :hoverable="true" title="出库统计" :loading="loading.OutStorage">
          <a-button slot="extra" type="dashed" shape="circle" size="small" icon="reload" @click="getOutStorageData" />
          <a-statistic title="今日出库" :value="OutStorSum.Total" />
          <a-statistic title="当前仓库" :value="OutStorSum.Storage" />
        </a-card>
      </a-col>
    </a-row>
    <a-row>
      <a-col :span="12">
        <a-card title="入库历史" :hoverable="true" :bordered="true" :loading="loading.ListInStorage" :bodyStyle="{padding:'0px'}">
          <a-button slot="extra" type="dashed" shape="circle" size="small" icon="reload" @click="getInStorageList" />
          <a-table :columns="column" :bordered="true" :dataSource="listInStor" :pagination="false"></a-table>
        </a-card>
      </a-col>
      <a-col :span="12">
        <a-card title="出库历史" :hoverable="true" :bordered="true" :loading="loading.ListOutStorage" :bodyStyle="{padding:'0px'}">
          <a-button slot="extra" type="dashed" shape="circle" size="small" icon="reload" @click="getOutStorageList" />
          <a-table :columns="column" :bordered="true" :dataSource="listOutStor" :pagination="false"></a-table>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>
<script>
const column = [
  { title: '', dataIndex: 'Name' },
  { title: '今天', dataIndex: 'Day0' },
  { title: '昨天', dataIndex: 'Day1' },
  { title: '前天', dataIndex: 'Day2' },
  { title: '前2天', dataIndex: 'Day3' },
  { title: '前3天', dataIndex: 'Day4' },
  { title: '前4天', dataIndex: 'Day5' },
  { title: '前5天', dataIndex: 'Day6' }
]
export default {
  data() {
    return {
      column,
      loading: { Material: false, InStorage: false, OutStorage: false, ListInStorage: false, ListOutStorage: false },
      MaterialSum: {
        Total: 0,
        Storage: 0
      },
      InStorSum: {
        Total: 0,
        Storage: 0
      },
      OutStorSum: {
        Total: 0,
        Storage: 0
      },
      listInStor: [
        { Name: '总数', Day0: 0, Day1: 0, Day2: 0, Day3: 0, Day4: 0, Day5: 0, Day6: 0 },
        { Name: '仓库', Day0: 0, Day1: 0, Day2: 0, Day3: 0, Day4: 0, Day5: 0, Day6: 0 }
      ],
      listOutStor: [
        { Name: '总数', Day0: 0, Day1: 0, Day2: 0, Day3: 0, Day4: 0, Day5: 0, Day6: 0 },
        { Name: '仓库', Day0: 0, Day1: 0, Day2: 0, Day3: 0, Day4: 0, Day5: 0, Day6: 0 }
      ]
    }
  },
  mounted() {
    this.getMaterialData()
    this.getInStorageData()
    this.getOutStorageData()
    this.getInStorageList()
    this.getOutStorageList()
  },
  methods: {
    getMaterialData() {
      this.loading.Material = true
      this.$http.get('/Report/Introduce/MaterialSummary')
        .then(resJson => {
          this.loading.Material = false
          this.MaterialSum = resJson.Data
        })
    },
    getInStorageData() {
      this.loading.InStorage = true
      this.$http.get('/Report/Introduce/InStorageSummary')
        .then(resJson => {
          this.loading.InStorage = false
          this.InStorSum = resJson.Data
        })
    },
    getOutStorageData() {
      this.loading.OutStorage = true
      this.$http.get('/Report/Introduce/OutStorageSummary')
        .then(resJson => {
          this.loading.OutStorage = false
          this.OutStorSum = resJson.Data
        })
    },
    getInStorageList() {
      this.loading.ListInStorage = true
      this.$http.get('/Report/Introduce/InStorageList')
        .then(resJson => {
          this.loading.ListInStorage = false
          this.listInStor = resJson.Data
        })
    },
    getOutStorageList() {
      this.loading.ListOutStorage = true
      this.$http.get('/Report/Introduce/OutStorageList')
        .then(resJson => {
          this.loading.ListOutStorage = false
          this.listOutStor = resJson.Data
        })
    }
  }
}
</script>