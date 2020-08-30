import Vue from 'vue'
import App from './App.vue'
import { HttpClient, AutoDomainIntercepter } from "@/common/httpClient";
import { ServerUrl } from "@/common/environment";


/* 添加全局Http请求 */
//url拦截
var urlIntercept = new AutoDomainIntercepter(() => ServerUrl);
HttpClient.intercepters.push(urlIntercept);
//全局网络失败
HttpClient.httpErrorFilter = () => {
    console.log('httpClient 请求失败');
    uni.navigateTo({ url: '/pages/no-network' });
};

let httpClient = new HttpClient();

Vue.config.productionTip = false;
Vue.prototype.$httpClient = httpClient;

let vueApp = new App();
vueApp.$mount();