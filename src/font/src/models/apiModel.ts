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

//通过乐心修改步数
export class LeXinChangeStepModel {
    phone: string | undefined;
    step: number | undefined;
}