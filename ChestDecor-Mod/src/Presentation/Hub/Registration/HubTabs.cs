using BepInEx.Configuration;
using KydlCore.Common;

namespace ChestDecor.Presentation.Hub.Registration;

/// <summary>What ChestDecor declares to the KY hub (F6, MIGRATION_DECISIONS B11): its title and its settings tab (every config entry).</summary>
internal static class HubTabs
{
    internal const string SettingsId = "settings";

    internal static bool RegisterModPage(ConfigFile config)
    {
        global::KydlCore.Features.Hub.Contracts.KyHub.SetModTitle(ChestDecor.Plugin.ModGuid, "Chest Decor");
        var tab = global::KydlCore.Features.Hub.Contracts.KyHub.AddTab(ChestDecor.Plugin.ModGuid, SettingsId, "Settings", () => global::KydlCore.Features.Hub.Contracts.KyHub.ConfigEntries(config), 0);
        ModLog.Verbose("HubTabs: settings tab " + (tab != null ? "registered" : "FAILED"));
        return tab != null;
    }

    internal static bool UnregisterModPage()
    {
        global::KydlCore.Features.Hub.Contracts.KyHub.RemoveTab(ChestDecor.Plugin.ModGuid, SettingsId);
        return !global::KydlCore.Features.Hub.Contracts.KyHub.HasTab(ChestDecor.Plugin.ModGuid, SettingsId);
    }
}
