using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Scripts;
using UnityEngine;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace TweaksAndFixes.Patches
{
    internal static class ShipItemPatches
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        public static class AwakePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    if (__instance is ShipItemLampHook)
                    {
                        __instance.gameObject.AddComponent<ShipItemLampHookItemHooked>();
                    }
                    else if (__instance.name.ToLower().Contains("map"))
                    {
                        __instance.gameObject.AddComponent<ShipItemMoveOnAltActivate>();
                        __instance.gameObject.AddComponent<ShipItemRotateOnAltActivate>().targetAngle=90f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnDrop")]
        public static class OnDropPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    ResetMove(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnEnterInventory")]
        public static class OnEnterInventoryPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    ResetMove(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnAltActivate")]
        public static class OnAltActivatePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    ShipRotate(__instance);
                    ShipMove(__instance);
                }
            }

            private static void ShipRotate(ShipItem __instance)
            {
                ShipItemRotateOnAltActivate shipRotateOnAltActivate = __instance.GetComponent<ShipItemRotateOnAltActivate>();
                if (shipRotateOnAltActivate)
                {
                    if (!__instance.sold || shipRotateOnAltActivate.rotating)
                    {
                        return;
                    }
                    shipRotateOnAltActivate.rotating = true;
                    if (__instance.heldRotationOffset != 0)
                    {
                        __instance.StartCoroutine(Rotate(shipRotateOnAltActivate, 0f));
                        return;
                    }
                    __instance.StartCoroutine(Rotate(shipRotateOnAltActivate, shipRotateOnAltActivate.targetAngle));
                }
            }

            private static void ShipMove(ShipItem __instance)
            {
                ShipItemMoveOnAltActivate shipMoveOnAltActivate = __instance.GetComponent<ShipItemMoveOnAltActivate>();
                if (shipMoveOnAltActivate)
                {
                    if (!__instance.sold || shipMoveOnAltActivate.moving)
                    {
                        return;
                    }
                    shipMoveOnAltActivate.moving = true;
                    if (__instance.holdDistance != shipMoveOnAltActivate.defaultDistance)
                    {
                        __instance.StartCoroutine(Move(shipMoveOnAltActivate, shipMoveOnAltActivate.defaultDistance));
                        return;
                    }
                    __instance.StartCoroutine(Move(shipMoveOnAltActivate, shipMoveOnAltActivate.targetDistance));
                }
            }

            public static IEnumerator Rotate(ShipItemRotateOnAltActivate shipRotateOnAltActivate, float target)
            {
                float start = shipRotateOnAltActivate.shipItem.heldRotationOffset;
                for (float t = 0f; t < 1f; t += Time.deltaTime * 3.22f)
                {
                    shipRotateOnAltActivate.shipItem.heldRotationOffset = Mathf.Lerp(start, target, t);
                    yield return new WaitForEndOfFrame();
                }
                shipRotateOnAltActivate.shipItem.heldRotationOffset = target;
                shipRotateOnAltActivate.rotating = false;
                Debug.Log("rotated.");
                yield break;
            }

            public static IEnumerator Move(ShipItemMoveOnAltActivate shipMoveOnAltActivate, float target)
            {
                float start = shipMoveOnAltActivate.shipItem.holdDistance;
                for (float t = 0f; t < 1f; t += Time.deltaTime * 3.22f)
                {
                    shipMoveOnAltActivate.shipItem.holdDistance = Mathf.Lerp(start, target, t);
                    yield return new WaitForEndOfFrame();
                }
                shipMoveOnAltActivate.shipItem.holdDistance = target;
                shipMoveOnAltActivate.moving = false;
                Debug.Log("moved.");
                yield break;
            }
        }

        public static void ResetMove(ShipItem __instance)
        {
            ShipItemMoveOnAltActivate shipMoveOnAltActivate = __instance.GetComponent<ShipItemMoveOnAltActivate>();
            if (shipMoveOnAltActivate)
            {
                __instance.holdDistance = shipMoveOnAltActivate.defaultDistance;
            }
        }
    }
}
