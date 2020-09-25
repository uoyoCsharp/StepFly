<template>
  <view class="tui-container">
    <view class="tui-header">
      <view class="tui-title">刷步记录</view>
      <view class="tui-sub-title"></view>
    </view>
    <tui-list-view class="tui-default">
      <tui-list-cell :arrow="false" :hover="false" v-for="item in source" :key="item.id">
        <view class="tui-item-box">
          <text class="list-header">{{ item.userKeyInfo }}</text>
        </view>
        <view class="tui-item-box" style="margin-top: 30rpx">
          <text>步数:</text>
          <tui-tag padding="8rpx" size="24rpx" type="light-orange">{{ item.stepNum}} 步</tui-tag>
        </view>
        <view class="tui-item-box" style="margin-top: 30rpx">
          <text>更改时间:</text>
          <tui-tag padding="8rpx" size="24rpx" type="light-green">{{item.creationTime |dateformat}}</tui-tag>
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
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyHistoryModel } from '@/models/apiModel';

@Component({
  components: {
    tuiTips,
    tuiLoadmore,
    tuiListCell,
    tuiListView,
    tuiTag
  }
})
export default class extends Vue {
  currentIndex: number = 1;
  loading: boolean = false;
  isEnd: boolean = false;

  source: StepFlyHistoryModel[] = [];

  pageNum: number = 10;

  public async onLoad() {
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
    let apiResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyHistoryModel[]>>(`/StepFlyHistory/Get?pageIndex=${this.currentIndex}&pageNum=${this.pageNum}`);

    if (apiResponse.result) {
      var records = apiResponse.result!;
      records.forEach(element => { this.source.push(element); });

      if (records.length < this.pageNum)
        this.isEnd = true;
    }
  }
}
</script>

<style lang="scss">
page {
  background:$uni-bg-color-setoff;
}
.tui-default {
  padding: 20rpx 30rpx;
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
</style>
