using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SkipTutorial.Patches
{
    public static class ShipItemScrollPatches
    {

        [HarmonyPatch(typeof(ShipItemScroll), "OnLoad")]
        private static class OnLoadPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItemScroll __instance, ref bool ___tutorialScroll, ref bool ___overrideEnableOutline)
            {
                if (Main.enabled)
                {
                    if (GameState.newGameRegion != PortRegion.none && ___tutorialScroll)
                    {
                        ___overrideEnableOutline = false;
                        if (Main.settings.DeleteScroll)
                        {
                            GameObject.Destroy(__instance.gameObject);
                        }
                    }
                }
            }
        }
    }
}
