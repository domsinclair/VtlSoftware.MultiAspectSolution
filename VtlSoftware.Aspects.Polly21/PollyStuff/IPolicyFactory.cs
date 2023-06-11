

using Polly;

namespace VtlSoftware.Aspects.Polly21
{
    /// <summary>
    /// Interface for policy factory.
    /// </summary>
    ///
    /// <remarks></remarks>

    public interface IPolicyFactory
    {
        #region Public Methods
        /// <summary>
        /// Gets asynchronous policy.
        /// </summary>
        ///
        /// <param name="policyKind">The policy kind.</param>
        ///
        /// <returns>The asynchronous policy.</returns>

        AsyncPolicy GetAsyncPolicy(PolicyKind policyKind);

        /// <summary>
        /// Gets a policy.
        /// </summary>
        ///
        /// <param name="policyKind">The policy kind.</param>
        ///
        /// <returns>The policy.</returns>

        Policy GetPolicy(PolicyKind policyKind);

        #endregion
    }
}
