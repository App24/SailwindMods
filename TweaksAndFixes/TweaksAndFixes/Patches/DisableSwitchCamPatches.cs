using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class DisableSwitchCamPatches
    {
        [HarmonyPatch(typeof(BoatCamera), "Update")]
        private static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(BoatCamera __instance)
            {
                if (Main.enabled)
                {
                    if (Utils.GamePaused) return false;
                    if (GameInput.GetKeyDown(InputName.CameraMode))
                    {
                        if (GameState.currentBoat == null)
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
