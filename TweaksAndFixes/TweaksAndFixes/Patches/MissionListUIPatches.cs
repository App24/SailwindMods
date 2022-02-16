using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace TweaksAndFixes.Patches
{
    internal static class MissionListUIPatches
    {
        // sometimes pages are empty, is that a game bug, or a mod bug?

        /*[HarmonyPatch(typeof(MissionListUI), "UpdatePageCountText")]
        public static class UpdatePageCountTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(MissionListUI __instance, int ___currentPageCount, int ___currentPage, TextMesh ___pageCountText)
            {
                if (Main.enabled)
                {
                    int num = ___currentPage + 1;
                    int num2 = ___currentPageCount;
                    ___pageCountText.text = num + " / " + num2;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(MissionListUI), "ChangePage")]
        public static class ChangePagePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(MissionListUI __instance, int pageChange, PortDude ___currentPortDude, ref int ___currentPage, int ___currentPageCount)
            {
                if (!Main.enabled) return true;
                if (___currentPortDude == null)
                {
                    return true;
                }
                ___currentPage += pageChange;
                if (___currentPage < 0)
                {
                    ___currentPage = 0;
                }
                else if (___currentPage >= ___currentPageCount)
                {
                    ___currentPage = ___currentPageCount - 1;
                }
                else
                {
                    UISoundPlayer.instance.PlayParchmentSound();
                    __instance.DisplayMissions(___currentPortDude.GetPort().GetMissions(___currentPage));
                }
                AccessTools.Method(typeof(MissionListUI), "UpdatePageCountText").Invoke(__instance, new object[] { });
                return false;
            }
        }*/
    }
}
