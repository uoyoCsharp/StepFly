using MiCake.Audit;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedBack : AggregateRoot, IHasCreationTime
    {
        /// <summary>
        /// 反馈用户
        /// </summary>
        public string UserKey { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 分类标签
        /// </summary>
        public string Tag { get; private set; }

        public DateTime CreationTime { get; set; }

        public FeedBack()
        {
        }

        public static FeedBack Create(string title, string content)
            => new FeedBack() { Title = title, Content = content };

        public void SetFeedBackUser(string userKey)
        {
            UserKey = userKey;
        }

        public void SetTag(string tag)
            => Tag = tag;
    }
}
