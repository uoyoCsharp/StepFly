<template>
	<view class="tui-container">
		<view class="tui-header">
			<view class="tui-title">打赏作者</view>
			<view class="tui-sub-title">给创作者一点小激励</view>
		</view>

		<view class="tui-flex img-container">
			<image src="/static/imgs/reward_zfb.jpg" mode="widthFix" class="zfb-img" />
            <text class="reward-text">长按图片进行保存</text>
		</view>
		<view class="reward-text">
			温馨提示：打赏超过五元时，请在转账中备注您的登录账号，我们将赠送您
			<span style="color:red;font-weight:bold">三个月的VIP会员</span>作为奖励。
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

}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";

page {
	background: $uni-bg-color-setoff;
}

.reward-text {
	padding: 20rpx 30rpx;
	box-sizing: border-box;
	color: #b3b3b3;
	font-size: 26rpx;
}

.img-container {
	justify-content: center;
    flex-direction: column;
    align-items: center;
}

.zfb-img {
	width: 70%;
}
</style>
