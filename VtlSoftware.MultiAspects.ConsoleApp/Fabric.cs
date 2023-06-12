using Metalama.Framework.Fabrics;
using VtlSoftware.Aspects.Logging21;

namespace VtlSoftware.MultiAspects.ConsoleApp
{
    internal class Fabric : ProjectFabric
    {
        #region Public Methods

        public override void AmendProject(IProjectAmender amender) { amender.LogAndTimeAllMethods(); }

        #endregion
    }
}
