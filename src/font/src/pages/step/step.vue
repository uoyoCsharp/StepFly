<template>
	<view class="container">
		<view class="tui-header">
			<view class="tui-title">欢迎</view>
			<view class="tui-sub-title">{{ phone }}</view>
		</view>
		<view class="tui-form">
			<view class="tui-view-input">
				<tui-list-view title="运动步数" color="#777">
					<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
						<view class="tui-cell-input">
							<tui-icon name="explore-fill" color="green" :size="20"></tui-icon>
							<input :adjust-position="false" :value="step" placeholder="填写想要的步数" placeholder-class="tui-phcolor" type="number" maxlength="6" @input="inputStep" />
							<view class="tui-ml-auto" @tap="randomStep">
								<tui-tag padding="10rpx 12rpx" margin="0 30rpx 0 0" size="24rpx" type="light-green" shape="circle">随机</tui-tag>
							</view>
						</view>
					</tui-list-cell>
				</tui-list-view>

				<tui-list-view title="比值" color="#777">
					<tui-list-cell :hover="false" :lineLeft="false" backgroundColor="transparent">
						<view class="visualize-container">
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
					</tui-list-cell>
				</tui-list-view>
				<view class="tui-btn-box">
					<tui-button :disabledGray="true" :shadow="true" :disabled="submitButtonDisable" shape="circle" @tap="submitStep">提交</tui-button>
					<tui-button :disabledGray="true" v-if="submitCompleted" plain link shape="circle" @tap="backHome" style="margin-top: 5rpx">回到首页</tui-button>
				</view>
			</view>
		</view>

		<view v-if="showWarning" class="tui-flex warning-text tui-center">
			<text>现在是上午{{currentTime}}，太早提交步数可能存在无效的情况!</text>
		</view>

		<tui-modal :show="showNoVip" :custom="true" @cancel="hideNoVipModal">
			<view class="tui-modal-custom">
				<image src="/static/imgs/fail.png" class="tui-tips-img" />
				<view class="tui-modal-custom-text">{{noVipText}}</view>
				<tui-button height="72rpx" :size="28" type="danger" shape="circle" @click="goOpenVip">去获取VIP</tui-button>
			</view>
		</tui-modal>
		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiListView from "@/components/thorui/tui-list-view/tui-list-view.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiModal from "@/components/thorui/tui-modal/tui-modal.vue";
import tuiCircularProgress from "@/components/thorui/tui-circular-progress/tui-circular-progress.vue";
import { ChangeStepResultModel, ChangeStepModel, StepFlyHistoryModel } from '@/models/apiModel';
import { MiCakeApiModel } from '@/common/environment';
import NumberHelper from "@/utils/numberHelper";
import TimeHelper from "@/utils/timeHelper";
import PageHelper from "../helper/pageHelper";
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';
import { format, getHours, Locale } from 'date-fns'
import { VipInfoStoreModule } from '@/store/vipInfo/vipInfoStore';

@Component({
	components: {
		tuiTips,
		tuiListCell,
		tuiListView,
		tuiIcon,
		tuiButton,
		tuiTag,
		tuiCircularProgress,
		tuiModal
	}
})
export default class extends Vue {
	public step: number = 0;
	public submitButtonDisable: boolean = false;
	public submitCompleted: boolean = false;

	public sAngle: number = -Math.PI
	public progress: number = 0;
	public stepOfMarathonPercent: number = 0;

	public phone: string = '';

	public currentTime?: string;
	public showWarning: boolean = false;

	private isVip: boolean = false;
	private platform?: 'lexin' | 'xiaomi';
	private todayRecord: TodayChangeRecord = { hasChanged: false };

	public noVipText?: string = '';

	public async onLoad() {
		//获取当前时间
		var currentHour = getHours(new Date());
		if (currentHour < 8) {
			this.currentTime = format(new Date(), 'HH:mm:ss');
			this.showWarning = true;
		}

		var vipInfoStoreInstance = getModule(VipInfoStoreModule, this.$store);
		this.isVip = vipInfoStoreInstance.vipInfo.isVip ?? false;

		var storeInstance = getModule(UserStoreModule, this.$store);
		if (uniHelper.validator.isNullOrEmpty(storeInstance.user.name)) {
			uni.navigateTo({ url: '/pages/login/login' });
			return;
		}
		this.phone = storeInstance.user.name!;
		this.platform = storeInstance.user.platform!;

		await this.getLastRecord();
		this.randomStep();
	}

	private async getLastRecord() {
		const url = `/StepFlyHistory/MyLastHistory?userKey=${this.phone}&type=${PageHelper.getPlatformCode(this.platform)}`;
		let changeResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyHistoryModel>>(url);

		var result = changeResponse.result;
		if (result && TimeHelper.isToday(new Date(result.creationTime!))) {
			this.todayRecord = { hasChanged: true, ChangedTime: result.creationTime!, ChangedNum: result.stepNum };
		}
	}

	public backHome() {
		uni.reLaunch({ url: '/pages/index/index' });
	}

