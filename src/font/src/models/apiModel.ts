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
    step: number | undefined;
}

//登录的结果模型
export class LoginResultModel {
    success: boolean | undefined;
    msg: string | undefined;
    token: string | undefined;
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
    candidateHomePics: string| undefined;
}

//修改步数结果的模型
export class ChangeStepResultModel {
    success: boolean | undefined;
    code: string | undefined;
    msg: string | undefined;
}