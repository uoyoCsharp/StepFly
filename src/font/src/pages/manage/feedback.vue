<template>
	<view class="tui-container">
		<view class="tui-header">
			<view class="tui-title">反馈管理</view>
			<view class="tui-sub-title">查看用户反馈的信息</view>
		</view>
		<tui-no-data v-if="source.length==0" :fixed="false" imgUrl="/static/toast/img_nodata.png">暂无数据</tui-no-data>

		<tui-card v-for="item in source" :key="item.id" :title="{text: item.title}" style="margin-top:30rpx">
			<template v-slot:body>
				<view class="tui-default">{{item.content}}</view>
			</template>
			<template v-slot:footer>
				<view class="tui-default tui-flex">
					<tui-tag type="light-green" padding="8rpx 12rpx" size="24rpx">{{item.tag}}</tui-tag>
          <tui-tag type="light-orange" padding="8rpx 12rpx" size="24rpx" style="margin-left:10rpx">{{item.userKey}}</tui-tag>
				</view>
			</template>
		</tui-card>
		<tui-tips ref="toast"></tui-tips>
		<!--加载loading-->
		<tui-loadmore v-if="loading" :index="3" type="primary"></tui-loadmore>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiLoadmore from "@/components/thorui/tui-loadmore/tui-loadmore.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiCard from "@/components/thorui/tui-card/tui-card.vue";
import tuiNoData from "@/components/thorui/tui-no-data/tui-no-data.vue";
import tuiSkeleton from "@/components/thorui/tui-skeleton/tui-skeleton.vue";
import { MiCakeApiModel } from '@/common/environment';
import { FeedbackItemModel } from '@/models/apiModel';

@Component({
	components: {
		tuiTips,
		tuiLoadmore,
		tuiTag,
		tuiCard,
    tuiNoData,
    tuiSkeleton
	}
})
export default class extends Vue {
	currentIndex: number = 1;
	loading: boolean = false;
	isEnd: boolean = false;
	skeletonShow: boolean = true;
	source: FeedbackItemModel[] = [];

	pageNum: number = 5;

	public async onLoad() {
		try {
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
		//获取已经绑定的平台
		let apiResponse = await this.$httpClient.get<MiCakeApiModel<FeedbackItemModel[]>>(`/Feedback/Get?pageIndex=${this.currentIndex}&pageNum=${this.pageNum}`);

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
