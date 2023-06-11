namespace VtlSoftware.Aspects.Logging
{
    /// <summary>
    /// An attribute for redacting parameter or return values. This class cannot be inherited.
    /// </summary>
    ///
    /// <remarks>
    /// If applied in front of Parameters or the retun value of a method the actual value will be logged as "Redacted".
    /// </remarks>
    ///
    /// <seealso cref="T:Attribute"/>

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class RedactAttribute : Attribute
    {
    }
}
