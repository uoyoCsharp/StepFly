<template>
	<view class="container">
		<view class="tui-status-bar"></view>
		<view class="tui-page-title">登录</view>
		<text class="sub-title">请使用您乐心健康的手机号码和密码进行登录</text>
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
				<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
					<view class="tui-cell-input">
						<tui-icon name="shield" color="#6d7a87" :size="20"></tui-icon>
						<input placeholder="请输入密码" placeholder-class="tui-phcolor" :value="code" type="password" maxlength="20" @input="inputCode" />
					</view>
				</tui-list-cell>
			</view>
			<view class="tui-btn-box">
				<tui-button :disabledGray="true" :shadow="true" shape="circle" @tap="login" :disabled="loginButtondisabled">登录</tui-button>
			</view>

			<tui-button :disabledGray="true" plain link @tap="goLogin" style="margin-top:25rpx">使用验证码登录</tui-button>
		</view>

		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
		<tui-footer :fixed="true" :copyright="'进行登录操作时，请确保您已经在乐心APP设置了密码'"></tui-footer>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import { Guid } from "guid-typescript";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import uniHelper, { thorUiHelper } from '../../common/uniHelper';
import { MiCakeApiModel, ServerUrl } from "@/common/environment";
import tuiFooter from "@/components/thorui/tui-footer/tui-footer.vue";
import ApiHelper from "@/utils/apiHelper";
import { SendPhoneCodeModel, LoginToLeXinModel, LoginToLeXinWithPwdModel, LoginResultModel } from '@/models/apiModel';
import { Md5 } from "md5-typescript";
import store from "@/store";
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';

@Component({
	components: {
		tuiListCell,
		tuiIcon,
		tuiButton,
		tuiTips,
		tuiFooter
	},
})
export default class extends Vue {
	public loginButtondisabled: boolean = true;

	public mobile: string = '';
	public code: string = '';


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
		this.loginButtondisabled = false;
	}

	public inputMobile(e: any) {
		this.mobile = e.detail.value;
		if (uniHelper.validator.isMobile(this.mobile)) {
			setTimeout(() => { this.phoneInputBlur(); }, 300);
		}
	}

	public inputCode(e: any) {
		this.code = e.detail.value;
	}

	public async login() {
		if (!uniHelper.validator.isMobile(this.mobile)) {
			thorUiHelper.showTips(this.$refs.toast, '貌似手机号码不正确');
			return;
		}

		if (this.code.length < 3) {
			thorUiHelper.showTips(this.$refs.toast, '请填写正确的密码');
			return;
		}

		let dto = new LoginToLeXinWithPwdModel();
		dto.phone = this.mobile;
		dto.password = Md5.init(this.code);

		uniHelper.showLoading(undefined, true);
		let loginResponse = await this.$httpClient.post<MiCakeApiModel<LoginResultModel>>(`/StepFly/LoginToLeXinByPassword`, dto);
		await uniHelper.hideLoading(1500);

		if (loginResponse.result!) {

			thorUiHelper.showTips(this.$refs.toast, '登录成功，即将跳转', 2000, 'green');

			this.storeLoginInfo(loginResponse.result!.token!);

			setTimeout(() => { uni.navigateTo({ url: `/pages/menu/index` }); }, 1500);

		} else {
			thorUiHelper.showTips(this.$refs.toast, '登录失败，请联系管理员反馈');
		}
	}

	goLogin() {
		uni.navigateTo({ url: `/pages/login/loginLeXinUseCode` });
	}

	private storeLoginInfo(token: string) {
		var storeInstance = getModule(UserStoreModule, this.$store);
		storeInstance.loginSuccessAction({ name: this.mobile, token: token });
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
