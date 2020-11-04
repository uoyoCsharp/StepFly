<template>
	<view>
		<view class="tui-header">
			<view class="tui-title">自动更改</view>
			<view class="tui-sub-title">每天定时自动修改</view>
		</view>
		<tui-no-data v-if="!isLevelTwo" :fixed="true" imgUrl="/static/logo.svg" btnText="去获取VIP" @click="goVip">很抱歉，该功能仅对VIP2以上用户开放~</tui-no-data>
		<tui-no-data v-else :fixed="true" imgUrl="/static/logo.svg">功能即将开放，敬请期待~</tui-no-data>
		<tui-tips ref="toast"></tui-tips>
	</view>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Emit, Ref } from "vue-property-decorator";
import tuiTips from "@/components/thorui/tui-tips/tui-tips.vue";
import tuiTag from "@/components/thorui/tui-tag/tui-tag.vue";
import tuiNoData from "@/components/thorui/tui-no-data/tui-no-data.vue";
import PageHelper from "../helper/pageHelper";
import { MiCakeApiModel } from '@/common/environment';
import { StepFlyHistoryModel } from '@/models/apiModel';
import { getModule } from 'vuex-module-decorators';
import { UserStoreModule } from '@/store/user/userStore';
import { VipInfoStoreModule } from '@/store/vipInfo/vipInfoStore';

@Component({
	components: {
		tuiTips,
        tuiTag,
        tuiNoData
	}
})
export default class extends Vue {
	isLevelTwo: boolean = false;

	public async onLoad() {
		var storeInstance = getModule(VipInfoStoreModule, this.$store);
		if (storeInstance.vipInfo.isVip) {
			this.isLevelTwo = storeInstance.vipInfo.level! >= 2;
		}
	}

	goVip() {

	}
}
</script>

<style lang="scss">
</style>
