using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SleepImprovement.Patches
{
    public static class SleepPatches
    {
        [HarmonyPatch(typeof(Sleep), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(Sleep __instance, ref float ___currentBedDuration)
            {
                if (!Main.enabled) return;
                var anyButtonDown = AccessTools.Method(typeof(Sleep), "AnyButtonDown");
                if (GameState.inBed && (bool)anyButtonDown.Invoke(__instance, new object[] { }) && ___currentBedDuration > 1.01f)
                {
                    if (GameState.sleeping)
                    {
                        if (GameState.eyesFullyClosed)
                        {
                            if (Main.settings.WakeUpAnytime)
                            {
                                __instance.LeaveBed();
                            }
                        }
                        else
                        {
                            GameState.eyesFullyClosed = true;
                            __instance.LeaveBed();
                        }
                    }
                    else
                    {
                        __instance.LeaveBed();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Sleep), "StartSleepTimeWarp")]
        public static class StartSleepTimeWarpPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Sleep __instance, ref float ___sleepTimeStep, ref float ___sleepTimescale)
            {
                if (Main.enabled)
                {
                    __instance.StartCoroutine(StartSleepTimeWarp(___sleepTimeStep, ___sleepTimescale));
                    return false;
                }
                return true;
            }

            private static IEnumerator StartSleepTimeWarp(float sleepTimeStep, float sleepTimescale)
            {
                yield return new WaitForSeconds(3f);
                if (!GameState.sleeping)
                {
                    yield break;
                }
                Time.fixedDeltaTime = sleepTimeStep;
                Time.timeScale = sleepTimescale;
                GameState.eyesFullyClosed = true;
                yield break;
            }
        }
    }
}
