<template>
	<view class="container">
		<view class="header">
			<view class="title">欢迎</view>
			<view class="sub-title" style="color:red">{{phone}}</view>
		</view>

		<view class="tui-form">
			<view class="tui-view-input">
				<text class="sub-title" style="margin-left:10rpx">请在下方填写您想要更改的运动步数：</text>
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="explore" color="#6d7a87" :size="20"></tui-icon>
						<input :adjust-position="false" :value="step" placeholder="填写想要的步数" placeholder-class="tui-phcolor" type="number" maxlength="8" @input="inputStep" />
						<view class="tui-icon-close" v-show="step" @tap="clearInput(1)">
							<tui-icon name="close-fill" :size="16" color="#bfbfbf"></tui-icon>
						</view>
					</view>
				</tui-list-cell>

				<view class="tui-btn-box">
					<tui-button :disabledGray="true" :shadow="true" :disabled="submitButtonDisable" shape="circle" @tap="submitStep">提交</tui-button>
					<tui-button :disabledGray="true" v-if="submitCompleted" :shadow="true" type="green" shape="circle" @tap="backHome" style="margin-top:5rpx">回到首页</tui-button>
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
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import { LeXinChangeStepModel } from '@/models/apiModel';
import { MiCakeApiModel } from '@/common/environment';
import NumberHelper from "@/utils/numberHelper";

@Component({
	components: {
		tuiTips,
		tuiListCell,
		tuiIcon,
		tuiButton,
	}
})
export default class extends Vue {
	public step: string = '';
	public submitButtonDisable: boolean = false;
	public submitCompleted: boolean = false;

	private phone: string = '';

	public async onLoad(options: any) {
		var phone = options.phone;
		if (!uniHelper.validator.isMobile(phone)) {
			thorUiHelper.showTips(this.$refs.toast, '严重错误，未找到手机号码！');

			setTimeout(() => {
				uni.navigateTo({ url: `/pages/index/index` })
			}, 1500);
		}

		this.step = NumberHelper.getRandomNumInt(30000, 50000).toString();
		this.phone = phone;
	}

	public backHome() {
		uni.navigateTo({
			url: '/pages/index/index'
		});
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

	public clearInput(type: number) {
		this.step = '';
	}

	public inputStep(e: any) {
		this.step = e.detail.value;
	}
}
</script>

<style lang="scss">
.container {
	.header {
		padding: 80rpx 90rpx 60rpx 90rpx;
		box-sizing: border-box;
	}

	.title {
		font-size: 34rpx;
		color: #333;
		font-weight: 500;
	}

	.sub-title {
		font-size: 24rpx;
		color: #7a7a7a;
		padding-top: 18rpx;
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

				.tui-icon-close {
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
