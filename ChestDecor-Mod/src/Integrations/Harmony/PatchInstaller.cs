using System;
using HarmonyLib;
using KydlCore.Common;

namespace ChestDecor;

/// <summary>Applies the mod's Harmony patch classes, each on its own (MIGRATION_DECISIONS A5: patching lives in Integrations/Harmony).</summary>
internal static class PatchInstaller
{
    /// <summary>
    /// Applies every Harmony patch on its own. Harmony throws when a patch's target no longer exists or its signature changed (a game
    /// update); PatchAll would then stop at the first one and take the whole mod with it. Here a failing patch is logged with what stops
    /// working, and the others are still applied. A new patch class has to be added to this list.
    /// Expected outcome: returns the number of patches that failed (0 = all applied).
    /// </summary>
    internal static int Apply(HarmonyLib.Harmony harmony)
    {
        int failed = 0;
        failed += TryPatch(harmony, typeof(ContainerAwakePatch), "chests can't be decorated at all (nothing gets the decorator)");
        failed += TryPatch(harmony, typeof(ContainerHoverPatch), "the '[K] Decorate chest' hint is not shown (the hotkey still works)");
        failed += TryPatch(harmony, typeof(MenuUpdatePatch), "Esc also opens the pause menu when it closes the editor");
        failed += TryPatch(harmony, typeof(GameCameraLateUpdatePatch), "the free camera is off while the editor is open", FreeCamera.Unavailable);
        failed += TryPatch(harmony, typeof(ChestAttachmentDestroyRefundPatch), "attachments are not refunded when a chest is destroyed by damage");
        failed += TryPatch(harmony, typeof(ChestAttachmentRemoveRefundPatch), "attachments are not refunded to the player who removes a chest with the hammer");
        if (failed > 0)
            ModLog.Warn(failed + " patch(es) could not be applied, probably because the game changed; the rest of " + Plugin.ModName + " still works. Please report this with the game version.");
        else
            ModLog.Verbose("Plugin: all 6 patch classes applied");
        return failed;
    }

    /// Applies one patch class. Expected outcome: 0 when it was applied, 1 when it failed.
    static int TryPatch(HarmonyLib.Harmony harmony, Type patchClass, string consequence, Action onFailure = null)
    {
        try
        {
            var patched = harmony.CreateClassProcessor(patchClass).Patch();
            ModLog.Verbose("Plugin: patch " + patchClass.Name + " applied (" + (patched != null ? patched.Count : 0) + " method(s))");
            return patched != null && patched.Count > 0 ? 0 : 1;
        }
        catch (HarmonyException e) { return PatchFailed(patchClass, consequence, onFailure, e); }
        catch (Exception e) { return PatchFailed(patchClass, consequence, onFailure, e); }
    }

    static int PatchFailed(Type patchClass, string consequence, Action onFailure, Exception e)
    {
        ModLog.Error("Patch " + patchClass.Name + " failed, so " + consequence + ". " + e.GetType().Name + ": " + e.Message);
        try { onFailure?.Invoke(); }
        catch (Exception inner) { ModLog.Verbose("Plugin: fallback after " + patchClass.Name + " failed as well: " + inner.Message); }
        return 1;
    }

}
