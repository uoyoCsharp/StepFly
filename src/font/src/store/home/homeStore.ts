import { Module, VuexModule, Mutation, Action, MutationAction } from "vuex-module-decorators";
import { HomeState } from './types';

@Module({
    name: 'HomeStoreModule',
    namespaced: true,
})
export class HomeStoreModule extends VuexModule {
    public home: HomeState = {
        readedNotices: '' || (uni.getStorageSync('readed-notice') as number[] ?? [])
    };

    @Mutation
    public markIsRead(noticeId: number) {
        this.home.readedNotices?.push(noticeId);
    }

    @Action({ commit: 'markIsRead' })
    public markIsReadAction(noticeId: number) {
        uni.setStorageSync('readed-notice', [noticeId]);
        return noticeId;
    }
}