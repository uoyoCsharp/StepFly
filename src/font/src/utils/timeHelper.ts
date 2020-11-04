import { addHours,isToday} from 'date-fns'

export default class TimeHelper {
    public static toBeiJing(time: Date) {
        return addHours(time, 8);
    }

    public static isToday(date:Date){
        return isToday(date);
    }
}