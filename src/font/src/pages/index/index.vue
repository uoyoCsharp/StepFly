<template>
	<view>
		<tui-skeleton v-if="skeletonShow" backgroundColor="#fafafa" borderRadius="10rpx"></tui-skeleton>
		<view class="tui-container">
			<view class="header-image">
				<image src="/static/logo.png" :mode="'widthFix'" style="width:300rpx" />
			</view>
			<view class="tui-banner tui-flex">
				<image :src="bannerImg" mode="widthFix" class="banner-img" style="width:100%" />
			</view>
			<view class="tui-text">{{showSentence}}</view>

			<view class="navgator-content">
				<tui-grid>
					<tui-grid-item :cell="2" @click="goLogin" :unlined="true">
						<view class="tui-grid-button">
							<image src="/static/imgs/rainbow.svg" :mode="'widthFix'" style="width:96rpx;" />
							<text class="navgator-text" style="color:#07c160">进入</text>
						</view>
					</tui-grid-item>
					<tui-grid-item :cell="2" @click="goDoc" :unlined="true">
						<view class="tui-grid-button">
							<image src="/static/imgs/book.svg" :mode="'widthFix'" style="width:52rpx;" />
							<text class="navgator-text" style="color:#fdc007">查看教程</text>
						</view>
					</tui-grid-item>
				</tui-grid>
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
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiFooter from "@/components/thorui/tui-footer/tui-footer.vue";
import tuiButton from "@/components/thorui/tui-button/tui-button.vue";
import tuiModal from "@/components/thorui/tui-modal/tui-modal.vue";
import tuiSkeleton from "@/components/thorui/tui-skeleton/tui-skeleton.vue"
import tuiGrid from "@/components/thorui/tui-grid/tui-grid.vue"
import tuiGridItem from "@/components/thorui/tui-grid-item/tui-grid-item.vue"
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
		tuiModal,
		tuiSkeleton,
		tuiGrid,
		tuiGridItem
	}
})
export default class extends Vue {
	skeletonShow: boolean = true;
	title: string = 'Hello';
	bannerImg: string = '';
	showNotice: boolean = false;
	showSentence: string = '';
	noticeHtml: string = ``;
	noShowCurrentNotice: boolean = false;

	copyright: string = '由MiCake提供，运行于.NET Core平台';
	private defaultBanner: string = "/static/default-banner.jpg";
	private noticeId: number = -1;

	public goLogin() {
		if (this.$store.state.isLogin) {
			uni.navigateTo({ url: '/pages/menu/index' });
		} else {
			uni.navigateTo({ url: '/pages/login/login' });
		}
	}

	public goDoc() {
		uni.navigateTo({ url: '/pages/index/doc' });
	}

	async onLoad() {
		try {
			await this.getHomeConfig();
			await this.getNotice();
		} catch {
			this.bannerImg = this.defaultBanner;
		} finally {
			this.skeletonShow = false;
		}
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
		if (!uniHelper.validator.isNullOrEmpty(result?.footerInfo))
			this.copyright = result.footerInfo!;

		if (!uniHelper.validator.isNullOrEmpty(result?.showSentence))
			this.showSentence = result.showSentence!;

		if (result?.candidateHomePics) {
			var pics = JSON.parse(result.candidateHomePics) as string[];
			this.bannerImg = pics[NumberHelper.getRandomNumInt(0, pics.length - 1)];
		} else {
			this.bannerImg = this.defaultBanner;
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

<style lang="scss">
@import "../../static/style/thorui.css";

.tui-banner {
	width: 100%;
	border-radius: 12px;
}

.navgator-content {
	margin-top: 48rpx;
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

.header-image {
	display: flex;
	justify-content: center;
	margin: 25px;
}

.banner-img {
	margin: 20rpx;
	border-radius: 20rpx;
}

.tui-grid-button {
	height: 120rpx;
	margin: 0 auto;
	text-align: center;
	vertical-align: middle;
	display: flex;
	justify-content: center;
	align-items: center;
}

.navgator-text {
	margin-left: 12rpx;
}
</style>
