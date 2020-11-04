export default class VersionHelper {

    // 对两个版本信息进行对比，如果version大于compareversion则返回true，否则返回false
    public static compareVersion(version: string, compareversion: string): boolean {
        if (!version || !compareversion) return false;

        let versionNum = this.getVersionNumber(version);
        let compareversionNum = this.getVersionNumber(compareversion);
        let minVersionCount = versionNum.length > compareversionNum.length ? compareversionNum.length : versionNum.length;

        for (let index = 0; index < minVersionCount; index++) {
            if (versionNum[index] > compareversionNum[index])
                return true
        }
        return false;
    }


    // 返回版本的数字号
    private static getVersionNumber(version: string): Array<number> {
        let reslut: Array<number> = [];
        for (let index = 0; index < version.length; index++) {
            const element = version[index];
            if ("0123456789".indexOf(element) > -1) {
                reslut.push(parseInt(element));
            }
        }
        return reslut;
    }
}