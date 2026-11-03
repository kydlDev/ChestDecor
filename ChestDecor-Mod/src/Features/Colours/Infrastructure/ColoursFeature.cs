using Kydl.Common;
using Kydl.Core.Features;

namespace ChestDecor.Features.Colours.Infrastructure
{
    /// <summary>Colour a chest (tint / texture, per part). Gate: ModSettings.On && ModSettings.ColoursEnable.Value.</summary>
    internal sealed class ColoursFeature : IFeature
    {
        internal const string FeatureId = "colours";
        internal static bool Running { get; private set; }
        public string Id { get { return FeatureId; } }
        public bool IsEnabled() { return ModSettings.On && ModSettings.ColoursEnable.Value; }
        public bool Start() { Running = true; ModLog.Verbose("ColoursFeature: started"); return Running; }
        public bool Stop() { Running = false; ModLog.Verbose("ColoursFeature: stopped"); return !Running; }
    }
}
