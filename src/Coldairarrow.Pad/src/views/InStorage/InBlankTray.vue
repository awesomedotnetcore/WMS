<template>
  <a-card title="空托盘入库">    
    <a-form-model layout="horizontal" :model="entity" :rules="rules" ref="form"> 
      <!-- <a-row :gutter="16">
        <a-col :span="18">
          <a-form-model-item prop="TrayTypeId">
              <a-select v-model="entity.TrayTypeId" placeholder="托盘类型">
                <a-select-option v-for="item in this.TrayTypeList" :key="item.Id">{{ item.Name }}</a-select-option>
              </a-select>
          </a-form-model-item>
        </a-col>
        <a-col :span="4">
          <a-form-model-item >
            <a-button slot="extra" type="primary" @click="handlerSubmit" :loading="loading">出库</a-button>  
          </a-form-model-item>
        </a-col>
      </a-row>            -->
    </a-form-model>  

  </a-card>
</template>