using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;

namespace VtlSoftware.Aspects.Logging
{
    /// <summary>
    /// A collection of fabric extensions.
    /// </summary>
    ///
    /// <remarks></remarks>

    [CompileTime]
    public static class FabricExtensions
    {
        #region Public Methods

        /// <summary>
        /// An IProjectAmender extension method that logs all methods, by applying the [LogMethod] attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored as will those marked with the [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAllMethods(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(type => !type.IsStatic || type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            .SelectMany(type => type.Methods)
            .Where(method => method.Name != "ToString")
            .AddAspectIfEligible<LogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs all properties.
        /// </summary>
        ///
        /// <remarks>Static classes will be ignored.</remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAllProperties(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(type => !type.IsStatic)
            .SelectMany(type => type.Properties)
             .AddAspectIfEligible<LogPropertyAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs all public and private methods, by applying the [LogMethod]
        /// attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored, as will those marked with the [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAllPublicAndPrivateMethods(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(
                type => type.Accessibility is Accessibility.Public or Accessibility.Internal &&
                    !type.IsStatic ||
                    type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            .SelectMany(type => type.Methods)
            .Where(
                method => method.Accessibility is Accessibility.Public or Accessibility.Private &&
                    method.Name != "ToString")
            .AddAspectIfEligible<LogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs all public methods, by applying the [LogMethod] attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored, as will those marked with the [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAllPublicMethods(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(
                type => type.Accessibility is Accessibility.Public or Accessibility.Internal &&
                    !type.IsStatic ||
                    type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            .SelectMany(type => type.Methods)
            .Where(method => method.Accessibility is Accessibility.Public && method.Name != "ToString")
            .AddAspectIfEligible<LogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs and times all methods, by applyining the [TimedLogMethod]
        /// attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored, as will those marked with the [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAndTimeAllMethods(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(type => !type.IsStatic || type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            .SelectMany(type => type.Methods)
            .Where(method => method.Name != "ToString")
            .AddAspectIfEligible<TimedLogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs and time all public and private methods, by applying the
        /// [TimedLogMethod] attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored, as will those marked with the  [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAndTimeAllPublicAndPrivateMethods(this IProjectAmender amender)
        {
            amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(
                type => type.Accessibility is Accessibility.Public or Accessibility.Internal &&
                    !type.IsStatic ||
                    type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            .SelectMany(type => type.Methods)
            .Where(
                method => method.Accessibility is Accessibility.Public or Accessibility.Private &&
                    method.Name != "ToString")
            .AddAspectIfEligible<TimedLogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs and times all public methods, by applying the [TimedLogMethod]
        /// attribute.
        /// </summary>
        ///
        /// <remarks>
        /// Static classes will be ignored, as will those marked with the [NoLog] attribute.
        /// </remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogAndTimeAllPublicMethods(this IProjectAmender amender)
        {
            amender.Outbound
           .SelectMany(compilation => compilation.AllTypes)
           .Where(
               type => type.Accessibility is Accessibility.Public or Accessibility.Internal &&
                   !type.IsStatic ||
                   type.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
           .SelectMany(type => type.Methods)
           .Where(method => method.Accessibility is Accessibility.Public && method.Name != "ToString")
           .AddAspectIfEligible<TimedLogMethodAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that logs an everything, by applying the [LogMethod] attribute to
        /// methods and the  [LogProperty] attribute to properties.
        /// </summary>
        ///
        /// <remarks>Static classes will be ignored.</remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void LogEverything(this IProjectAmender amender)
        {
            var types = amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(type => !type.IsStatic);
            types.SelectMany(type => type.Methods)
                .Where(method => method.Name != "ToString")
                .AddAspectIfEligible<LogMethodAttribute>();
            types.SelectMany(type => type.Properties)
                .AddAspectIfEligible<LogPropertyAttribute>();
        }

        /// <summary>
        /// An IProjectAmender extension method that Times every method and logs every property.
        /// </summary>
        ///
        /// <remarks>Static classes will be ignored.</remarks>
        ///
        /// <param name="amender">The amender to act on.</param>

        public static void TimeEveryMethodAndLogEveryProperty(this IProjectAmender amender)
        {
            var types = amender.Outbound
            .SelectMany(compilation => compilation.AllTypes)
            .Where(type => !type.IsStatic);
            types.SelectMany(type => type.Methods)
                .Where(method => method.Name != "ToString")
                .AddAspectIfEligible<TimedLogMethodAttribute>();
            types.SelectMany(type => type.Properties)
                .AddAspectIfEligible<LogPropertyAttribute>();
        }

        #endregion
    }
}
