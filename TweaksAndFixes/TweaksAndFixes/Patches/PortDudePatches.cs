using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Scripts;

namespace TweaksAndFixes.Patches
{
    internal static class PortDudePatches
    {
        [HarmonyPatch(typeof(PortDude), "ActivateMissionListUI")]
        public static class ActivateMissionListUIPatch
        {
            [HarmonyPostfix]
            public static void Postfix(PortDude __instance)
            {
                MissionSortButton.currentPort = __instance.GetPort();
            }
        }

        [HarmonyPatch(typeof(PortDude), "DeactivateMissionListUI")]
        public static class DeactivateMissionListUIPatch
        {
            [HarmonyPostfix]
            public static void Postfix(PortDude __instance)
            {
                MissionSortButton.currentPort = null;
            }
        }
    }
}
