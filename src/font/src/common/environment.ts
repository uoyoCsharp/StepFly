export const ServerUrl: string = 'http://localhost:5001';

export class MiCakeApiModel<TDataType> {
    statusCode: number = 0;
    isError: boolean = false;
    errorCode?: string;
    message?: string;
    result?: TDataType;
}