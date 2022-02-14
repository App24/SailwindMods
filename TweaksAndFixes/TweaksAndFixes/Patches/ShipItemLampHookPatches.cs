using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Scripts;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    public static class ShipItemLampHookPatches
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        public static class AwakePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled && __instance is ShipItemLampHook)
                {
                    __instance.gameObject.AddComponent<ShipItemLampHookItemHooked>();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook), "OnEnterInventory")]
        public static class OnEnterInventoryPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemLampHook __instance, ref bool ___occupied)
            {
                if (Main.enabled)
                {
                    ShipItemLampHookItemHooked component = __instance.GetComponent<ShipItemLampHookItemHooked>();
                    if (component && component.hookedItem && ___occupied)
                    {
                        component.hookedItem.GetComponent<HangableItem>().DisconnectJoint();
                        component.hookedItem = null;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook), "OnItemClick")]
        public static class OnItemClickPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemLampHook __instance, PickupableItem heldItem, ref bool ___occupied, ref bool __result)
            {
                if (!Main.enabled) return true;
                if (!__instance.sold || ___occupied)
                {
                    __result = false;
                    return false;
                }
                HangableItem component = heldItem.GetComponent<HangableItem>();
                if (component)
                {
                    component.ConnectJoint(__instance.GetComponent<Collider>());
                    __instance.GetComponent<ShipItemLampHookItemHooked>().hookedItem = heldItem;
                }
                __result = true;
                return false;
            }
        }
    }
}
