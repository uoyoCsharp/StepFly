<template>
	<view class="container">
		<image :src="bannerImg" mode="widthFix" class="tui-banner" />
		<view class="tui-text">---我知道你会来，所以我会等。</view>

		<view class="navgator-content">
			<tui-button :disabledGray="true" plain type="green" link @tap="goLogin">进入</tui-button>
			<tui-button :disabledGray="true" plain type="danger" link @tap="goDoc">查看教程</tui-button>
		</view>

		<tui-modal :show="showNotice" :maskClosable="false" :custom="true">
			<view class="tui-modal-custom">
				<scroll-view scroll-y="true" class="scroll-Y">
					<view v-html="noticeHtml" class="notice-content"></view>
				</scroll-view>
				<tui-button height="72rpx" :size="26" shape="circle" @click="closeNotice">确定</tui-button>
				<checkbox-group class="uni-list" @change="checkboxChange">
					<label class="uni-list-cell uni-list-cell-pd">
						<checkbox :checked="noShowCurrentNotice" style="transform:scale(0.8)"></checkbox>
						<text class="sub-title">不再提示</text>
					</label>
				</checkbox-group>
			</view>
		</tui-modal>
		<tui-footer :fixed="true" :copyright="copyright"></tui-footer>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiFooter from "@/components/thorui/tui-footer/tui-footer.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiModal from "@/components/thorui/tui-modal/tui-modal.vue";
import NumberHelper from "@/utils/numberHelper";
import { HomeModel, NoticeModel } from '@/models/apiModel';
import uniHelper, { thorUiHelper } from '@/common/uniHelper';
import { getModule } from 'vuex-module-decorators';
import { HomeStoreModule } from "@/store/home/homeStore";
import { MiCakeApiModel } from '@/common/environment';

@Component({
	components: {
		tuiFooter,
		tuiButton,
		tuiModal
	}
})
export default class extends Vue {
	title: string = 'Hello';
	bannerImg: string = '';
	showNotice: boolean = false;
	noticeHtml: string = ``;
	noShowCurrentNotice: boolean = false;

	copyright: string = '由MiCake提供，运行于.NET Core平台';
	private defaultBanner: string = "https://up.enterdesk.com/edpic/fa/91/9d/fa919d09c60c3438427e54516a8a16be.jpg";
	private noticeId: number = -1;

	public goLogin() {
		if (this.$store.state.isLogin) {
			uni.navigateTo({ url: '/pages/menu/index' });
		} else {
			uni.navigateTo({ url: '/pages/login/loginLeXin' });
		}
	}

	public goDoc() {
		uni.navigateTo({ url: '/pages/index/doc' });
	}

	async onLoad() {
		this.bannerImg = this.defaultBanner;

		await this.getHomeConfig();
		await this.getNotice();
	}

	private async getNotice() {
		var apiRes = await this.$httpClient.get<MiCakeApiModel<NoticeModel[]>>('/Notice/GetNotices');

		if (apiRes.isError)
			return;

		if (apiRes.result!.length > 0) {
			//目前只处理单个公告
			var notice = apiRes.result![0];
			var homeStore = getModule(HomeStoreModule, this.$store);
			if (homeStore.home.readedNotices!.indexOf(notice.id!) < 0) {
				this.showNotice = true;
				this.noticeHtml = notice.content!;
				this.noticeId = notice.id!;
			}
		}
	}

	private async getHomeConfig() {
		var apiRes = await this.$httpClient.get<MiCakeApiModel<HomeModel>>('/Home/GetHomeConfig');
		if (apiRes.isError)
			return;

		var result = apiRes.result!;
		if (!uniHelper.validator.isNullOrEmpty(result.footerInfo))
			this.copyright = result.footerInfo!;

		if (result.candidateHomePics != null) {
			var pics = JSON.parse(result.candidateHomePics) as string[];
			this.bannerImg = pics[NumberHelper.getRandomNumInt(0, pics.length - 1)];
		}
	}

	public closeNotice() {
		if (this.noShowCurrentNotice) {
			var homeStore = getModule(HomeStoreModule, this.$store);
			homeStore.markIsReadAction(this.noticeId);
		}

		this.showNotice = false;
	}

	public checkboxChange(e: any) {
		this.noShowCurrentNotice = e.detail.value.length > 0;
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

.tui-modal-custom {
	text-align: center;
}

.uni-list-cell {
	justify-content: flex-start;
}
.uni-list {
	padding: 0;
	float: left;
}

.label-2-text {
	flex: 1;
}

.sub-title {
	font-size: 28rpx;
	color: #7a7a7a;
	padding-top: 18rpx;
}

.notice-content {
	margin-bottom: 28rpx;
}

.scroll-Y {
	max-height: 300px;
	text-align: left;
}
</style>
