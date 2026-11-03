using Kydl.Common;
using Kydl.Core.Features;

namespace ChestDecor.Features.Camera.Infrastructure
{
    /// <summary>Free camera while the editor is open. Gate: ModSettings.On && ModSettings.FreeCameraEnabled.Value.</summary>
    internal sealed class CameraFeature : IFeature
    {
        internal const string FeatureId = "camera";
        internal static bool Running { get; private set; }
        public string Id { get { return FeatureId; } }
        public bool IsEnabled() { return ModSettings.On && ModSettings.FreeCameraEnabled.Value; }
        public bool Start() { Running = true; ModLog.Verbose("CameraFeature: started"); return Running; }
        public bool Stop() { Running = false; ModLog.Verbose("CameraFeature: stopped"); return !Running; }
    }
}
