<template>
	<view class="tui-container">
		<uni-segmented-control :current="current" :values="items" @clickItem="onClickItem" style-type="text" class="segment"></uni-segmented-control>
		<view class="content">
			<view v-if="current === 0">
				<login-xiaomi></login-xiaomi>
			</view>
			<view v-if="current === 1">
				<login-lexin></login-lexin>
			</view>
		</view>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import uniSegmentedControl from "@/components/uni-segmented-control/uni-segmented-control.vue"
import LoginLexin from "./loginLeXin.vue";
import LoginXiaomi from "./loginXiaoMi.vue";

@Component({
	components: {
		uniSegmentedControl,
		LoginLexin,
		LoginXiaomi
	},
})
export default class extends Vue {
	items: string[] = ['小米平台（默认）', '乐心平台'];
	current: number = 0;


	public onClickItem(e: any) {
		if (this.current !== e.currentIndex) {
			this.current = e.currentIndex;
		}
	}
}
</script>

<style lang="scss">
.segment {
	margin: 24rpx;
}
</style>
