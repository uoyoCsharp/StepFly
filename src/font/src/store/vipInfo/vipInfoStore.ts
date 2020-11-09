import { Module, VuexModule, Mutation, Action, MutationAction } from "vuex-module-decorators";
import { VipInfoState } from './types';

@Module({
    name: 'VipInfoStoreModule',
    namespaced: true,
})
export class VipInfoStoreModule extends VuexModule {
    public vipInfo: VipInfoState = {
        isVip: '' || uni.getStorageSync('isVip'),
        level: '' || uni.getStorageSync('level'),
        expireTime: '' || uni.getStorageSync('expireTime'),
        obtainTime: '' || uni.getStorageSync('obtainTime'),
    };

    @Mutation
    public clearVipInfo() {
        debugger;
        this.vipInfo = { isVip: false, level: undefined, expireTime: undefined, obtainTime: undefined };
    }

    @Action({ commit: 'clearVipInfo' })
    public clearVipInfoAction() {
    }

    @Mutation
    public setVipInfo(info: { isVip: boolean, level: number, expireTime: string, obtainTime: string }) {
        this.vipInfo = info;
    }

    @Action({ commit: 'setVipInfo' })
    public setVipInfoAction(info: { isVip: boolean, level: number, expireTime: string }) {
        const currentTime = new Date().getTime().toString();

        uni.setStorageSync('isVip', info.isVip);
        uni.setStorageSync('level', info.level);
        uni.setStorageSync('expireTime', info.expireTime);
        uni.setStorageSync('obtainTime', currentTime);

        return { obtainTime: currentTime, ...info };
    }
}