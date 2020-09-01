<template>
	<view class="container">
		<view class="tui-status-bar"></view>
		<view class="tui-page-title">登录</view>
		<text class="sub-title">请使用您已经注册过乐心健康的手机号码进行登录</text>
		<view class="tui-form">
			<view class="tui-view-input">
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="mobile" color="#6d7a87" :size="20"></tui-icon>
						<input :adjust-position="false" :value="mobile" placeholder="请输入手机号" placeholder-class="tui-phcolor" type="number" maxlength="11" @input="inputMobile" />
						<view class="tui-icon-close" v-show="mobile" @tap="clearInput(1)">
							<tui-icon name="close-fill" :size="16" color="#bfbfbf"></tui-icon>
						</view>
					</view>
				</tui-list-cell>
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent" v-if="showCodeImg">
					<view class="tui-cell-input">
						<tui-icon name="shield" color="#6d7a87" :size="20"></tui-icon>
						<input placeholder="请输入图形验证码" placeholder-class="tui-phcolor" :value="imgCode" type="text" maxlength="6" @input="inputImgCode" />
						<view class="tui-btn-send">
							<img :src="validateCodeUrl" @tap="refreshImgCode" />
						</view>
					</view>
				</tui-list-cell>
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="shield" color="#6d7a87" :size="20"></tui-icon>
						<input placeholder="短信验证码" placeholder-class="tui-phcolor" :value="code" type="text" maxlength="6" @input="inputCode" :disabled="codeInputDisabled" />
						<view class="tui-btn-send" @tap="sendCode" :class="{ 'tui-gray': isSend }">{{ btnSendText }}</view>
					</view>
				</tui-list-cell>
			</view>
			<view class="tui-btn-box">
				<tui-button :disabledGray="true" :shadow="true" shape="circle" @tap="login" :disabled="loginButtondisabled">登录</tui-button>
			</view>
		</view>

		<tui-modal :show="hasLoginRecord" @click="modalClick" @cancel="hideModal" title="提示" content="您在最近已经登录过系统，是否使用以往的登录记录？" :maskClosable="false"></tui-modal>
		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import { Guid } from "guid-typescript";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiModal from "@/components/thorui/tui-modal/tui-modal.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import uniHelper, { thorUiHelper } from '../../common/uniHelper';
import { MiCakeApiModel, ServerUrl } from "@/common/environment";
import ApiHelper from "@/utils/apiHelper";
import { SendPhoneCodeModel, LoginToLeXinModel } from '@/models/apiModel';

@Component({
	components: {
		tuiListCell,
		tuiIcon,
		tuiButton,
		tuiTips,
		tuiModal
	},
})
export default class extends Vue {
	public loginButtondisabled: boolean = false;

	public get codeInputDisabled(): boolean {
		return !(uniHelper.validator.isMobile(this.mobile) && this.imgCode.length > 3);
	}

	public mobile: string = '';
	public code: string = '';
	public imgCode: string = '';

	public showCodeImg: boolean = false;
	public validateCodeUrl: string | null = null;
	public isSend: boolean = false;
	public get btnSendText(): string {
		if (this.isSend) {
			return `${this.countDown}s`;
		} else {
			return `获取验证码`;
		}
	}

	public hasLoginRecord: boolean = false;

	//发送验证码倒计时
	private countDown: number = 90;

	public clearInput(type: number) {
		if (type == 1) {
			this.mobile = '';
		}
	}

	public async phoneInputBlur() {
		if (!uniHelper.validator.isMobile(this.mobile)) {
			thorUiHelper.showTips(this.$refs.toast, '请输入正确的手机号码');
			return;
		}

		var hasLoginResponse = await this.$httpClient.get<MiCakeApiModel<boolean>>(`/StepFly/HasLoginInfoWithLeXin?phone=${this.mobile}`);

		if (hasLoginResponse.result!) {
			this.hasLoginRecord = true;
		} else {
			await this.getValidateCodeImg();
		}
	}

