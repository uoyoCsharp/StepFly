<template>
  <view class="tui-container">
    <view class="tui-header">
      <view class="tui-title">用户管理</view>
      <view class="tui-sub-title"></view>
    </view>
    <tui-card :title="{text: '总注册人数'}">
      <template v-slot:body>
        <view class="tui-default tui-flex">
          <tui-tag type="light-green" padding="8rpx 12rpx" size="24rpx">{{userCount}}人</tui-tag>
        </view>
      </template>
    </tui-card>

    <tui-list-view style="margin-top: 10rpx" class="tui-default">
      <tui-list-cell :arrow="false" :hover="false" v-for="item in source" :key="item.userKeyInfo">
        <view class="tui-item-box">
          <text class="list-header">{{ item.userKeyInfo }}</text>
        </view>
        <view class="tui-item-box" style="margin-top: 30rpx">
          <text>注册时间:</text>
          <tui-tag padding="8rpx" size="24rpx" type="light-orange">{{ item.loginTime | dateformat}}</tui-tag>
        </view>
        <view class="tui-item-box" style="margin-top: 30rpx">
          <text>最新登录时间:</text>
          <tui-tag padding="8rpx" size="24rpx" type="light-green">{{item.modificationTime |dateformat}}</tui-tag>
        </view>
      </tui-list-cell>
    </tui-list-view>
    <tui-tips ref="toast"></tui-tips>
    <!--加载loading-->
    <tui-loadmore v-if="loading" :index="3" type="primary"></tui-loadmore>
  </view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiLoadmore from "@/components/thorui/tui-loadmore/tui-loadmore.vue";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiListView from "@/components/thorui/tui-list-view/tui-list-view.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiCard from "@/components/thorui/tui-card/tui-card.vue";
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyUserItemModel } from '@/models/apiModel';

@Component({
  components: {
    tuiTips,
    tuiLoadmore,
    tuiListCell,
    tuiListView,
    tuiTag,
    tuiCard
  }
})
export default class extends Vue {
  currentIndex: number = 1;
  loading: boolean = false;
  isEnd: boolean = false;

  source: StepFlyUserItemModel[] = [];
  userCount: number = 0;

  pageNum: number = 10;

  public async onLoad() {
    this.getUserCount();
    this.getSource();
  }

  // 页面上拉触底事件的处理函数
  async onReachBottom() {
    if (this.isEnd)
      return;

    this.loading = true;
    this.currentIndex++;
    await this.getSource();

    this.loading = false;
  }

  private async getSource() {
    //获取已经绑定的平台
    let apiResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyUserItemModel[]>>(`/StepFlyUser/GetList?pageIndex=${this.currentIndex}&pageNum=${this.pageNum}`);

    if (apiResponse.result) {
      var records = apiResponse.result!;
      records.forEach(element => { this.source.push(element); });

      if (records.length < this.pageNum)
        this.isEnd = true;
    }
  }

  private async getUserCount() {
    let apiResponse = await this.$httpClient.get<MiCakeApiModel<number>>(`/StepFlyUser/GetCount`);
    if (apiResponse.result) {
      this.userCount = apiResponse.result!;
    }
  }
}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";
page {
  background: $uni-bg-color-setoff;
}

.tui-item-box {
  width: 100%;
  display: flex;
  align-items: center;
}

.list-header {
  font-size: 30rpx;
  color: #333;
  font-weight: bold;
}

.tui-default {
  padding: 20rpx 30rpx;
}
</style>
