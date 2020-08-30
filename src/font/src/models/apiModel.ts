// 乐心的API所返回的API格式
export class LeXinApiResponse<T>  {
    code: undefined;
    msg?: string | undefined;
    data?: T | undefined;
}