	public async modalClick(e: any) {
		let index = e.index;
		if (index === 1) {
			uni.navigateTo({ url: `/pages/step/step?phone=${this.mobile}` });

			this.storeLoginInfo();
		} else {
			await this.getValidateCodeImg();
		}
		this.hideModal();
	}

	public async refreshImgCode() {
		await this.getValidateCodeImg();
	}

	private async getValidateCodeImg() {
		//获取并显示图形验证码
		try {
			uniHelper.showLoading('请稍等', true);
			this.showCodeImg = true;

			let validateCodeUrl = ServerUrl + `/LeXin/GetValidateCodeImg?phone=${this.mobile}`;
			let imgData = await this.$httpClient.get(validateCodeUrl, undefined, {}, { responseType: 'arraybuffer' });
			this.validateCodeUrl = ApiHelper.getImgBase64Data(imgData);
		} finally {
			await uniHelper.hideLoading();
		}
	}

	public hideModal() {
		this.hasLoginRecord = false;
	}

	public inputMobile(e: any) {
		this.mobile = e.detail.value;
		if (uniHelper.validator.isMobile(this.mobile)) {
			setTimeout(() => { this.phoneInputBlur(); }, 300);
		} else {
			this.showCodeImg = false;
		}
	}

	public inputCode(e: any) {
		this.code = e.detail.value;
	}

	public inputImgCode(e: any) {
		this.imgCode = e.detail.value;
	}

	public async sendCode() {
		if (!uniHelper.validator.isMobile(this.mobile)) {
			thorUiHelper.showTips(this.$refs.toast, '貌似手机号码不正确');
			return;
		}

		if (this.imgCode.length < 3) {
			thorUiHelper.showTips(this.$refs.toast, '请输入正确的图形验证码');
			return;
		}

		let dto = new SendPhoneCodeModel();
		dto.phone = this.mobile;
		dto.imageCode = this.imgCode;

		var sendResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/LeXin/SendPhoneCode`, dto);
		if (sendResponse.result!) {
			thorUiHelper.showTips(this.$refs.toast, '验证码发送成功', 2000, 'green');
			this.isSend = true;

			let handler = setInterval(() => {
				this.countDown--
				if (this.countDown < 1) {
					this.isSend = false;
					clearInterval(handler);
				}
			}, 1000);
		} else {
			thorUiHelper.showTips(this.$refs.toast, `验证码发送失败: ${sendResponse.message}`);
		}
	}

	public async login() {
		if (!uniHelper.validator.isMobile(this.mobile)) {
			thorUiHelper.showTips(this.$refs.toast, '貌似手机号码不正确');
			return;
		}

		if (this.code.length < 3) {
			thorUiHelper.showTips(this.$refs.toast, '请填写正确的短信验证码');
			return;
		}

		let dto = new LoginToLeXinModel();
		dto.phone = this.mobile;
		dto.code = this.code;

		uniHelper.showLoading(undefined, true);
		let loginResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/StepFly/LoginToLeXinByCode`, dto);
		await uniHelper.hideLoading(1500);

		if (loginResponse.result!) {

			thorUiHelper.showTips(this.$refs.toast, '登录成功，即将跳转', 2000, 'green');

			this.storeLoginInfo();

			setTimeout(() => {
				uni.navigateTo({ url: `/pages/step/step?phone=${this.mobile}` });
			}, 1500);

		} else {
			thorUiHelper.showTips(this.$refs.toast, '登录失败，请联系管理员反馈');
		}
	}

	private storeLoginInfo() {
		uni.setStorageSync('login', true);
		uni.setStorageSync('login-phone', this.mobile);
		uni.setStorageSync('login-time', new Date().getTime().toString());
	}

	public back() {
		uni.navigateBack({
			animationType: 'pop-out',
			animationDuration: 300
		});
	}
}
</script>

<style lang="scss">
.container {
	.tui-status-bar {
		width: 100%;
		height: var(--status-bar-height);
	}

	.sub-title {
		font-size: 24rpx;
		color: #7a7a7a;
		padding-top: 18rpx;
		margin-left: 40rpx;
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
