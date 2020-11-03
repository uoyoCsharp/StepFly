<template>
	<view>
		<view class="tui-header">
			<view class="tui-title">Step-Fly</view>
			<view class="tui-sub-title">菜单导航</view>
		</view>
		<tui-grid>
			<block v-for="(item,index) in routers" :key="index">
				<tui-grid-item @tap="goMenu(item)">
					<view class="tui-grid-icon">
						<image class="tui-grid-img" :src="'/static/menus/'+item.img+'.png'" :style="{width:item.width+'rpx',height:item.height+'rpx'}" />
					</view>
					<text class="tui-grid-label">{{item.name}}</text>
				</tui-grid-item>
			</block>
			<view v-if="isAdmin">
				<block v-for="(item,index) in manageRouters" :key="index">
					<tui-grid-item @tap="goMenu(item)">
						<view class="tui-grid-icon">
							<image class="tui-grid-img" :src="'/static/menus/'+item.img+'.png'" :style="{width:item.width+'rpx',height:item.height+'rpx'}" />
						</view>
						<text class="tui-grid-label">{{item.name}}</text>
					</tui-grid-item>
				</block>
			</view>
		</tui-grid>

		<tui-list-view title="登录信息" style="margin-top:48rpx">
			<tui-list-cell :arrow="false" :hover="false">
				<view class="tui-item-box">
					<tui-icon name="wealth-fill" :size="24" color="#007aff"></tui-icon>
					<text class="tui-list-cell_name">账号：{{phone}}</text>
				</view>
			</tui-list-cell>
			<tui-list-cell :arrow="false" :hover="false" v-if="platform === 'lexin'">
				<view class="tui-item-box">
					<tui-icon name="member-fill" :size="24" color="red"></tui-icon>
					<view class="tui-list-cell_name">会员等级:</view>
					<view class="tui-list-content">
						<image v-if="vipInfo.isVip" src="/static/imgs/VIP.svg" :mode="'widthFix'" style="width:42rpx;" />
						<text :class="vipInfo.isVip?'viplevel':'no-vip'">VIP{{vipInfo.vipLevel}}</text>
					</view>
				</view>
			</tui-list-cell>
			<tui-list-cell :arrow="false" :hover="false" v-if="platform === 'lexin'">
				<view class="tui-item-box">
					<tui-icon name="explore-fill" :size="24" color="#19be6b"></tui-icon>
					<view class="tui-list-cell_name">同步平台:</view>
					<view class="tui-list-content">
						<tui-icon name="alipay" :size="24" color="#007aff" v-if="bindingType.antForestBinding"></tui-icon>
						<tui-icon name="qq" :size="24" color="#00c3ff" v-if="bindingType.qqBinding"></tui-icon>
						<tui-icon name="wechat" :size="24" color="#60ce8c" v-if="bindingType.wechatBinding"></tui-icon>
						<tui-icon name="like-fill" :size="24" color="#e85949" v-if="bindingType.lifeSenseSportFollowing"></tui-icon>
					</view>
				</view>
			</tui-list-cell>
			<tui-list-cell :arrow="true" @click="loginOut">
				<view class="tui-item-box">
					<tui-icon name="manage-fill" :size="24" color="#4cd964"></tui-icon>
					<text class="tui-list-cell_name">退出登录</text>
				</view>
			</tui-list-cell>
		</tui-list-view>

		<!--居中消息-->
		<tui-tips position="center" ref="toast"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiGrid from "@/components/thorui/tui-grid/tui-grid.vue";
import tuiGridItem from "@/components/thorui/tui-grid-item/tui-grid-item.vue";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiListView from "@/components/thorui/tui-list-view/tui-list-view.vue";
import tuiIcon from "@/components/thorui/tui-icon/tui-icon.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';
import { HomeStoreModule } from '@/store/home/homeStore';
import { BindingTypeModel, UserVipModel } from '@/models/apiModel';
import { MiCakeApiModel } from '@/common/environment';
import { VipInfoStoreModule } from '@/store/vipInfo/vipInfoStore';
import { format, addDays } from 'date-fns'

@Component({
	components: {
		tuiGrid,
		tuiGridItem,
		tuiTips,
		tuiListCell,
		tuiListView,
		tuiIcon,
		tuiTag
	}
})
export default class extends Vue {
	isAdmin: boolean = false;
	platform: string = '';
	vipInfo: VipInfo = { isVip: false, vipLevel: 0 };

