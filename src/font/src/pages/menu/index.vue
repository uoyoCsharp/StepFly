<template>
  <view class="container">
    <view class="tui-header">
      <view class="tui-title">Step-Fly</view>
      <view class="tui-sub-title">菜单导航</view>
    </view>
    <tui-grid>
      <block v-for="(item,index) in routers" :key="index">
        <tui-grid-item>
          <view class="tui-grid-icon" @tap="goMenu(item)">
            <image
              class="tui-grid-img"
              :src="'/static/menus/'+item.img+'.png'"
              :style="{width:item.width+'rpx',height:item.height+'rpx'}"
            />
          </view>
          <text class="tui-grid-label">{{item.name}}</text>
        </tui-grid-item>
      </block>
    </tui-grid>

    <!--居中消息-->
    <tui-tips position="center" ref="toast"></tui-tips>
  </view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiGrid from "@/components/thorui/tui-grid/tui-grid.vue";
import tuiGridItem from "@/components/thorui/tui-grid-item/tui-grid-item.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import { thorUiHelper } from '@/common/uniHelper';

@Component({
  components: {
    tuiGrid,
    tuiGridItem,
    tuiTips
  }
})
export default class extends Vue {

  routers: MenuItem[] = [{
    name: '更改步数',
    url: '/pages/step/step',
    img: "badge",
    width: 58,
    height: 58,
    isOpen: true,
  }, {
    name: '自动更改',
    url: '',
    img: "layout",
    width: 64,
    height: 50,
    isOpen: false
  }];

  public goMenu(item: MenuItem) {
    if (!item.isOpen) {
      thorUiHelper.showTips(this.$refs.toast, '该功能暂未开放，敬请期待');
      return;
    }

    uni.navigateTo({ url: item.url! });
  }
}

interface MenuItem {
  name: string | undefined;
  url: string | undefined;
  img: string | undefined;
  width: number;
  height: number;
  isOpen: boolean;
}
</script>

<style>
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

.tui-grid-icon {
  width: 100%;
  height: 74rpx;
  display: flex;
  align-items: center;
  justify-content: center;
}

.tui-grid-label {
  margin-top: 10rpx;
  display: block;
  text-align: center;
  font-weight: 400;
  color: #333;
  font-size: 28rpx;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
