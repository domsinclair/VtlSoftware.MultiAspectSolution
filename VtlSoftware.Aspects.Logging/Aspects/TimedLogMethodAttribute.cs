using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

using VtlSoftware.Common.Standard21;

namespace VtlSoftware.Aspects.Logging
{
    /// <summary>
    /// An attribute that will add logging and timing to a Method.
    /// </summary>
    ///
    /// <remarks></remarks>
    ///
    /// <seealso cref="T:OverrideMethodAspect"/>

#pragma warning disable CS8618
#pragma warning disable IDE0063
    public class TimedLogMethodAttribute : OverrideMethodAspect
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
        /// <seealso cref="M:Metalama.Framework.Aspects.OverrideMethodAspect.BuildAspect(IAspectBuilder{IMethod})"/>

        public override void BuildAspect(IAspectBuilder<IMethod> builder)
        {
            if(!(builder.Target.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any() ||
                    builder.Target.Attributes.OfAttributeType(typeof(LogMethodAttribute)).Any()) ||
                builder.Target.DeclaringType.Attributes.OfAttributeType(typeof(NoLogAttribute)).Any())
            {
                builder.Advice.Override(builder.Target, nameof(this.OverrideMethod));
            }
        }

        /// <summary>
        /// Builds the eligibility criteria for this particular aspect.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <param name="builder">The builder.</param>
        ///
        /// <seealso cref="M:Metalama.Framework.Aspects.MethodAspect.BuildEligibility(IEligibilityBuilder{IMethod})"/>

        public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
        {
            base.BuildEligibility(builder);
            builder.MustNotBeStatic();
            builder.MustNotHaveAspectOfType(typeof(LogMethodAttribute));
        }

        /// <summary>
        /// Default template of the new method implementation.
        /// </summary>
        ///
        /// <remarks></remarks>
        ///
        /// <returns>A dynamic object that will be resolved at runtime.</returns>
        ///
        /// <seealso cref="M:Metalama.Framework.Aspects.OverrideMethodAspect.OverrideMethod()"/>

        public override dynamic? OverrideMethod()
        {
            var methodName = $"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Method.Name}";
            int paramCount = meta.Target.Parameters.Count;
            const string redacted = "<Redacted>";

            //add a check to see if we do actually want to do any logging at all
            var isTracingEnabled = this.logger.IsEnabled(LogLevel.Trace);

            if(isTracingEnabled)
            {
                //add the entry message

                using(var guard = LogRecursionGuard.Begin())
                {
                    if(guard.CanLog)
                    {
                        if(paramCount == 0)
                        {
                            logger.LogString(LogLevel.Information, $"Entering {methodName}.");
                        } else
                        {
                            Dictionary<string, object> parameters = new();
                            foreach(var p in meta.Target.Parameters)
                            {
                                if(p.RefKind != RefKind.Out)
                                {
                                    if(SensitiveDataFilter.IsSensitive(p))
                                    {
                                        parameters.Add($"Type = {p.Type}: Parameter Name ={p.Name}", redacted);
                                    } else
                                    {
                                        parameters.Add($"Type = {p.Type}: Parameter Name = {p.Name}", p.Value);
                                    }
                                } else
                                {
                                    //Metalame can't serialise an out parameter but it would help if we know it's there.
                                    parameters.Add(p.Name, " = <out>");
                                }
                            }
                            logger.Log(
                                LogLevel.Information,
                                $"Entering {methodName} with these parameters: {parameters}");
                        }
                    }
                }
            }
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                var result = meta.Proceed();
                if(isTracingEnabled)
                {
                    // Display the success message which will be different when the method is void.
                    bool isVoid = meta.Target.Method.ReturnType.Is(typeof(void));

                    using(var guard = LogRecursionGuard.Begin())
                    {
                        if(guard.CanLog)
                        {
                            if(isVoid)
                            {
                                logger.LogString(LogLevel.Information, $"Leaving {methodName}.");
                            } else
                            {
                                if(SensitiveDataFilter.IsSensitive(meta.Target.Method.ReturnParameter))
                                {
                                    logger.Log(
                                        LogLevel.Information,
                                        $"Leaving {methodName} with the following result which has been {redacted}");
                                } else
                                {
                                    logger.Log(
                                        LogLevel.Information,
                                        $"Leaving {methodName} with the following result: {result}");
                                }
                            }
                        }
                    }
                }
                return result;
            } catch(Exception e) when (this.logger.IsEnabled(LogLevel.Warning))
            {
                using(var guard = LogRecursionGuard.Begin())
                {
                    if(guard.CanLog)
                    {
                        logger.Log(LogLevel.Error, $"An error has occured in {methodName}. These are the details: {e}");
                    }
                }
                throw;
            } finally
            {
                watch.Stop();
                using(var guard = LogRecursionGuard.Begin())
                {
                    if(guard.CanLog)
                    {
                        logger.Log(
                            LogLevel.Information,
                            $"{methodName} took {watch.ElapsedMilliseconds} ms to complete.");
                    }
                }
            }
        }

        #endregion
    }
}
