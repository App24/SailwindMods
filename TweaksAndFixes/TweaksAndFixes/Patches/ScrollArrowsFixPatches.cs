using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes.Patches
{
    internal static class ScrollArrowsFixPatches
    {
        [HarmonyPatch(typeof(ShipItemScroll), "OnLoad")]
        private static class OnLoadPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItemScroll __instance)
            {
                if (Main.enabled)
                {
                    __instance.InvokePrivateMethod("HideArrows");
                }
            }
        }
    }
}
