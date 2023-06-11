using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Microsoft.Extensions.Logging;

namespace VtlSoftware.Aspects.Polly21
{
    public class RetryAttribute : OverrideMethodAspect
    {
        #region Fields
        [IntroduceDependency]
        private readonly IPolicyFactory _policyFactory;
        [IntroduceDependency]
        private readonly ILogger logger;

        #endregion

        #region Constructors
        public RetryAttribute(PolicyKind kind = PolicyKind.Retry) { this.Kind = kind; }

        #endregion

        #region Public Methods
        // Template for async methods.
        public override async Task<dynamic?> OverrideAsyncMethod()
        {
            var methodName = $"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Method.Name}";
            async Task<object?> ExecuteCoreAsync(CancellationToken cancellationToken)
            {
                try
                {
                    return await meta.ProceedAsync();
                } catch(Exception e)
                {
                    logger.Log(
                        LogLevel.Error,
                        $"An error thrown by retry attribute, {methodName} has failed withe the following error {e}");
                    throw;
                }
            }

            var cancellationTokenParameter
                = meta.Target.Parameters.Where(p => p.Type.Is(typeof(CancellationToken))).LastOrDefault();

            var policy = this._policyFactory.GetAsyncPolicy(this.Kind);
            return await policy.ExecuteAsync(
                ExecuteCoreAsync,
                cancellationTokenParameter != null
                    ? (CancellationToken)cancellationTokenParameter.Value
                    : CancellationToken.None);
        }

        // Template for non-async methods.
        public override dynamic? OverrideMethod()
        {
            var methodName = $"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Method.Name}";
            object? ExecuteCore()
            {
                try
                {
                    return meta.Proceed();
                } catch(Exception e)
                {
                    logger.Log(
                        LogLevel.Error,
                        $"An error thrown by retry attribute, {methodName} has failed withe the following error {e}");
                    throw;
                }
            }

            var policy = this._policyFactory.GetPolicy(this.Kind);
            return policy.Execute(ExecuteCore);
        }

        #endregion

        #region Public Properties
        public PolicyKind Kind { get; }

        #endregion
    }
}
