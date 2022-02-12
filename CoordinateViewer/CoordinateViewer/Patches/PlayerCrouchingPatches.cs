using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Patches
{
    public static class PlayerCrouchingPatches
    {
        // Not sure exactly what controller the player uses, so I attached this script to the player crouching
        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerCrouching __instance)
            {
                if (!GameState.playing || !Main.enabled)
                {
                    return;
                }
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Vector3 globePos = FloatingOriginManager.instance.GetGlobeCoords(__instance.transform);
                    NotificationUi.instance.ShowNotification($"Longitude: {globePos.x:0.000}\nLatitude: {globePos.z:0.000}");
                }
            }
        }
    }
}
