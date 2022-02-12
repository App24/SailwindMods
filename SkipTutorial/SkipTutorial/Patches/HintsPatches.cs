using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipTutorial.Patches
{
    public static class HintsPatches
    {
        [HarmonyPatch(typeof(Hints), "GetAppropriateHint")]
        private static class GetAppropriateHintPatch
        {
            [HarmonyPrefix]
            public static void Prefix(Hints __instance, ref float ___smoothSailingTime)
            {
                if (Main.enabled)
                {
                    GameState.hasReadManual = true;
                    GameState.droppedManual = true;
                    ___smoothSailingTime = 12;
                }
            }

            [HarmonyPostfix]
            public static void Postfix(Hints __instance, ref object __result)
            {
                if (Main.enabled)
                {
                    __result = 8;
                }
            }
        }
    }
}
