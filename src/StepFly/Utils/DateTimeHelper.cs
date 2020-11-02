using System;

namespace StepFly.Utils
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取对应时间的时间戳
        /// </summary>
        /// <param name="timeStampType"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime date, TimeStampType timeStampType = TimeStampType.TenLength)
        {
            //1970-01-01的Ticks值
            long tStart = 621355968000000000;
            TimeSpan ts = new TimeSpan(new DateTimeOffset(date).Ticks - tStart);

            if (timeStampType == TimeStampType.TenLength)
                return Convert.ToInt64(ts.TotalSeconds);

            if (timeStampType == TimeStampType.TenLength)
                return Convert.ToInt64(ts.TotalMilliseconds);

            return 0;
        }

        /// <summary>
        /// 将时间戳转换为datetime
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="timeStampType"></param>
        /// <returns></returns>
        public static DateTime ConvertStampToDateTime(long stamp, TimeStampType timeStampType = TimeStampType.TenLength)
        {
            TimeSpan ts = default;

            if (timeStampType == TimeStampType.TenLength)
                ts = new TimeSpan((stamp * 1000 * 10000) + 621355968000000000);
            else if (timeStampType == TimeStampType.ThirteenLength)
                ts = new TimeSpan((stamp * 10000) + 621355968000000000);

            return new DateTime(ts.Ticks);
        }
    }


    /// <summary>
    /// 需要获取时间戳的类型
    /// </summary>
    public enum TimeStampType
    {
        /// <summary>
        /// 10位长度
        /// </summary>
        TenLength = 0,

        /// <summary>
        /// 13位长度
        /// </summary>
        ThirteenLength = 1
    }
}
