using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using System.Linq;

namespace VtlSoftware.Aspects.Logging21
{
    /// <summary>
    /// A sensitive data filter.
    /// </summary>
    ///
    /// <remarks></remarks>

    [CompileTime]
    internal static class SensitiveDataFilter
    {
        #region Public Methods

        /// <summary>
        /// Query if 'parameter' is sensitive.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="parameter">The parameter.</param>
        ///
        /// <returns>True if sensitive, false if not.</returns>

        public static bool IsSensitive(IParameter parameter)
        {
            bool result = false;
            if(parameter.Attributes.OfAttributeType(typeof(RedactAttribute)).Any())
            {
                result = true;
            }

            return result;

            //var options = parameter.Compilation.Project.LoggingOptions();

            //return options.SensitiveData.Contains(parameter.Name);
        }

        #endregion
    }
}
