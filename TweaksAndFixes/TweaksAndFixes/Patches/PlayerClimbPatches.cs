using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes.Patches
{
    internal static class PlayerClimbPatches
    {
        [HarmonyPatch(typeof(PlayerClimb), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (!Main.enabled) return true;
                return !Main.paused;
            }
        }
    }
}
