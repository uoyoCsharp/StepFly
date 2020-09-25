using MiCake.Audit;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    public class HomeConfig : AggregateRoot, IHasModificationTime
    {
        /// <summary>
        /// 站点信息
        /// </summary>
        public string SiteName { get; private set; }

        /// <summary>
        /// 要展示的语句
        /// </summary>
        public string ShowSentence { get; private set; }

        /// <summary>
        /// Footer 显示信息
        /// </summary>
        public string FooterInfo { get; private set; }

        /// <summary>
        /// 首页显示的图片候选集合
        /// </summary>
        public string CandidateHomePics { get; private set; }

        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// 设置主页图片候选集
        /// </summary>
        public void SetHomePics(string pics)
        {
            CandidateHomePics = pics;
        }

        public HomeConfig()
        {
        }
    }
}
