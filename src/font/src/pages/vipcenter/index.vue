<template>
	<view>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
		<view class="tui-container">
			<view class="tui-header">
				<view class="tui-title">会员中心</view>
			</view>

			<tui-card v-if="vipInfo" :title="{text:'我的会员信息'}" style="margin-top:24rpx">
				<template v-slot:body>
					<view class="tui-default" v-if="vipInfo.isVip">
						<view class="content-title">
							会员等级:
							<view class="tui-list-content">
								<image src="/static/imgs/VIP.svg" :mode="'widthFix'" style="width:42rpx;" />
								<text class="viplevel">VIP{{vipInfo.level}}</text>
							</view>
						</view>
						<view class="content-title">过期时间: {{vipInfo.expireTime |dateformat('yyyy年MM月dd日 HH:mm')}}</view>
					</view>
					<view class="tui-default" v-else>
						<view class="tui-sub-title">非VIP用户</view>
						<br />
					</view>
				</template>
			</tui-card>

			<view class="tui-header">
				<view class="tui-sub-title">活动赢一年VIP</view>
			</view>
			<tui-no-data :fixed="false" imgUrl="/static/imgs/noorder.png">即将上线</tui-no-data>

			<view class="tui-header">
				<view class="tui-sub-title">打赏作者得VIP</view>
			</view>
			<view class="reward-container">
				<tui-button height="72rpx" :size="28" plain type="warning" shape="circle" @click="goReward">去打赏</tui-button>
			</view>
		</view>
		<tui-tips ref="toast" position="center"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiCard from "@/components/thorui/tui-card/tui-card.vue";
import tuiNoData from "@/components/thorui/tui-no-data/tui-no-data.vue";
import tuiSkeleton from "@/components/thorui/tui-skeleton/tui-skeleton.vue";
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyUserItemModel, UserVipModel } from '@/models/apiModel';
import { UserStoreModule } from '@/store/user/userStore';
import { getModule } from 'vuex-module-decorators';
import PageHelper from '../helper/pageHelper';
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import { VipInfoStoreModule } from '@/store/vipInfo/vipInfoStore';

@Component({
	components: {
		tuiTips,
		tuiTag,
		tuiCard,
		tuiButton,
		tuiSkeleton,
		tuiNoData
	}
})
export default class extends Vue {
	platform?: string;
	skeletonShow: boolean = true;

	userId?: string;
	vipInfo: UserVipModel = {};

	public async onLoad() {
		try {
			var storeInstance = getModule(UserStoreModule, this.$store);
			this.platform = storeInstance.user.platform;
			this.userId = storeInstance.user.userId;

			await this.getUserVipInfo();
		} finally {
			this.skeletonShow = false;
		}
	}

	private async getUserVipInfo() {
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<UserVipModel>>(`/VIPUser/GetVIPInfo?userId=${this.userId}`);
		this.vipInfo = apiResponse.result!;

		var vipInstance = getModule(VipInfoStoreModule, this.$store);
		if (vipInstance.vipInfo.isVip !== this.vipInfo.isVip) {
			console.log('change vip info');
			vipInstance.setVipInfoAction({ isVip: this.vipInfo.isVip!, level: this.vipInfo.level!, expireTime: this.vipInfo.expireTime! });
		}
	}

	goReward() {
		uni.navigateTo({ url: '/pages/reward/index' });
	}
}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";
page {
	background: $uni-bg-color-setoff;
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

.viplevel {
	color: #ff5722;
	font-weight: bold;
}

.tui-list-content {
	margin-left: 25rpx;
	display: flex;
	justify-content: center;
	align-content: center;
	align-items: center;
}

.content-title {
	font-size: 15px;
	color: #7a7a7a;
	padding-top: 9px;
	display: flex;
	align-content: center;
	align-items: center;
}

.reward-container {
	width: 50%;
	padding: 0rpx 66rpx;
}
</style>
