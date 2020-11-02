using MiCake.Core.Util;
using System.Collections.Generic;

namespace StepFly.Utils
{
    public class IdentityHelper
    {
        /// <summary>
        /// 获取随机的设备ID，格式为：59:31:6b:e9:3c:bf
        /// </summary>
        /// <returns></returns>
        public static string GetRandomDeviceId()
        {
            string[] candidate = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h" };

            var tempList = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                tempList.Add(RandomHelper.GetRandomOf(candidate) + RandomHelper.GetRandomOf(candidate));
            }
            return string.Join(":", tempList);
        }
    }
}
