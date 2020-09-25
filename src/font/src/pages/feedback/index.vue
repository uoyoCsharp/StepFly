<template>
  <view class="tui-container">
    <view class="tui-header">
      <view class="tui-title">意见/问题反馈</view>
      <view class="tui-sub-title">您的意见对我们很宝贵</view>
    </view>
    <view class="tui-form">
      <input class="tui-input" placeholder-class="phcolor" placeholder="请输入标题" v-model="title" maxlength="50" />

      <view class="tui-item-box">
        <view class="cell-title tui-flex tui-col-2">类别:</view>
        <picker class="tui-flex tui-col-10" @change="pickerChange" :value="index" :range="tagArr">
          <view class="pickerContent">{{tagArr[index]}}</view>
        </picker>
      </view>

      <view class="tui-cells">
        <textarea class="tui-textarea" placeholder="如果您在使用中遇到什么问题，可以在此处填写。如果有需要，可以在内容中包含您的联系方式，管理员会与您联系。同时，您有什么建议也可以在这儿填写。" maxlength="500" placeholder-class="phcolor" v-model="content" />
      </view>
      <view class="tui-btn-box">
        <tui-button shape="circle" :shadow="true" @click="submit">提交反馈</tui-button>
      </view>
    </view>

    <!--居中消息-->
    <tui-tips position="center" ref="toast"></tui-tips>
  </view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import { MiCakeApiModel } from '@/common/environment';
import { AddFeedbackModel } from '@/models/apiModel';

@Component({
  components: {
    tuiButton,
    tuiTips
  }
})
export default class extends Vue {
  title: string = '';
  content: string = '';

  tagArr: string[] = ['建议', '问题反馈', '随便水水经验'];
  index: number = 0;

  async submit() {
    if (uniHelper.validator.isNullOrEmpty(this.title)) {
      thorUiHelper.showTips(this.$refs.toast, '请输入标题');
      return;
    }

    if (uniHelper.validator.isNullOrEmpty(this.content)) {
      thorUiHelper.showTips(this.$refs.toast, '请填写内容');
      return;
    }

    var storeInstance = getModule(UserStoreModule, this.$store);
    if (uniHelper.validator.isNullOrEmpty(storeInstance.user.name)) {
      uni.navigateTo({ url: '/pages/login/loginLeXin' });
      return;
    }
	uniHelper.showLoading('提交中...');
	
    var dto = new AddFeedbackModel();
    dto.userKey = storeInstance.user.name!;
    dto.title = this.title;
    dto.content = this.content;
    dto.tag = this.tagArr[this.index];
    let apiResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/Feedback/Add`, dto);

    await uniHelper.hideLoading(1500);

    if (!apiResponse.result) {
      thorUiHelper.showTips(this.$refs.toast, '您近期已经提交过反馈了，请明天再试吧~');
    } else {
	  thorUiHelper.showTips(this.$refs.toast, '感谢您的反馈~', 2000, 'green');
	  
	  setTimeout(() => {
		  uni.navigateTo({ url: '/pages/menu/index' });
	  }, 1500);
    }
  }

  pickerChange(e: any) {
    this.index = e.detail.value
  }
}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";

page {
  background: #fff;
}

.cell-title {
  font-size: 30rpx;
  color: #666;
  padding-bottom: 32rpx;
  padding-left: 20rpx;
}

.tui-form {
  padding: 50rpx;

  .tui-btn-box {
    width: 100%;
    padding: 0 $uni-spacing-row-lg;
    box-sizing: border-box;
    padding-top: 80rpx;
  }
}

.tui-cells {
  border: 1rpx solid #e6e6e6;
  border-radius: 4rpx;
  box-sizing: border-box;
  padding: 20rpx 20rpx 0 20rpx;
  margin-top: 50rpx;
}

.tui-textarea {
  height: 400rpx;
  width: 100%;
  color: #666;
  font-size: 28rpx;
}

.pholder {
  color: #ccc;
}

.textarea-counter {
  font-size: 24rpx;
  color: #999;
  text-align: right;
  height: 40rpx;
  line-height: 40rpx;
  padding-top: 4rpx;
}

.top64 {
  margin-top: 64rpx;
}

.tui-input {
  font-size: 30rpx;
  height: 88rpx;
  border: 1rpx solid #e6e6e6;
  border-radius: 4rpx;
  padding: 0 25rpx;
  box-sizing: border-box;
}

.tui-item-box {
  margin-top: 50rpx;
  width: 100%;
  display: flex;
  align-items: center;
}

.pickerContent {
  font-size: 30rpx;
  color: #5677fc;
  width: 100%;
  padding-bottom: 32rpx;
}
</style>
