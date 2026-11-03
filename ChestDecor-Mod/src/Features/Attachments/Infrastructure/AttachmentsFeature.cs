using KydlCore.Common;
using KydlCore.Core;

namespace ChestDecor.Features.Attachments.Infrastructure
{
    /// <summary>Attach items to a chest (catalog, cost and refund, placement). Gate: ModSettings.On && ModSettings.AttachmentsEnable.Value.</summary>
    internal sealed class AttachmentsFeature : IFeature
    {
        internal const string FeatureId = "attachments";
        internal static bool Running { get; private set; }
        public string Id { get { return FeatureId; } }
        public bool IsEnabled() { return ModSettings.On && ModSettings.AttachmentsEnable.Value; }
        public bool Start() { Running = true; ModLog.Verbose("AttachmentsFeature: started"); return Running; }
        public bool Stop() { Running = false; ModLog.Verbose("AttachmentsFeature: stopped"); return !Running; }
    }
}
