export default class NumberHelper {
    //获取指定范围的随机整数
    public static getRandomNumInt(min: number, max: number) {
        var Range = max - min;
        var Rand = Math.random(); //获取[0-1）的随机数
        return (min + Math.round(Rand * Range)); //放大取整
    }
}