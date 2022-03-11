using HarmonyLib;
using LowerGraphics.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LowerGraphics.Patches
{
    internal static class ToggleShadowPatches
    {
        [HarmonyPatch(typeof(ShipItemCompass), "OnEnterInventory")]
        public static class OnEnterInventoryPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (PlayerPrefs.GetInt("enableShadows") <= 0)
                {
                    ShadowsController.DisableShadows();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemCompass), "OnDrop")]
        public static class OnDropPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (PlayerPrefs.GetInt("enableShadows") <= 0)
                {
                    ShadowsController.DisableShadows();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemCompass), "ExtraLateUpdate")]
        public static class ExtraLateUpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItemCompass __instance)
            {
                if (Main.enabled && __instance.held && (__instance.sunCompassSundial || __instance.sharpenShadow))
                {
                    ShadowsController.EnableShadows();
                }
            }
        }
    }
}
