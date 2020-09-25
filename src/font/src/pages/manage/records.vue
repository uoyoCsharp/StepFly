<template>
	<view class="tui-container">
		<tui-list-view style="margin-top:48rpx">
			<tui-list-cell :arrow="false" :hover="false" v-for="item in source" :key="item">
				<view class="tui-item-box">
					<text class="list-header">{{item}}</text>
				</view>
				<view class="tui-item-box" style="margin-top:30rpx">
					<text>更改步数:</text>
					<tui-tag padding="8rpx" size="24rpx" type="light-orange">{{item}}</tui-tag>
					<text style="margin-left:55rpx">更改时间:</text>
					<tui-tag padding="8rpx" size="24rpx" type="light-green">2020/2/3 15:00:00</tui-tag>
				</view>
			</tui-list-cell>
		</tui-list-view>
		<tui-tips ref="toast"></tui-tips>
		<!--加载loadding-->
		<tui-loadmore v-if="loadding" :index="3" type="primary"></tui-loadmore>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiLoadmore from "@/components/thorui/tui-loadmore/tui-loadmore.vue";
import tuiListCell from "@/components/thorui/tui-list-cell/tui-list-cell.vue";
import tuiListView from "@/components/thorui/tui-list-view/tui-list-view.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";

@Component({
	components: {
		tuiTips,
		tuiLoadmore,
		tuiListCell,
		tuiListView,
		tuiTag
	}
})
export default class extends Vue {
	currentIndex: number = 1;
	loadding: boolean = false;

	source: any = []

	onLoad() {
		for (let index = 0; index < 60; index++) {
			this.source.push(index);
		}
	}

	// 页面上拉触底事件的处理函数
	onReachBottom() {
		this.loadding = true;
		setTimeout(() => {
			this.loadding = false;
			this.currentIndex++;
		}, 2000);
	}
}
</script>

<style lang="scss">
.tui-container {
	display: flex;
	flex-direction: column;
	box-sizing: border-box;
	padding-bottom: env(safe-area-inset-bottom);
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
</style>
