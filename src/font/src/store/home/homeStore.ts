import { BindingTypeModel } from '@/models/apiModel';
import { Module, VuexModule, Mutation, Action, MutationAction } from "vuex-module-decorators";
import { HomeState } from './types';

@Module({
    name: 'HomeStoreModule',
    namespaced: true,
})
export class HomeStoreModule extends VuexModule {
    public home: HomeState = {
        readedNotices: '' || (uni.getStorageSync('readed-notice') as number[] ?? []),
        bindType: '' || uni.getStorageSync('bindingType')
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

    @Mutation
    public setBindingType(types: BindingTypeModel) {
        this.home.bindType = types;
    }

    @Action({ commit: 'setBindingType' })
    public setBindingTypeAction(types: BindingTypeModel) {
        uni.setStorageSync('bindingType', types);
        return types;
    }
}