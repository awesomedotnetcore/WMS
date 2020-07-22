<template>
  <a-row :gutter="10">
    <a-col :span="12">
      <in-svg class="svg"></in-svg>
    </a-col>
    <a-col :span="12">
      <out-svg class="svg"></out-svg>
    </a-col>
    <a-col :span="12">
      <receive-svg class="svg"></receive-svg>
    </a-col>
    <a-col :span="12">
      <send-svg class="svg"></send-svg>
    </a-col>
  </a-row>
</template>

<script>
import InSvg from '../../assets/in.svg'
import OutSvg from '../../assets/out.svg'
import ReceiveSvg from '../../assets/receive.svg'
import SendSvg from '../../assets/send.svg'
// const data = [
//   { title: '收货', icon: 'pic-left', path: '/TD/Receive' },
//   { title: '发货', icon: 'pic-right', path: '/TD/Send' },
//   { title: '入库', icon: 'login', path: '/TD/InStorage' },
//   { title: '出库', icon: 'logout', path: '/TD/OutStorage' }
// ];

export default {
  components:{
    InSvg,
    OutSvg,
    ReceiveSvg,
    SendSvg
  },
  data() {
    return {
      
    };
  }
};
</script>
<style>
.svg{
  width:60%;
}
</style>