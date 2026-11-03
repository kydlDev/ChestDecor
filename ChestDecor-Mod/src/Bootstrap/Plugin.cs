using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using Kydl.Common;
using UnityEngine;
using ChestDecor.Bootstrap;
using ChestDecor.Ui;

namespace ChestDecor
{
    /// <summary>
    /// ChestDecor: decorate chests (colours, attached items, positions). Cosmetic only.
    /// Entry point. Order in Awake matters: log first, then the config (player settings), then text, then everything that uses them.
    /// </summary>
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency(KydlCore.Plugin.ModGuid, "0.1.0")]     // the UI (hub, windows, input lock) and the KY-Items registry (manifest: KydlDev-KydlCore-0.1.0, same version)
    // Purely cosmetic: players without the mod can still join (they see the normal chest); when both sides have it the version must match.
    [NetworkCompatibility(CompatibilityLevel.VersionCheckOnly, VersionStrictness.Patch)]
    public class Plugin : BaseUnityPlugin
    {
        // ModGuid never changes (it is the config file name of 1.0.0: BepInEx/config/com.kydldev.chestdecor.cfg).
        // ModVersion = manifest.json version_number = newest CHANGELOG heading (tests check it).
        public const string ModGuid = "com.kydldev.chestdecor";
        public const string ModName = "ChestDecor";
        public const string ModVersion = "2.0.0";

        internal static Plugin Instance;
        Harmony _harmony;
        ConfigReloader _cfgWatcher;
        ModInstaller _installer;

        void Awake()
        {
            Instance = this;
            ModLog.Init(Logger, ModName, () => ModSettings.DebugLog != null && ModSettings.DebugLog.Value);
            ModLog.Info(ModName + " " + ModVersion + " loaded (dll built " + File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("yyyy-MM-dd HH:mm:ss") + ")");

            // The UI kit (Kydl.Ui), its log sinks, the one UI root and the input lock belong to KydlCore (2.0.0): nothing is set up here.
            ModSettings.Bind(Config);
            _cfgWatcher = new ConfigReloader(Config);
            Loc.Register(UiText.En);

            _installer = new ModInstaller(Config);
            bool installed = _installer.Install();
            Config.SettingChanged += (s, e) => { if (_installer != null) _installer.Refresh(); };
            ModLog.Verbose("Plugin: ModInstaller " + (installed ? "installed" : "reported a problem"));

            _harmony = new Harmony(ModGuid);
            int failed = PatchInstaller.Apply(_harmony);
            ModLog.Info(ModName + " ready: customize hotkey " + ModSettings.Hotkey.Value + (failed > 0 ? ", " + failed + " patch(es) missing" : ""));
        }

        /// A setting that decides what the Attach items tab lists changed: rebuild the list, also in an open window.
        internal static void OnCatalogSettingChanged(string what)
        {
            ModLog.Verbose("Plugin: '" + what + "' changed - attachment list rebuilt on next use");
            AttachCatalog.Invalidate();
            ChestDecorUI.CatalogChanged();
        }

        void Update()
        {
            if (_cfgWatcher != null) _cfgWatcher.Tick();
            DecorOpener.WatchWorld();
            if (!ModSettings.On) return;
            DecorOpener.Tick();
        }

        void OnDestroy()
        {
            if (_cfgWatcher != null) { _cfgWatcher.Dispose(); _cfgWatcher = null; }
            if (_installer != null) { _installer.Uninstall(); _installer = null; }
            DecorOpener.Shutdown();
            _harmony?.UnpatchSelf();
            NeutralTextures.Clear();
            ZoneMap.Clear();
            AttachCatalog.Invalidate();
            AttachVisuals.ClearCache();
            AttachCost.ClearCache();
            ModLog.Verbose("Plugin: destroyed (patches removed, caches cleared)");
        }
    }
}
