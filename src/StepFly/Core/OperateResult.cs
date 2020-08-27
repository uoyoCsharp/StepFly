using System;

namespace StepFly.Core
{
    /// <summary>
    /// 操作返回结果包装类
    /// </summary>
    public class OperateResult
    {
        /// <summary>
        /// 指示当前的操作是否成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 失败所捕获的<see cref="Exception"/>
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 操作结果的详细资料信息
        /// </summary>
        public ResultInformation Information { get; private set; }

        public static OperateResult Success()
            => new OperateResult() { Succeeded = true };

        public static OperateResult Success(string code, string description)
        {
            var result = new OperateResult
            {
                Succeeded = true
            };

            result.Information = new ResultInformation()
            {
                Code = code,
                Description = description
            };

            return result;
        }

        public static OperateResult Success(string code, string description, object playLoad)
        {
            var result = new OperateResult
            {
                Succeeded = true
            };

            result.Information = new ResultInformation()
            {
                Code = code,
                Description = description,
                PlayLoad = playLoad
            };

            return result;
        }

        public static OperateResult Failed(Exception ex)
        {
            var result = new OperateResult
            {
                Succeeded = false,
                Exception = ex
            };

            return result;
        }

        public static OperateResult Failed(Exception ex, string errorCode, string description)
        {
            var result = new OperateResult
            {
                Succeeded = false,
                Exception = ex
            };

            result.Information = new ResultInformation()
            {
                Code = errorCode,
                Description = description
            };

            return result;
        }

        public static OperateResult Failed(Exception ex, string errorCode, string description, object playLoad)
        {
            var result = new OperateResult
            {
                Succeeded = false,
                Exception = ex
            };

            result.Information = new ResultInformation()
            {
                Code = errorCode,
                Description = description,
                PlayLoad = playLoad
            };

            return result;
        }

        public override string ToString()
        {
            return Succeeded
                ? "Succeeded"
                : string.Format("{0} : {1}", "Failed", Information?.Description);
        }
    }

    /// <summary>
    /// 操作结果的信息类
    /// </summary>
    public class ResultInformation
    {
        /// <summary>
        /// 用于描述业务操作的code信息
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 操作结果的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作结果的荷载信息
        /// </summary>
        public object PlayLoad { get; set; }
    }
}
