using MiCake.Audit;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    /// <summary>
    /// Vip 用户
    /// </summary>
    public class VIPUser : AggregateRoot, IHasModificationTime, IHasCreationTime
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 会员过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? ModificationTime { get; set; }

        public VIPUser()
        {
        }

        public static VIPUser Create(Guid userId, int effectDay)
        {
            return new VIPUser()
            {
                UserId = userId,
                Level = 1,
                ExpireTime = DateTime.Now.AddDays(effectDay)
            };
        }

        /// <summary>
        /// 设置会员等级
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(int level)
        {
            if (level > 7 || level < 0)
                throw new ArgumentException("会员等级不正确");

            Level = level;
        }
    }
}
