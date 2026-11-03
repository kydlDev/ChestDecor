using Kydl.Common;
using Kydl.Core.Features;

namespace ChestDecor.Features.Decoration.Infrastructure
{
    /// <summary>Chests get the decorator, the Decorate hotkey / hover hint and the editor window. Gate: ModSettings.On.</summary>
    internal sealed class DecorationFeature : IFeature
    {
        internal const string FeatureId = "decoration";
        internal static bool Running { get; private set; }
        public string Id { get { return FeatureId; } }
        public bool IsEnabled() { return ModSettings.On; }
        public bool Start() { Running = true; ModLog.Verbose("DecorationFeature: started"); return Running; }
        public bool Stop() { Running = false; ModLog.Verbose("DecorationFeature: stopped"); return !Running; }
    }
}
