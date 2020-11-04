<template>
	<view>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
		<view class="tui-container">
			<view class="tui-header">
				<view class="tui-title">用户详情</view>
				<view class="tui-sub-title">{{userInfo.userKeyInfo}}</view>
			</view>

			<tui-card v-if="userInfo.userKeyInfo" :title="{text:'状态信息'}">
				<template v-slot:body>
					<view class="tui-default">
						<view class="tui-sub-title" v-if="userInfo.modificationTime">上次登录:{{userInfo.modificationTime |dateformat('yyyy年MM月dd日 HH:mm')}}</view>
						<view class="tui-sub-title">注册时间:{{userInfo.loginTime |dateformat('yyyy年MM月dd日 HH:mm')}}</view>
					</view>
				</template>
				<template v-slot:footer>
					<view class="tui-default tui-flex">
						<tui-tag type="light-green" padding="8rpx 12rpx" size="24rpx">{{userInfo.isLockout? '已锁定':'正常'}}</tui-tag>
					</view>
				</template>
			</tui-card>

			<tui-card v-if="vipInfo" :title="{text:'会员信息'}" style="margin-top:24rpx">
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
					</view>
				</template>
			</tui-card>

			<view class="tui-default" style="margin-top:36rpx">
				<tui-grid>
					<tui-grid-item @tap="upToAdmin()">
						<view class="tui-grid-icon">
							<image class="tui-grid-img" :src="'/static/menus/up.png'" style="width:64rpx;height: 64rpx;" />
						</view>
						<text class="tui-grid-label">提升为管理员</text>
					</tui-grid-item>
					<tui-grid-item @tap="upToVip()">
						<view class="tui-grid-icon">
							<image class="tui-grid-img" :src="'/static/menus/vipcenter.png'" style="width:64rpx;height: 64rpx;" />
						</view>
						<text class="tui-grid-label">提升为VIP用户</text>
					</tui-grid-item>
					<tui-grid-item @tap="lockUser()">
						<view class="tui-grid-icon">
							<image class="tui-grid-img" :src="'/static/menus/lock.png'" style="width:64rpx;height: 64rpx;" />
						</view>
						<text class="tui-grid-label">锁定该用户</text>
					</tui-grid-item>
				</tui-grid>
			</view>
		</view>
		<tui-tips ref="toast" position="center"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiLoadmore from "@/components/thorui/tui-loadmore/tui-loadmore.vue";
import tuiGrid from "@/components/thorui/tui-grid/tui-grid.vue";
import tuiGridItem from "@/components/thorui/tui-grid-item/tui-grid-item.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiCard from "@/components/thorui/tui-card/tui-card.vue";
import tuiSkeleton from "@/components/thorui/tui-skeleton/tui-skeleton.vue";
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyUserItemModel, UserVipModel } from '@/models/apiModel';
import { UserStoreModule } from '@/store/user/userStore';
import { getModule } from 'vuex-module-decorators';
import PageHelper from '../helper/pageHelper';
import uniHelper, { thorUiHelper } from '@/common/uniHelper';

@Component({
	components: {
		tuiTips,
		tuiLoadmore,
		tuiTag,
		tuiCard,
		tuiSkeleton,
		tuiGrid,
		tuiGridItem
	}
})
export default class extends Vue {
	platform?: string;
	skeletonShow: boolean = false;

	userId?: string;
	userInfo: StepFlyUserItemModel = {};
	vipInfo: UserVipModel = {};

	public async onLoad(option: any) {
		try {
			var storeInstance = getModule(UserStoreModule, this.$store);
			this.platform = storeInstance.user.platform;

			this.userId = option.id;

			await this.getUserInfo();
			await this.getUserVipInfo();
		} finally {
			this.skeletonShow = false;
		}
	}

	private async getUserInfo() {
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyUserItemModel>>(`/StepFlyUser/Detail?id=${this.userId}`);
		this.userInfo = apiResponse.result!;
	}

	private async getUserVipInfo() {
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<UserVipModel>>(`/VIPUser/GetVIPInfo?userId=${this.userId}`);
		this.vipInfo = apiResponse.result!;
	}

	public async upToAdmin() {
		uniHelper.showLoading(undefined, true);
		let apiResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/StepFlyUser/PromotedToAdmin`, { userId: this.userId });
		await uniHelper.hideLoading(1500);

		if (!apiResponse.result) {
			thorUiHelper.showTips(this.$refs.toast, '提升为管理员失败');
		} else {
			thorUiHelper.showTips(this.$refs.toast, '提升成功，请通知用户重新登录', 2000, 'green');
		}
	}

	public async upToVip() {
		uniHelper.showLoading(undefined, true);
		let apiResponse = await this.$httpClient.post<MiCakeApiModel<boolean>>(`/VIPUserManage/ActiveVIP`, { userId: this.userId, effectDay: 60 });
		await uniHelper.hideLoading(1500);

		if (!apiResponse.result) {
			thorUiHelper.showTips(this.$refs.toast, '激活VIP失败，请核实该用户是否已经为VIP');
		} else {
			thorUiHelper.showTips(this.$refs.toast, '激活VIP成功，VIP有效期为【60天】', 2000, 'green');
			this.getUserVipInfo();
		}
	}

	public async lockUser() {
		uniHelper.showLoading(undefined, true);
		let apiResponse = await this.$httpClient.put<MiCakeApiModel<boolean>>(`/StepFlyUser/Lock?userId=${this.userId}`, '');
		await uniHelper.hideLoading(1500);

		if (!apiResponse.result) {
			thorUiHelper.showTips(this.$refs.toast, '锁定失败');
		} else {
			thorUiHelper.showTips(this.$refs.toast, '锁定成功', 2000, 'green');
			this.getUserInfo();
		}
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
</style>
