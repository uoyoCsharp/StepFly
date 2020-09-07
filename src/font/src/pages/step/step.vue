<template>
  <view class="container">
    <view class="tui-header">
      <view class="tui-title">欢迎</view>
      <view class="tui-sub-title">{{ phone }}</view>
    </view>
    <view class="tui-form">
      <view class="tui-view-input">
        <text class="sub-title" style="margin-left: 10rpx">请在下方填写您想要更改的运动步数：</text>
        <tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
          <view class="tui-cell-input">
            <tui-icon name="explore-fill" color="green" :size="20"></tui-icon>
            <input :adjust-position="false" :value="step" placeholder="填写想要的步数" placeholder-class="tui-phcolor" type="number" maxlength="8" @input="inputStep" />
            <view class="tui-ml-auto" @tap="randomStep">
              <tui-tag padding="10rpx 12rpx" margin="0 30rpx 0 0" size="24rpx" type="light-green" shape="circle">随机</tui-tag>
            </view>
          </view>
        </tui-list-cell>

        <view class="visualize-container">
          <!-- v-if="stepOfMarathonPercent>0" -->
          <view class="tui-progress-box">
            <tui-circular-progress
              :fontShow="false"
              :percentage="stepOfMarathonPercent"
              :sAngle="sAngle"
              :diam="240"
              :height="130"
              :lineWidth="12"
              progressColor="#19be6b"
              fontColor="#19be6b"
              defaultColor="rgba(25,190,107,0.1)"
              @change="progressChange"
            >
              <view class="tui-progress-text">
                <view>相对于马拉松比值</view>
                <view class="tui-progress-num">{{ progress }}%</view>
              </view>
            </tui-circular-progress>
          </view>
        </view>

        <view class="tui-btn-box">
          <tui-button :disabledGray="true" :shadow="true" :disabled="submitButtonDisable" shape="circle" @tap="submitStep">提交</tui-button>
          <tui-button :disabledGray="true" v-if="!submitCompleted" plain link shape="circle" @tap="backHome" style="margin-top: 5rpx">回到首页</tui-button>
        </view>
      </view>
    </view>

    <!--居中消息-->
    <tui-tips position="center" ref="toast"></tui-tips>
  </view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiCircularProgress from "@/components/thorui/tui-circular-progress/tui-circular-progress.vue";
import { LeXinChangeStepModel } from '@/models/apiModel';
import { MiCakeApiModel } from '@/common/environment';
import NumberHelper from "@/utils/numberHelper";

@Component({
  components: {
    tuiTips,
    tuiListCell,
    tuiIcon,
    tuiButton,
    tuiTag,
    tuiCircularProgress
  }
})
export default class extends Vue {
  public step: string = '';
  public submitButtonDisable: boolean = false;
  public submitCompleted: boolean = false;

  public sAngle: number = -Math.PI
  public progress: number = 0;
  public stepOfMarathonPercent: number = 0;

  public phone: string = '1518888888';

  public async onLoad() {
  }

  public backHome() {
    uni.reLaunch({ url: '/pages/index/index' });
  }

  public async submitStep() {
    let stepNum = parseInt(this.step);

    if (stepNum > 100000) {
      thorUiHelper.showTips(this.$refs.toast, '老铁，要不还是直接选择飞天吧？');
      return;
    }

    let dto = new LeXinChangeStepModel();
    dto.phone = this.phone;
    dto.step = stepNum;

    uniHelper.showLoading(undefined, true);
    let changeResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/StepFly/ChangeStepByLeXin`, dto);
    await uniHelper.hideLoading(1500);

    if (changeResponse.result!) {
      thorUiHelper.showTips(this.$refs.toast, '修改成功，请到对应的平台进行查看', 2000, 'green');

      this.submitCompleted = true;
      this.submitButtonDisable = true;
    } else {
      thorUiHelper.showTips(this.$refs.toast, `修改失败，可以联系管理员进行反馈：${changeResponse.message}`);
    }
  }

  private progressIsBusy: boolean = false;
  private stepVisualization(step: number) {
    if (!this.progressIsBusy) {
      this.progressIsBusy = true;

      setTimeout(() => {
        var marathonCount = step / 40000;
        this.stepOfMarathonPercent = Number(marathonCount.toFixed(2)) * 50;
        this.progressIsBusy = false;
      }, 400);
    }
  }

  public randomStep() {
    this.step = NumberHelper.getRandomNumInt(10000, 60000).toString();
    this.stepVisualization(Number(this.step));
  }

  public progressChange(e: any) {
    //半圆 进度 * 2
    this.progress = e.percentage * 2;
  }

  public clearInput(type: number) {
    this.step = '';
  }

  public inputStep(e: any) {
    this.step = e.detail.value;
    this.stepVisualization(Number(this.step));
  }
}
</script>

<style lang="scss">
.container {
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
    color: $uni-color-primary;
    padding-top: 18rpx;
  }

  .visualize-container {
    width: 100%;
    display: flex;
    justify-content: center;

    .tui-progress-box {
      padding: 0 30rpx;
      box-sizing: border-box;
      display: flex;
      align-items: center;
    }

    .tui-progress-text {
      width: 100%;
      height: 130px;
      font-size: 28rpx;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      padding-top: 40rpx;
      position: absolute;
      left: 0;
      top: 0;
    }

    .tui-progress-num {
      font-size: 60rpx;
      padding-top: 20rpx;
    }
  }

  .tui-status-bar {
    width: 100%;
    height: var(--status-bar-height);
  }

  .tui-header {
    width: 100%;
    padding: 40rpx;
    display: flex;
    align-items: center;
    justify-content: space-between;
    box-sizing: border-box;
  }

  .sub-title {
    font-size: 24rpx;
    color: #7a7a7a;
    padding-top: 18rpx;
  }

  .tui-page-title {
    width: 100%;
    font-size: 48rpx;
    font-weight: bold;
    color: $uni-text-color;
    line-height: 42rpx;
    padding: 40rpx;
    box-sizing: border-box;
  }

  .tui-form {
    padding-top: 50rpx;

    .tui-view-input {
      width: 100%;
      box-sizing: border-box;
      padding: 0 40rpx;

      .tui-cell-input {
        width: 100%;
        display: flex;
        align-items: center;
        padding-top: 48rpx;
        padding-bottom: $uni-spacing-col-base;

        input {
          flex: 1;
          padding-left: $uni-spacing-row-base;
        }

        .tui-ml-auto {
          margin-left: auto;
        }

        .tui-gray {
          color: $uni-text-color-placeholder;
        }
      }
    }

    .tui-cell-text {
      width: 100%;
      padding: $uni-spacing-col-lg $uni-spacing-row-lg;
      box-sizing: border-box;
      font-size: $uni-font-size-sm;
      color: $uni-text-color-grey;
      display: flex;
      align-items: center;
      justify-content: space-between;

      .tui-color-primary {
        color: $uni-color-primary;
      }
    }

    .tui-btn-box {
      width: 100%;
      padding: 0 $uni-spacing-row-lg;
      box-sizing: border-box;
      margin-top: 80rpx;
    }
  }

  .tui-login-way {
    width: 100%;
    font-size: 26rpx;
    color: $uni-color-primary;
    display: flex;
    justify-content: center;
    position: fixed;
    left: 0;
    bottom: 80rpx;

    view {
      padding: 12rpx 0;
    }
  }
}
</style>
