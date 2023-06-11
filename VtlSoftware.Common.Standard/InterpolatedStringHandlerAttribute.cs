#if !NET5_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates the attributed type is to be used as an interpolated string handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class InterpolatedStringHandlerAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="InterpolatedStringHandlerAttribute"/>.
        /// </summary>
        public InterpolatedStringHandlerAttribute()
        {
        }

        #endregion
    }
}

#endif
