import Vue from 'vue'
import App from './App.vue'
import { HttpClient, AutoDomainIntercepter, JwtTokenIntercepter, MiCakeResponseNoAuthorizeFilter } from "@/common/httpClient";
import { ServerUrl } from "@/common/environment";
import VuexStore from "./store";
import { UserStoreModule } from "./store/user/userStore";
import { getModule } from 'vuex-module-decorators';

/* 添加全局Http请求 */
//url拦截
var urlIntercept = new AutoDomainIntercepter(() => ServerUrl);
HttpClient.intercepters.push(urlIntercept);
//全局网络失败
HttpClient.httpErrorFilter = () => {
    console.log('httpClient 请求失败');
    uni.navigateTo({ url: '/pages/no-network' });
};
//全局权限处理
let authorizeFilter = new MiCakeResponseNoAuthorizeFilter(() => {
    uni.navigateTo({
        url: '/pages/no-authorize'
    });
});
HttpClient.resultFilters.push(authorizeFilter);
//全局jwt header
var userStoreModule = getModule(UserStoreModule, VuexStore).user;
var jwtIntercept = new JwtTokenIntercepter(() => {
    if (VuexStore.state.isLogin) {
        return userStoreModule.accessToken ?? '';
    }
    return null;
});
HttpClient.intercepters.push(jwtIntercept);

let httpClient = new HttpClient();

Vue.config.productionTip = false;
Vue.prototype.$httpClient = httpClient;

let vueApp = new App({
    store: VuexStore
});
vueApp.$mount();