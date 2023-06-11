using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Microsoft.Extensions.Logging;

namespace VtlSoftware.Aspects.Logging21
{
    /// <summary>
    /// An attribute that applies logging to a Property.
    /// </summary>
    ///
    /// <remarks></remarks>
    ///
    /// <seealso cref="T:OverrideFieldOrPropertyAspect"/>

#pragma warning disable CS8618
    public class LogPropertyAttribute : OverrideFieldOrPropertyAspect
    {
        #region Fields

        /// <summary>
        /// (Immutable) The logger.
        /// </summary>
        [IntroduceDependency]
        private readonly ILogger logger;

        #endregion

        #region Public Methods
        /// <summary>
        /// Builds an aspect.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="builder">The builder.</param>
        ///
        /// <seealso cref="M:Metalama.Framework.Aspects.OverrideFieldOrPropertyAspect.BuildAspect(IAspectBuilder{IFieldOrProperty})"/>

        public override void BuildAspect(IAspectBuilder<IFieldOrProperty> builder)
        {
            if(!(builder.Target.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any() ||
                builder.Target.DeclaringType.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any()))
            {
                builder.Advice.Override(builder.Target, nameof(this.OverrideProperty));
            }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the override property.
        /// </summary>
        ///
        /// <value>The override property.</value>
        ///
        /// <seealso cref="P:Metalama.Framework.Aspects.OverrideFieldOrPropertyAspect.OverrideProperty"/>

        public override dynamic? OverrideProperty
        {
            get => meta.Proceed();
            set
            {
                var propertyName = $"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Property.Name}";
                var propType = meta.Target.Property.Type;
                var oldPropValue = meta.Target.Property.Value;
                meta.Proceed();
                var newPropValue = meta.Target.Property.Value;
                logger.Log(LogLevel.Information, $"The old value of {propertyName} was: {propType} = {oldPropValue}");
                logger.Log(LogLevel.Information, $"The new value of {propertyName} is: {propType} = {newPropValue}");
            }
        }

        #endregion
    }
}
