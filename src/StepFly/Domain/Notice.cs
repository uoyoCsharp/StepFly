using MiCake.Audit;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    public class Notice : AggregateRoot, IHasCreationTime, IHasModificationTime
    {
        /// <summary>
        /// 公告内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 有效无效
        /// </summary>
        public bool IsEffective { get; private set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; private set; }

        public DateTime? ModificationTime { get; set; }
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 设置公告内容
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string content)
           => Content = content;

        /// <summary>
        /// 设置为无效
        /// </summary>
        public void SetToInvalid()
            => IsEffective = false;

        /// <summary>
        /// 设置优先级
        /// </summary>
        /// <param name="order"></param>
        public void ChangeOrder(int order)
            => Order = order;
    }
}