	public async submitStep() {
		if (!this.checkSubmitStep())
			return;

		let result: changeStepState = 'success';
		if (this.platform === 'lexin') {
			result = await this.changeByLeXin();
		} else if (this.platform === 'xiaomi') {
			result = await this.changeByXiaoMi();
		}

		if (result === 'success') {
			thorUiHelper.showTips(this.$refs.toast, '修改成功，请3分钟后查看数据更新结果', 2000, 'green');

			this.submitCompleted = true;
			this.submitButtonDisable = true;
		} else if (result === 'noauthor') {
			thorUiHelper.showTips(this.$refs.toast, '修改失败，登录信息已经过期');

			setTimeout(() => {
				//退出登录
				getModule(UserStoreModule, this.$store).loginOutAction();
				uni.redirectTo({ url: '/pages/index/index' });
			}, 2500);
		} else {
			if (result.errorMsg)
				thorUiHelper.showTips(this.$refs.toast, result.errorMsg!);
		}
	}

	private async changeByLeXin() {
		let dto = new ChangeStepModel();
		dto.phone = this.phone;
		dto.step = this.step;

		uniHelper.showLoading(undefined, true);
		let changeResponse = await this.$httpClient.post<MiCakeApiModel<ChangeStepResultModel>>(`/StepFly/ChangeStepByLeXin`, dto);
		await uniHelper.hideLoading(1500);

		if (changeResponse.isError) {
			return { isError: true, errorMsg: `修改失败，可以联系管理员进行反馈：${changeResponse.message}` };
		}

		var result = changeResponse.result!;
		if (result.success) {
			return 'success';
		}

		if (result.code == '401') {
			return 'noauthor';
		} else {
			return { errorMsg: `修改失败，服务器返回错误：${result.msg}`, isError: true };
		}
	}

	private async changeByXiaoMi() {
		let dto = new ChangeStepModel();
		dto.phone = this.phone;
		dto.step = this.step;

		uniHelper.showLoading(undefined, true);
		let changeResponse = await this.$httpClient.post<MiCakeApiModel<ChangeStepResultModel>>(`/StepFly/ChangeStepByXiaoMi`, dto);
		await uniHelper.hideLoading(1500);

		if (changeResponse.isError) {
			return { isError: true, errorMsg: `修改失败，可以联系管理员进行反馈：${changeResponse.message}` };
		}

		var result = changeResponse.result!;
		if (result.success) {
			return 'success';
		}

		if (result.code === 'Unauthorized') {
			return 'noauthor';
		} else {
			return { errorMsg: `修改失败，服务器返回错误：${result.msg}`, isError: true };
		}
	}

	showNoVip: boolean = false;
	private checkSubmitStep(): boolean {
		if (this.step > 20000 && !this.isVip) {
			this.noVipText = '非VIP用户步数不能超过2万';
			this.showNoVip = true;
			return false;
		}

		if (!this.isVip && this.todayRecord.hasChanged) {
			this.noVipText = '非VIP用户一天只能更改一次';
			this.showNoVip = true;
			return false;
		}

		if (this.step > 100000) {
			thorUiHelper.showTips(this.$refs.toast, '你知道微信步数的上限是多少吗？悄悄告诉你：98800....');
			return false;
		}

		if (this.todayRecord.ChangedNum && this.todayRecord.ChangedNum > this.step) {
			thorUiHelper.showTips(this.$refs.toast, `上次更改步数为：【${this.todayRecord.ChangedNum}】 ,请不要低于该步数`);
			return false;
		}

		return true;
	}

	hideNoVipModal() {
		this.showNoVip = false;
	}

	goOpenVip() {
		uni.navigateTo({ url: '/pages/vipcenter/index' });
	}

	//#region 半圆显示
	private progressIsBusy: boolean = false;
	private stepVisualization() {
		if (this.step < 10000)
			return;

		if (!this.progressIsBusy) {
			this.progressIsBusy = true;

			setTimeout(() => {
				var marathonCount = Number(this.step) / 40000;
				this.stepOfMarathonPercent = Number(marathonCount.toFixed(2)) * 50;
				this.progressIsBusy = false;
			}, 400);
		}
	}

	public randomStep() {
		var minStepNum = this.isVip ? 10000 : 5000;
		var maxStepNum = this.isVip ? 60000 : 20000;

		if (this.isVip && this.todayRecord.hasChanged) {
			minStepNum = this.todayRecord.ChangedNum!;
			maxStepNum = minStepNum === maxStepNum ? minStepNum + 5000 : maxStepNum;
		}

		this.step = NumberHelper.getRandomNumInt(minStepNum, maxStepNum);
		this.stepVisualization();
	}

	public progressChange(e: any) {
		//半圆 进度 * 2
		this.progress = e.percentage * 2;
	}

	public clearInput(type: number) {
		this.step = 0;
	}

	public inputStep(e: any) {
		if (e.detail.value !== '')
			this.step = Number(e.detail.value);

		this.stepVisualization();
	}
	//#endregion
}

type changeStepState = 'success' | 'noauthor' | { errorMsg: string, isError: boolean };

interface TodayChangeRecord {
	hasChanged: boolean;
	ChangedTime?: string;
	ChangedNum?: number;
}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";

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

	.warning-text {
		font-size: 28rpx;
		color: $uni-color-warning;
		margin-top: 44rpx;
	}

	.tui-modal-custom {
		text-align: center;
	}

	.tui-modal-custom-text {
		font-size: 30rpx;
		color: #333;
		padding: 30rpx 0 50rpx;
	}

	.tui-tips-img {
		width: 80rpx;
		height: 80rpx;
		margin-top: 20rpx;
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
