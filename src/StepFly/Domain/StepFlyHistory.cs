using MiCake.Audit;
using MiCake.Core.Util;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    /// <summary>
    /// 操作成功之后的历史结果
    /// </summary>
    public class StepFlyHistory : AggregateRoot<long>, IHasCreationTime
    {
        /// <summary>
        /// 用户Key
        /// </summary>
        public string UserKeyInfo { get; private set; }

        /// <summary>
        /// 提交的步数
        /// </summary>
        public int StepNum { get; private set; }

        /// <summary>
        /// 来源[乐心Or小米]
        /// </summary>
        public int Source { get; set; }

        public DateTime CreationTime { get; set; }

        public StepFlyHistory()
        {
        }

        public StepFlyHistory(string userKey, int stepNum)
        {
            CheckValue.NotNullOrWhiteSpace(userKey, nameof(userKey));

            UserKeyInfo = userKey;
            StepNum = stepNum;
            Source = (int)StepFlyProviderType.XiaoMi;
        }

        public void SetSource(StepFlyProviderType type)
        {
            Source = (int)type;
        }
    }
}
