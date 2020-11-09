import { Module, VuexModule, Mutation, Action, getModule } from "vuex-module-decorators";
import { VipInfoStoreModule } from '../vipInfo/vipInfoStore';
import { UserState } from './types';

@Module({
    name: 'UserStoreModule',
    namespaced: true,
})
export class UserStoreModule extends VuexModule {
    public user: UserState = {
        userId: '' || uni.getStorageSync('userId'),
        name: '' || uni.getStorageSync('login-userName'),
        accessToken: '' || uni.getStorageSync('token'),
        lastLoginTime: '' || uni.getStorageSync('login-time'),
        isLockout: '' || uni.getStorageSync('user-islockout'),
        roles: '' || uni.getStorageSync('user-roles'),
        platform: '' || uni.getStorageSync('platform')
    };

    @Mutation
    public loginSuccess(data: { userId: string, name: string, token: string, isLockout: boolean, roles: string, platform: 'lexin' | 'xiaomi' }) {
        this.user.userId = data.userId;
        this.user.name = data.name;
        this.user.accessToken = data.token;
        this.user.isLockout = data.isLockout;
        this.user.roles = data.roles;
        this.user.lastLoginTime = new Date().getTime();
        this.user.platform = data.platform;
    }

    @Mutation
    public loginOut() {
        this.user.name = undefined;
        this.user.accessToken = undefined;
        this.user.isLockout = undefined;
        this.user.roles = undefined;
        this.user.lastLoginTime = 0;
    }

    @Action({ commit: 'loginSuccess' })
    public loginSuccessAction(data: { userId: string, name: string, token: string, isLockout: boolean, roles: string, platform: 'lexin' | 'xiaomi' }) {
        uni.setStorageSync('userId', data.userId);
        uni.setStorageSync('login', true);
        uni.setStorageSync('login-userName', data.name);
        uni.setStorageSync('user-islockout', data.isLockout);
        uni.setStorageSync('user-roles', data.roles);
        uni.setStorageSync('login-time', new Date().getTime().toString());
        uni.setStorageSync('token', data.token);
        uni.setStorageSync('platform', data.platform);

        this.context.rootState.isLogin = true;
        return data;
    }

    @Action({ commit: 'loginOut' })
    public loginOutAction() {
        uni.clearStorageSync();
        this.context.dispatch('VipInfoStoreModule/clearVipInfoAction',{},{root:true});

        this.context.rootState.isLogin = false;
        return '';
    }

}