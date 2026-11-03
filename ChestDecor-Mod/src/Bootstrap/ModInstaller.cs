using BepInEx.Configuration;
using ChestDecor.Features.Attachments.Infrastructure;
using ChestDecor.Features.Camera.Infrastructure;
using ChestDecor.Features.Colours.Infrastructure;
using ChestDecor.Features.Decoration.Infrastructure;
using ChestDecor.Presentation.Hub.Registration;
using KydlCore.Common;
using KydlCore.Core;

namespace ChestDecor.Bootstrap
{
    /// <summary>Composition root: the mod's KY hub page (title + settings tab) and its features in the FeatureRegistry (MIGRATION_DECISIONS A4).</summary>
    internal sealed class ModInstaller
    {
        internal static ModInstaller Current { get; private set; }
        readonly ConfigFile _config;
        internal FeatureRegistry Features { get; }
        internal ModInstaller(ConfigFile config) { _config = config; Features = new FeatureRegistry(line => ModLog.Verbose(line)); }

        internal bool Install()
        {
            Current = this;
            bool page = HubTabs.RegisterModPage(_config);
            bool added = true;
            foreach (IFeature feature in new IFeature[] { new DecorationFeature(), new ColoursFeature(), new AttachmentsFeature(), new CameraFeature() })
            {
                OperationResponse<int> add = Features.Add(feature);
                if (!add.IsSuccess) { added = false; ModLog.Error("ModInstaller: " + add.Reason); }
            }
            return page && added && Refresh();
        }

        internal bool Refresh()
        {
            OperationResponse<int> r = Features.Refresh();
            if (r.Status == ResponseStatus.Failed) ModLog.Warn("ModInstaller: " + r.Reason); else ModLog.Verbose("ModInstaller: refresh - " + r);
            return r.IsSuccess;
        }

        internal bool Uninstall()
        {
            OperationResponse<int> r = Features.StopAll();
            bool gone = HubTabs.UnregisterModPage();
            if (Current == this) Current = null;
            return r.IsSuccess && gone;
        }
    }
}
