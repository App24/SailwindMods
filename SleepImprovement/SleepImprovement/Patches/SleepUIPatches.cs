using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SleepImprovement.Patches
{
    public static class SleepUIPatches
    {

        [HarmonyPatch(typeof(SleepUI), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(SleepUI __instance)
            {
                if (!GameState.sleeping && Main.enabled)
                {
                    AccessTools.Method(typeof(SleepUI), "SetOutlineAndSounds").Invoke(__instance, new object[] { true });
                }
                return true;
            }
        }
    }
}
