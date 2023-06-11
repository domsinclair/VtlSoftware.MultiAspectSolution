

using Polly;
using Polly.Retry;

namespace VtlSoftware.Aspects.Polly21
{
    /// <summary>
    /// A policy factory.
    /// </summary>
    ///
    /// <remarks></remarks>
    ///
    /// <seealso cref="T:IPolicyFactory"/>

    internal class PolicyFactory : IPolicyFactory
    {
        #region Fields

        /// <summary>
        /// (Immutable) The asynchronous retry.
        /// </summary>
        private static readonly AsyncRetryPolicy _asyncRetry = Policy.Handle<Exception>().WaitAndRetryAsync(
            new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4),
                TimeSpan.FromSeconds(8),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
            });
        /// <summary>
        /// (Immutable) The retry.
        /// </summary>
        private static readonly RetryPolicy _retry = Policy.Handle<Exception>().WaitAndRetry(
            new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4),
                TimeSpan.FromSeconds(8),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
            });

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets asynchronous policy.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="policyKind">The policy kind.</param>
        ///
        /// <returns>The asynchronous policy.</returns>
        ///
        /// <seealso cref="M:IPolicyFactory.GetAsyncPolicy(PolicyKind)"/>

        public AsyncPolicy GetAsyncPolicy(PolicyKind policyKind) => policyKind switch
        {
            PolicyKind.Retry => _asyncRetry,
            _ => throw new ArgumentOutOfRangeException(nameof(policyKind))
        };

        /// <summary>
        /// Gets a policy.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="policyKind">The policy kind.</param>
        ///
        /// <returns>The policy.</returns>
        ///
        /// <seealso cref="M:IPolicyFactory.GetPolicy(PolicyKind)"/>

        public Policy GetPolicy(PolicyKind policyKind) => policyKind switch
        {
            PolicyKind.Retry => _retry,
            _ => throw new ArgumentOutOfRangeException(nameof(policyKind))
        };

        #endregion
    }
}
