import { BindingTypeModel } from '@/models/apiModel';
import { version } from 'vue/types/umd';
import { Module, VuexModule, Mutation, Action, MutationAction } from "vuex-module-decorators";
import { HomeState } from './types';

@Module({
    name: 'HomeStoreModule',
    namespaced: true,
})
export class HomeStoreModule extends VuexModule {
    public home: HomeState = {
        readedNotices: '' || (uni.getStorageSync('readed-notice') as number[] ?? []),
        bindType: '' || uni.getStorageSync('bindingType'),
        version: '' || uni.getStorageSync('version')
    };

    @Mutation
    public setVersion(version: string) {
        this.home.version = version;
    }

    @Action({ commit: 'setVersion' })
    public setVersionAction(version: string) {
        uni.setStorageSync('version', version);

        return version;
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