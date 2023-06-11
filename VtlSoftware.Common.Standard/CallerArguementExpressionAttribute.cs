#if !NET5_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerArgumentExpressionAttribute : Attribute
    {
        #region Constructors

        public CallerArgumentExpressionAttribute(string parameterName) { ParameterName = parameterName; }

        #endregion

        #region Public Properties
        public string ParameterName { get; }

        #endregion
    }
}

#endif
