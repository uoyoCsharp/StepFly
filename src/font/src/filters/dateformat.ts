import { format } from 'date-fns'
import { zhCN } from 'date-fns/locale';

// 根据传入值将日期进行格式化
export function dateformat(value: any, formatStr: string = 'yyyy/MM/dd hh:mm:ss') {
    var date = new Date(value);
    return format(date, formatStr, { locale: zhCN });
}