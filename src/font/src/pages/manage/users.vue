<template>
	<view>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
		<view class="tui-container">
			<view class="tui-header">
				<view class="tui-title">用户管理</view>
			</view>
			<tui-card :title="{text: '总注册人数'}">
				<template v-slot:body>
					<view class="tui-default tui-flex">
						<tui-tag type="light-green" padding="8rpx 12rpx" size="24rpx">{{userCount}}人</tui-tag>
					</view>
				</template>
			</tui-card>

			<tui-list-view style="margin-top: 10rpx" class="tui-default" title="用户列表" unlined="all">
				<tui-list-cell :arrow="true" v-for="item in source" :key="item.userKeyInfo" @click="goDetail(item)">{{ item.userKeyInfo }}</tui-list-cell>
			</tui-list-view>
			<tui-tips ref="toast"></tui-tips>
			<!--加载loading-->
			<tui-loadmore v-if="loading" :index="3" type="primary"></tui-loadmore>
		</view>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiLoadmore from "@/components/thorui/tui-loadmore/tui-loadmore.vue";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiListView from "@/components/thorui/tui-list-view/tui-list-view.vue";
import tuiSkeleton from "@/components/thorui/tui-skeleton/tui-skeleton.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiCard from "@/components/thorui/tui-card/tui-card.vue";
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyUserItemModel } from '@/models/apiModel';
import { UserStoreModule } from '@/store/user/userStore';
import { getModule } from 'vuex-module-decorators';
import PageHelper from '../helper/pageHelper';

@Component({
	components: {
		tuiTips,
		tuiLoadmore,
		tuiListCell,
		tuiListView,
		tuiTag,
		tuiCard,
		tuiSkeleton
	}
})
export default class extends Vue {
	skeletonShow: boolean = true;
	currentIndex: number = 1;
	loading: boolean = false;
	isEnd: boolean = false;

	source: StepFlyUserItemModel[] = [];
	userCount: number = 0;

	pageNum: number = 10;
	platform?: string;

	public async onLoad() {
		try {
			var storeInstance = getModule(UserStoreModule, this.$store);
			this.platform = storeInstance.user.platform;

			await this.getUserCount();
			await this.getSource();
		} finally {
			this.skeletonShow = false;
		}
	}

	// 页面上拉触底事件的处理函数
	async onReachBottom() {
		if (this.isEnd)
			return;

		this.loading = true;
		this.currentIndex++;
		await this.getSource();

		this.loading = false;
	}

	private async getSource() {
		var url = `/StepFlyUser/List?pageIndex=${this.currentIndex}&pageNum=${this.pageNum}&type=${PageHelper.getPlatformCode(this.platform)}`;
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyUserItemModel[]>>(url);

		if (apiResponse.result) {
			var records = apiResponse.result!;
			records.forEach(element => { this.source.push(element); });

			if (records.length < this.pageNum)
				this.isEnd = true;
		}
	}

	goDetail(item: StepFlyUserItemModel) {
		uni.navigateTo({ url: `/pages/manage/user-detail?id=${item.id}` });
	}

	private async getUserCount() {
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<number>>(`/StepFlyUser/Count?type=${PageHelper.getPlatformCode(this.platform)}`);
		this.userCount = apiResponse.result ?? 0;
	}
}
</script>

<style lang="scss">
@import "../../static/style/thorui.css";
page {
	background: $uni-bg-color-setoff;
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
</style>
