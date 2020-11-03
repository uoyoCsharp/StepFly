// 乐心的API所返回的API格式
export class LeXinApiResponse<T>  {
    code: undefined;
    msg?: string | undefined;
    data?: T | undefined;
}

// 发送乐心验证码
export class SendPhoneCodeModel {
    phone: string | undefined;
    imageCode: string | undefined;
}

// 登录到乐心的验证码
export class LoginToLeXinModel {
    phone: string | undefined;
    code: string | undefined;
}

//使用账号密码登录的模型
export class LoginToLeXinWithPwdModel {
    phone: string | undefined;
    password: string | undefined;
}

//通过乐心修改步数
export class LeXinChangeStepModel {
    phone: string | undefined;
    step: number = 0;
}

//登录的结果模型
export class LoginResultModel {
    userId: string | undefined;
    success: boolean | undefined;
    msg: string | undefined;
    token: string | undefined;
    isLockout: boolean | undefined;
    roles: string | undefined;
}

//公告消息的模型
export class NoticeModel {
    id: number | undefined;
    content: string | undefined;
    order: number = 0;
}

//主页的模型
export class HomeModel {
    siteName: string | undefined;
    footerInfo: string | undefined;
    showSentence: string | undefined;
    candidateHomePics: string | undefined;
}

//修改步数结果的模型
export class ChangeStepResultModel {
    success: boolean | undefined;
    code: string | undefined;
    msg: string | undefined;
}

// 绑定的类型
export class BindingTypeModel {
    wechatBinding: boolean | undefined;
    lifeSenseSportFollowing: boolean | undefined;
    qqBinding: boolean | undefined;
    antForestBinding: boolean | undefined;
}

// 更改步数的记录
export class StepFlyHistoryModel {
    id?: number;
    userKeyInfo?: string;
    stepNum?: number;
    creationTime?: string;
}

// 管理端-用户列表模型
export class StepFlyUserItemModel {
    userKeyInfo?: string;
    userSystemId?: string;
    loginTime?: string;
    isLockout?: boolean;
    modificationTime?: boolean;
}

// 用户反馈
export class FeedbackItemModel {
    id?: number;
    userKey?: string;
    title?: string;
    content?: string;
    tag?: string;
}

// 提交用户反馈
export class AddFeedbackModel {
    userKey?: string;
    title?: string;
    content?: string;
    tag?: string;
}

// 小米登录的模型
export class LoginToXiaoMiModel {
    phone?: string;
    password?: string;
}

// 会员的VIP信息
export class UserVipModel {
    isVip?: boolean;
    level?: number;
    expireTime?: string;
}
