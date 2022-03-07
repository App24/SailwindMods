using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.MonoBehaviourScripts;
using UnityEngine;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace TweaksAndFixes.Patches
{
    internal static class ShipItemMiscPatches
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        private static class AwakePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    __instance.gameObject.AddComponent<ShipItemInventory>();
                    if (__instance is ShipItemLampHook)
                    {
                        __instance.gameObject.AddComponent<ShipItemLampHookItemHooked>();
                    }
                    else if (__instance.name.ToLower().Contains("map"))
                    {
                        __instance.gameObject.AddComponent<ShipItemMoveOnAltActivate>();
                        __instance.gameObject.AddComponent<ShipItemRotateOnAltActivate>().targetAngle = 90f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnDrop")]
        private static class OnDropPatch
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
        private static class OnEnterInventoryPatch
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
        private static class OnAltActivatePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                if (Main.enabled)
                {
                    Rotate(__instance);
                    Move(__instance);
                }
            }

            private static void Rotate(ShipItem instance)
            {
                ShipItemRotateOnAltActivate shipRotateOnAltActivate = instance.GetComponent<ShipItemRotateOnAltActivate>();
                if (shipRotateOnAltActivate)
                {
                    if (!instance.sold || shipRotateOnAltActivate.rotating)
                    {
                        return;
                    }
                    shipRotateOnAltActivate.rotating = true;
                    if (instance.heldRotationOffset != 0)
                    {
                        instance.StartCoroutine(RotateShipItem(shipRotateOnAltActivate, 0f));
                        return;
                    }
                    instance.StartCoroutine(RotateShipItem(shipRotateOnAltActivate, shipRotateOnAltActivate.targetAngle));
                }
            }

            private static void Move(ShipItem instance)
            {
                ShipItemMoveOnAltActivate shipMoveOnAltActivate = instance.GetComponent<ShipItemMoveOnAltActivate>();
                if (shipMoveOnAltActivate)
                {
                    if (!instance.sold || shipMoveOnAltActivate.moving)
                    {
                        return;
                    }
                    shipMoveOnAltActivate.moving = true;
                    if (instance.holdDistance != shipMoveOnAltActivate.defaultDistance)
                    {
                        instance.StartCoroutine(MoveShipItem(shipMoveOnAltActivate, shipMoveOnAltActivate.defaultDistance));
                        return;
                    }
                    instance.StartCoroutine(MoveShipItem(shipMoveOnAltActivate, shipMoveOnAltActivate.targetDistance));
                }
            }

            public static IEnumerator RotateShipItem(ShipItemRotateOnAltActivate shipRotateOnAltActivate, float target)
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

            public static IEnumerator MoveShipItem(ShipItemMoveOnAltActivate shipMoveOnAltActivate, float target)
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

        public static void ResetMove(ShipItem instance)
        {
            ShipItemMoveOnAltActivate shipMoveOnAltActivate = instance.GetComponent<ShipItemMoveOnAltActivate>();
            if (shipMoveOnAltActivate)
            {
                instance.holdDistance = shipMoveOnAltActivate.defaultDistance;
            }
        }
    }
}
