export default class ApiHelper { 
    /**
     * 将api所返回的图片字节数据转换为base64数据供img显示
     */
    public static getImgBase64Data(imgByteData:any):string {
        return 'data:image/png;base64,' + btoa(new Uint8Array(imgByteData).reduce((data, byte) => data + String.fromCharCode(byte), ''));
    }
}