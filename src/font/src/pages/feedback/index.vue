<template>
	<view class="container">
		<view class="title">意见/问题反馈</view>
		<view class="tui-form">
			<input class="tui-input" placeholder-class="phcolor" placeholder="请输入标题" v-model="title" maxlength="50" />
			<view class="tui-cells">
				<textarea
					class="tui-textarea"
					placeholder="如果您在使用中遇到什么问题，可以在此处填写。如果有需要，可以在内容中包含您的联系方式，管理员会与您联系。同时，您有什么建议也可以在这儿填写。"
					maxlength="500"
					placeholder-class="phcolor"
					v-model="content"
				/>
			</view>
			<view class="tui-btn-box">
				<tui-button shape="circle" :shadow="true" @click="submit">提交反馈</tui-button>
			</view>
		</view>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import TuiButton from "@/components/thorui/tui-button/tui-button.vue";

@Component({
	components: {
		TuiButton
	}
})
export default class extends Vue {
	title: string = '';
	content: string = '';

	submit() {

	}
}
</script>

<style lang="scss">
page {
	background: #fff;
}

.container {
	background-color: #fff;
	margin-top: 22rpx;
	padding: 30rpx;
	box-sizing: border-box;
	padding-bottom: 162rpx;
}

.title {
	font-size: 30rpx;
	color: #666;
	padding-bottom: 32rpx;
}

.tui-form {
	padding-top: 50rpx;

	.tui-btn-box {
		width: 100%;
		padding: 0 $uni-spacing-row-lg;
		box-sizing: border-box;
		padding-top: 80rpx;
	}
}

.tui-cells {
	border: 1rpx solid #e6e6e6;
	border-radius: 4rpx;
	height: 480rpx;
	box-sizing: border-box;
	padding: 20rpx 20rpx 0 20rpx;
	margin-top: 50rpx;
}

.tui-textarea {
	height: 210rpx;
	width: 100%;
	color: #666;
	font-size: 28rpx;
}

.pholder {
	color: #ccc;
}

.textarea-counter {
	font-size: 24rpx;
	color: #999;
	text-align: right;
	height: 40rpx;
	line-height: 40rpx;
	padding-top: 4rpx;
}

.top64 {
	margin-top: 64rpx;
}

.tui-input {
	font-size: 30rpx;
	height: 88rpx;
	border: 1rpx solid #e6e6e6;
	border-radius: 4rpx;
	padding: 0 25rpx;
	box-sizing: border-box;
}
</style>
