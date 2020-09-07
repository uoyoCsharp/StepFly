import { Module, VuexModule, Mutation, Action, MutationAction } from "vuex-module-decorators";
import { UserState } from './types';

@Module({
    name: 'UserStoreModule',
    namespaced: true,
})
export class UserStoreModule extends VuexModule {
    public user: UserState = {
        name: '' || uni.getStorageSync('login-userName'),
        accessToken: '' || uni.getStorageSync('token'),
        lastLoginTime: '' || uni.getStorageSync('login-time'),
    };

    @Mutation
    public loginSuccess(data: { name: string, token: string }) {
        this.user.name = data.name;
        this.user.accessToken = data.token;
        this.user.lastLoginTime = new Date().getTime();
    }

    @Mutation
    public loginOut() {
        this.user.name = undefined;
        this.user.accessToken = undefined;
        this.user.lastLoginTime = 0;
    }

    @Action({ commit: 'loginSuccess' })
    public loginSuccessAction(data: { name: string, token: string }) {
        uni.setStorageSync('login', true);
        uni.setStorageSync('login-userName', data.name);
        uni.setStorageSync('login-time', new Date().getTime().toString());
        uni.setStorageSync('token', data.token);

        this.context.rootState.isLogin = true;
        return data;
    }

    @Action({ commit: 'loginOut' })
    public loginOutAction() {
        uni.removeStorageSync('login');
        uni.removeStorageSync('login-userName');
        uni.removeStorageSync('login-time');
        uni.removeStorageSync('token');

        this.context.rootState.isLogin = false;
        return '';
    }

}