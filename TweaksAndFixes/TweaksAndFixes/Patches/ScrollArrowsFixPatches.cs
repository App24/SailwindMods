using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes.Patches
{
    public static class ScrollArrowsFixPatches
    {
        [HarmonyPatch(typeof(ShipItemScroll), "OnLoad")]
        public static class OnLoadPatch
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
