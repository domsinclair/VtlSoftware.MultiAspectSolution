

using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Microsoft.Extensions.Logging;

namespace VtlSoftware.Aspects.Polly21
{
    /// <summary>
    /// Attribute for retry.
    /// </summary>
    ///
    /// <remarks></remarks>
    ///
    /// <seealso cref="T:OverrideMethodAspect"/>

    public class RetryAttribute : OverrideMethodAspect
    {
        #region Fields

        /// <summary>
        /// (Immutable) The policy factory.
        /// </summary>
        [IntroduceDependency]
        private readonly IPolicyFactory _policyFactory;
        /// <summary>
        /// (Immutable) The logger.
        /// </summary>
        [IntroduceDependency]
        private readonly ILogger logger;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="kind">(Optional) The kind.</param>

        public RetryAttribute(PolicyKind kind = PolicyKind.Retry) { this.Kind = kind; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Template for async methods.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <returns>A dynamic?</returns>

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

        /// <summary>
        /// Template for non-async methods.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <returns>A dynamic?</returns>

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
        /// <summary>
        /// Gets the kind.
        /// </summary>
        ///
        /// <value>The kind.</value>

        public PolicyKind Kind { get; }

        #endregion
    }
}
