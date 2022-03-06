using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes.Patches
{
    internal static class PreventMovementWhenPausePatches
    {
        [HarmonyPatch(typeof(PlayerClimb), "Update")]
        public static class PlayerClimbUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (!Main.enabled) return true;
                return !Utils.GamePaused;
            }
        }

        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        public static class PlayerCrouchingUpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (!Main.enabled) return true;
                return !Utils.GamePaused;
            }
        }
    }
}
