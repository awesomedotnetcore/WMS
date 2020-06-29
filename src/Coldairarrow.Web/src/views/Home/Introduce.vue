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
  <!-- <a-card :bordered="false">
    <div>
      <p>中南智能仓库管理系统</p>
      <p>
        使用技术栈：<br />
        后端：采用.NET Core平台，ASP.NET Core3.0，C#语言（使用反射等高级技术），Entity
        FrameworkCore（数据库ORM框架）。<br />
        使用数据仓储模式，抽象化数据库操作(CRUD等)、支持事务处理以及分布式事务处理（跨库）<br />
        支持数据库读写分离、分库分表及事务(仅支持单表操作,不支持多表)
        全面采用Autofac作为IOC容器,面向接口编程,全面解耦<br />
        集成多种工具类库以及操作拓展<br />
        数据库：支持SqlServer,PostgreSQL,MySQL,Oracle（框架使用简单工厂，工厂方法，抽象工厂，可轻松更换数据库）,Redis作为分布式缓存<br />
        前端：Vue2.x全家桶+Ant Design Vue，其中集成常用组件，力求方便项目开发。
        <br />
        <br />
        具体技术实施：<br />
        项目采用前后端完全分离模式，并采用严格分层模式，极大的增加聚合度，降低耦合度，<br />
        提高代码的健壮性，可维护性。<br />
        前后端通过JWT进行身份验证,通过数据接口操作数据，统一使用JSON作为数据格式，并使用默认接口签名算法保证接口的安全性。
        <br />
        <br />
        功能架构部分详解：<br />
        快速开发：此功能为框架的核心，通过选择数据库中的表，就能自动生成对应的实体层、业务逻辑层、控制器、前端页面Vue文件，无需编写代码即可生成基本的CRUD操作。<br />
        接口密钥管理：管理接口的密钥，若开启接口签名的规则，则前端需要通过给接口签名才能够正常访问后台接口。<br />
        权限管理：使用基本的RBAC权限控制，支持操作权限、接口权限以及数据权限
        <br />
      </p>
    </div>
  </a-card> -->
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