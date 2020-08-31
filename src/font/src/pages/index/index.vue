<template>
	<view class="container">
		<image :src="bannerImg" mode="widthFix" class="tui-banner" />
		<view class="tui-text">---我知道你会来，所以我会等。</view>

		<view class="navgator-content">
			<tui-button :disabledGray="true" plain type="green" link @tap="goLogin">进入</tui-button>
			<tui-button :disabledGray="true" plain type="danger" link @tap="goDoc">查看教程</tui-button>
		</view>

		<tui-footer :fixed="true" :copyright="copyright"></tui-footer>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiFooter from "@/components/thorui/tui-footer/tui-footer.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import NumberHelper from "@/utils/numberHelper";

@Component({
	components: {
		tuiFooter,
		tuiButton
	}
})
export default class extends Vue {
	title: string = 'Hello';
	bannerImg: string = '';

	copyright: string = '由MiCake提供，运行于.NET Core平台';
	footerNavigate: any = [{ url: "https://github.com/uoyoCsharp", type: "switchTab", text: "Github", color: "#5677fc" }]

	private banners: string[] = [
		"https://up.enterdesk.com/edpic/91/80/4d/91804dd1db189643a144d450cdc3f2ce.jpg",
		"https://up.enterdesk.com/edpic/77/e8/84/77e884298c5b2c34f86cec82e5a0a875.jpeg",
		"https://up.enterdesk.com/edpic/fa/91/9d/fa919d09c60c3438427e54516a8a16be.jpg",
		"https://up.enterdesk.com/edpic/34/fa/1c/34fa1cd5bb7fb28b43dd93ebcef92948.jpg"
	];

	public goLogin() {
		uni.navigateTo({
			url: '/pages/login/loginLeXin',
		});
	}

	public goDoc() {
		uni.navigateTo({
			url: '/pages/index/doc',
		});
	}

	public onLoad(options: any) {
		this.bannerImg = this.banners[NumberHelper.getRandomNumInt(0, this.banners.length - 1)];
	}
}
</script>

<style>
.container {
	padding-bottom: 80rpx;
}

.tui-banner {
	width: 100%;
}

.navgator-content {
	margin-top: 30rpx;
}

.tui-text {
	width: 100%;
	padding: 20rpx 30rpx;
	box-sizing: border-box;
	color: #b3b3b3;
	font-size: 26rpx;
	text-align: right;
}

.tui-view {
	width: 100%;
	padding: 20rpx 30rpx;
	box-sizing: border-box;
}

.tui-cell {
	padding: 24rpx 0;
	color: #555;
}

.tui-title {
	width: 100%;
	padding: 0 8rpx;
	box-sizing: border-box;
}

.tui-link {
	width: 100%;
	padding: 30rpx;
	box-sizing: border-box;
	background: #fff;
	box-shadow: 0px 3rpx 20rpx rgba(183, 183, 183, 0.1);
	border-radius: 10rpx;
	color: #0066cc;
	margin-top: 20rpx;
	word-break: break-all;
}
</style>
