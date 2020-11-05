<template>
	<view>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
		<view class="tui-header">
			<view class="tui-title">记录管理</view>
		</view>
		<tui-list-view class="tui-default">
			<tui-list-cell :arrow="false" :hover="false" v-for="item in source" :key="item.id">
				<text class="list-header">{{ item.userKeyInfo }}</text>
				<view class="tui-item-box">
					<tui-tag size="26rpx" :type="PageHelper.getStepTagColor(item.stepNum)" :shape="'circle'">{{item.stepNum}} 步</tui-tag>
					<view style="margin-left: auto;">
						<text class="tui-sub-title">{{item.creationTime |dateformat('MM月dd日 HH:mm')}}</text>
					</view>
				</view>
			</tui-list-cell>
		</tui-list-view>
		<tui-tips ref="toast"></tui-tips>
		<!--加载loading-->
		<tui-loadmore v-if="loading" :index="3" type="primary"></tui-loadmore>
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
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyHistoryModel } from '@/models/apiModel';
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
		tuiSkeleton
	}
})
export default class extends Vue {
	PageHelper = PageHelper;

	currentIndex: number = 1;
	loading: boolean = false;
	skeletonShow: boolean = true;
	isEnd: boolean = false;

	source: StepFlyHistoryModel[] = [];

	pageNum: number = 10;
	platform?: string;

	public async onLoad() {
		try {
			var storeInstance = getModule(UserStoreModule, this.$store);
			this.platform = storeInstance.user.platform;

			await this.getSource();
		} finally {
			this.skeletonShow = false
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
		const url = `/StepFlyHistory/Get?pageIndex=${this.currentIndex}&pageNum=${this.pageNum}&type=${PageHelper.getPlatformCode(this.platform)}`;
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<StepFlyHistoryModel[]>>(url);

		if (apiResponse.result) {
			var records = apiResponse.result!;
			records.forEach(element => { this.source.push(element); });

			if (records.length < this.pageNum)
				this.isEnd = true;
		}
	}
}
</script>

<style lang="scss">
page {
	background: $uni-bg-color-setoff;
}

.tui-default {
	padding: 20rpx 30rpx;
}
.tui-item-box {
	width: 100%;
	display: flex;
	align-items: center;
	margin-top: 12rpx;
}

.list-header {
	font-size: 24rpx;
	color: #333;
	font-weight: bold;
}
</style>
