using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class MouseButtonPointerPatches
    {
        [HarmonyPatch(typeof(MouseButtonPointer), "LateUpdate")]
        private static class LateUpdatePatches
        {
            [HarmonyPostfix]
            public static void Postfix(MouseButtonPointer __instance, GoPointerButton ___pointedAtButton, RaycastHit ___hit)
            {
                if (Input.GetMouseButtonDown(1) && ___pointedAtButton)
                {
                    ___pointedAtButton.OnAltActivate();
                    if (__instance.linkedPointer)
                    {
                        ___pointedAtButton.OnAltActivate(__instance.linkedPointer);
                    }
                }
            }
        }
    }
}
