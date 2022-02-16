using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class MissionDetailsUIPatches
    {
        [HarmonyPatch(typeof(MissionDetailsUI), "ClickButton")]
        public static class ClickButtonPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(MissionDetailsUI __instance, bool ___clickable, bool ___mapZoomedIn, Mission ___currentMission, GameObject ___UI)
            {
                if (!Main.enabled) return true;
				if (!___clickable)
				{
					return true;
				}
				if (___mapZoomedIn)
				{
					AccessTools.Method(typeof(MissionDetailsUI), "ZoomMap").Invoke(__instance, new object[] { false });
					return false;
				}
				if (___currentMission.missionIndex == -1)
				{
					PlayerMissions.AcceptMission(___currentMission);
					MissionListUI.instance.DisplayMissions(___currentMission.originPort.GetMissions((int)Traverse.Create(MissionListUI.instance).Field("currentPage").GetValue()));
					___UI.SetActive(false);
					AccessTools.Method(typeof(MissionDetailsUI), "UpdateTexts").Invoke(__instance, new object[] { });
					return false;
				}
				return true;
			}
        }
    }
}