	routers: MenuItem[] = [{
		name: '更改步数',
		url: '/pages/step/step',
		img: "badge",
		width: 58,
		height: 58,
		isOpen: true,
	},
	{
		name: '问题反馈',
		url: '/pages/feedback/index',
		img: "feedback",
		width: 58,
		height: 58,
		isOpen: true,
	}, {
		name: '自动更改',
		url: '',
		img: "layout",
		width: 64,
		height: 50,
		isOpen: false
	}];

	manageRouters: MenuItem[] = [{
		name: '人员管理',
		url: '/pages/manage/users',
		img: "user-manage",
		width: 58,
		height: 58,
		isOpen: true,
	},
	{
		name: '记录管理',
		url: '/pages/manage/records',
		img: "record-manage",
		width: 58,
		height: 58,
		isOpen: true,
	},
	{
		name: '反馈管理',
		url: '/pages/manage/feedback',
		img: "feedback",
		width: 58,
		height: 58,
		isOpen: true,
	}];

	public phone: string = '';
	public bindingType: BindingTypeModel = {
		wechatBinding: false,
		lifeSenseSportFollowing: false,
		qqBinding: false,
		antForestBinding: false,
	};

	public goMenu(item: MenuItem) {
		if (!item.isOpen) {
			thorUiHelper.showTips(this.$refs.toast, '该功能暂未开放，敬请期待');
			return;
		}

		uni.navigateTo({ url: item.url! });
	}

	async loginOut() {
		uniHelper.showLoading('积极退出中', true);

		var storeInstance = getModule(UserStoreModule, this.$store);
		storeInstance.loginOutAction();

		await uniHelper.hideLoading(1500);

		uni.redirectTo({ url: '/pages/index/index' });
	}

	public async onLoad() {
		if (!this.$store.state.isLogin) {
			uni.navigateTo({ url: '/pages/login/loginLeXin' });
			return;
		}

		var storeInstance = getModule(UserStoreModule, this.$store);
		this.phone = storeInstance.user.name!;
		if (storeInstance.user.roles) {
			this.isAdmin = storeInstance.user.roles!.indexOf('admin') >= 0 ? true : false;
		}
		this.platform = storeInstance.user.platform;

		await this.getVipInfo(storeInstance.user.userId!);

		if (storeInstance.user.platform === 'lexin') {
			this.getBindType();
		}
	}

	private async getBindType() {
		var homeStoreInstance = getModule(HomeStoreModule, this.$store);
		if (!homeStoreInstance.home.bindType) {
			//获取已经绑定的平台
			let loginResponse = await this.$httpClient.get<MiCakeApiModel<BindingTypeModel>>(`/StepFly/GetBindingType?userKey=${this.phone}`);

			if (loginResponse.result) {
				this.bindingType = loginResponse.result;
				homeStoreInstance.setBindingTypeAction(loginResponse.result);
			}
		} else {
			this.bindingType = homeStoreInstance.home.bindType;
		}
	}

	private async getVipInfo(userId: string) {
		var vipInfoStoreInstance = getModule(VipInfoStoreModule, this.$store);

		let needObtain = !vipInfoStoreInstance.vipInfo.obtainTime || addDays(new Date(vipInfoStoreInstance.vipInfo.obtainTime), 1) < new Date();
		if (!needObtain)
			return;

		let response = await this.$httpClient.get<MiCakeApiModel<UserVipModel>>(`/VIPUser/GetVIPInfo?userId=${userId}`);
		if (response.result) {
			let info = response.result;
			this.vipInfo.isVip = info.isVip!;
			this.vipInfo.vipLevel = info.level!;

			vipInfoStoreInstance.setVipInfoAction({ isVip: info.isVip!, level: info.level!, expireTime: new Date(info.expireTime!).getTime().toString() });
		}
	}
}

interface MenuItem {
	name: string | undefined;
	url: string | undefined;
	img: string | undefined;
	width: number;
	height: number;
	isOpen: boolean;
}

interface VipInfo {
	isVip: boolean;
	vipLevel: number;
}
</script>

<style>
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

.tui-list-cell_name {
	padding-left: 20rpx;
	display: flex;
	align-items: center;
	justify-content: center;
}

.tui-item-box {
	width: 100%;
	display: flex;
	align-items: center;
}

.tui-ml-auto {
	margin-left: auto;
}

.tui-list-content {
	margin-left: 25rpx;
	display: flex;
	justify-content: center;
	align-content: center;
	align-items: center;
}

.viplevel {
	color: #ff5722;
	font-weight: bold;
}

.no-vip {
	color: #bbbbbb;
	font-weight: bold;
}
</style>
