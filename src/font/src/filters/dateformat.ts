import { format } from 'date-fns'
import { zhCN } from 'date-fns/locale';
import TimeHelper from "@/utils/timeHelper";

// 根据传入值将日期进行格式化
export function dateformat(value: any, formatStr: string = 'yyyy/MM/dd HH:mm:ss') {
    var date = TimeHelper.toBeiJing(new Date(value));
    return format(date, formatStr, { locale: zhCN });
}