<script lang="ts">
import Vue from 'vue';
export default Vue.extend({
  mpType: 'app',
  onLaunch() {
    console.log('App Launch')
  },
  onShow() {
    console.log('App Show')
  },
  onHide() {
    console.log('App Hide')
  }
});
</script>

<style>
/*每个页面公共css */
.tui-container {
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
  padding-bottom: env(safe-area-inset-bottom);
}

.tui-header {
  padding: 80rpx 60rpx;
}

.tui-title {
  font-size: 36rpx;
  color: #333;
  font-weight: bold;
}

.tui-sub-title {
  font-size: 28rpx;
  color: #7a7a7a;
  padding-top: 18rpx;
}
</style>
