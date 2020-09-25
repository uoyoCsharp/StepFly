<template>
	<view class="container">
		<view class="tui-header">
			<view class="tui-title">Step-Fly</view>
			<view class="tui-sub-title">菜单导航</view>
		</view>
		<tui-grid>
			<block v-for="(item,index) in routers" :key="index">
				<tui-grid-item>
					<view class="tui-grid-icon" @tap="goMenu(item)">
						<image class="tui-grid-img" :src="'/static/menus/'+item.img+'.png'" :style="{width:item.width+'rpx',height:item.height+'rpx'}" />
					</view>
					<text class="tui-grid-label">{{item.name}}</text>
				</tui-grid-item>
			</block>
			<block v-for="(item,index) in manageRouters" :key="index" v-if="isAdmin">
				<tui-grid-item>
					<view class="tui-grid-icon" @tap="goMenu(item)">
						<image class="tui-grid-img" :src="'/static/menus/'+item.img+'.png'" :style="{width:item.width+'rpx',height:item.height+'rpx'}" />
					</view>
					<text class="tui-grid-label">{{item.name}}</text>
				</tui-grid-item>
			</block>
		</tui-grid>

		<tui-list-view title="登录信息" style="margin-top:48rpx">
			<tui-list-cell :arrow="false" :hover="false">
				<view class="tui-item-box">
					<tui-icon name="wealth-fill" :size="24" color="#007aff"></tui-icon>
					<text class="tui-list-cell_name">账号：{{phone}}</text>
				</view>
			</tui-list-cell>
			<tui-list-cell :arrow="false" :hover="false">
				<view class="tui-item-box">
					<tui-icon name="explore-fill" :size="24" color="#19be6b"></tui-icon>
					<view class="tui-list-cell_name">同步平台:</view>
					<view class="tui-list-content">
						<tui-icon name="alipay" :size="24" color="#007aff" v-if="bindingType.antForestBinding"></tui-icon>
						<tui-icon name="qq" :size="24" color="#00c3ff" v-if="bindingType.qqBinding"></tui-icon>
						<tui-icon name="wechat" :size="24" color="#60ce8c" v-if="bindingType.wechatBinding"></tui-icon>
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
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';
import { HomeStoreModule } from '@/store/home/homeStore';
import { BindingTypeModel } from '@/models/apiModel';
import { MiCakeApiModel } from '@/common/environment';

@Component({
	components: {
		tuiGrid,
		tuiGridItem,
		tuiTips,
		tuiListCell,
		tuiListView,
		tuiIcon,
	}
})
export default class extends Vue {
	isAdmin: boolean = false;

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
		wechatBinding: true,
		lifeSenseSportFollowing: false,
		qqBinding: true,
		antForestBinding: true,
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

		var homeStoreInstance = getModule(HomeStoreModule, this.$store);
		if (!homeStoreInstance.home.bindType) {
			//获取已经绑定的平台
			let loginResponse = await this.$httpClient.get<MiCakeApiModel<BindingTypeModel>>(`/StepFly//StepFly/GetBindingType`, this.phone);

			if (loginResponse.result) {
				this.bindingType = loginResponse.result;
			}
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
</script>

<style>
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
	color: #7a7a7a;
	padding-top: 18rpx;
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
}
</style>
