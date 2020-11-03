import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import { RootState } from './types';
import { UserStoreModule } from "./user/userStore";
import { HomeStoreModule } from "./home/homeStore";
import { VipInfoStoreModule } from "./vipInfo/vipInfoStore";

Vue.use(Vuex);

const store: StoreOptions<RootState> = {
    state: {
        isLogin: '' || (uni.getStorageSync('login') ?? false)
    },
    modules: {
        UserStoreModule,
        HomeStoreModule,
        VipInfoStoreModule
    }
};

export default new Vuex.Store<RootState>(store);