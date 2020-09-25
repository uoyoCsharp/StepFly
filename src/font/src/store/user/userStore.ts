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
        isLockout: '' || uni.getStorageSync('user-islockout'),
        roles: '' || uni.getStorageSync('user-roles'),
    };

    @Mutation
    public loginSuccess(data: { name: string, token: string, isLockout: boolean, roles: string }) {
        this.user.name = data.name;
        this.user.accessToken = data.token;
        this.user.isLockout = data.isLockout;
        this.user.roles = data.roles;
        this.user.lastLoginTime = new Date().getTime();
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
    public loginSuccessAction(data: { name: string, token: string, isLockout: boolean, roles: string }) {
        uni.setStorageSync('login', true);
        uni.setStorageSync('login-userName', data.name);
        uni.setStorageSync('user-islockout', data.isLockout);
        uni.setStorageSync('user-roles', data.roles);
        uni.setStorageSync('login-time', new Date().getTime().toString());
        uni.setStorageSync('token', data.token);

        this.context.rootState.isLogin = true;
        return data;
    }

    @Action({ commit: 'loginOut' })
    public loginOutAction() {
        // uni.removeStorageSync('login');
        // uni.removeStorageSync('login-userName');
        // uni.removeStorageSync('user-islockout');
        // uni.removeStorageSync('user-roles');
        // uni.removeStorageSync('login-time');
        // uni.removeStorageSync('token');
        uni.clearStorageSync();

        this.context.rootState.isLogin = false;
        return '';
    }

}