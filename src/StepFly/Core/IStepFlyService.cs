using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Core
{
    public interface IStepFlyService
    {
        /// <summary>
        /// 修改为指定的步数
        /// </summary>
        Task<OperateResult> UpdateStepAsync(int stepNum, CancellationToken cancellationToken = default);
    }
}
