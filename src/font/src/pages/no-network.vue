<template>
	<view class="container">
		<tui-no-data :fixed="true" imgUrl="/static/logo.svg" btnText="返回主页"  @click="goHome">网络访问出错，请重新操作</tui-no-data>
	</view>
</template>

<script lang="ts">
import tuiNoData from "@/components/thorui/tui-no-data/tui-no-data.vue";
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";

@Component({
    components:{
        tuiNoData
    }
})
export default class extends Vue {
    onLoad(){
		uni.hideLoading();
	}

	goHome(){
		uni.reLaunch({url:'/pages/index/index'});
	}
}
</script>

<style>
	.container {
		padding: 20rpx 0 120rpx 0;
		box-sizing: border-box;
	}

	.header {
		padding: 80rpx 90rpx 60rpx 90rpx;
		box-sizing: border-box;
	}

	.title {
		font-size: 34rpx;
		color: #333;
		font-weight: 500;
	}

	.sub-title {
		font-size: 24rpx;
		color: #7a7a7a;
		padding-top: 18rpx;
	}

	.tui-btn-box {
		padding: 10rpx 40rpx;
		box-sizing: border-box;
	}
</style>
