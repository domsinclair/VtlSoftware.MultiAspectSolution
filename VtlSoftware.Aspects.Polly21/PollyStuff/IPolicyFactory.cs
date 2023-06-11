using Polly;

namespace VtlSoftware.Aspects.Polly21
{
    public interface IPolicyFactory
    {
        #region Public Methods

        AsyncPolicy GetAsyncPolicy(PolicyKind policyKind);

        Policy GetPolicy(PolicyKind policyKind);

        #endregion
    }
}
