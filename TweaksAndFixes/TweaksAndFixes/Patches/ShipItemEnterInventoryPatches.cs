using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.MonoBehaviourScripts;

namespace TweaksAndFixes.Patches
{
    internal static class ShipItemEnterInventoryPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnEnterInventory")]
        private static class EnterInventory
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    __instance.GetComponent<ShipItemInventory>().inInventory = true;
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnLeaveInventory")]
        private static class ExitInventory
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    __instance.GetComponent<ShipItemInventory>().inInventory = false;
                }
            }
        }
    }
}
