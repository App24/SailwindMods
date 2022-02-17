using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Patches;
using UnityEngine;

namespace TweaksAndFixes.Scripts
{
    internal class MissionSortButton : GoPointerButton
    {
        public TextMesh text;

        public static Port currentPort;

        public override void OnActivate()
        {
            PortPatches.missionSorting++;
            if (PortPatches.missionSorting >= MissionSorting.Last)
            {
                PortPatches.missionSorting = 0;
            }
            if (currentPort != null)
            {
                PortPatches.GenerateMissionsPatch.SortMissions(currentPort);
                MissionListUI.instance.ChangePage(-(int)Traverse.Create(MissionListUI.instance).Field("currentPage").GetValue());
                ((GameObject)Traverse.Create(MissionDetailsUI.instance).Field("UI").GetValue()).SetActive(false);
                Traverse.Create(MissionDetailsUI.instance).Field("currentMission").SetValue(null);
                AccessTools.Method(typeof(MissionDetailsUI), "ZoomMap").Invoke(MissionDetailsUI.instance, new object[] { false });
                AccessTools.Method(typeof(MissionListUI), "UpdatePageCountText").Invoke(MissionListUI.instance, new object[] { });
            }
            UpdateText();
        }

        public void UpdateText()
        {
            text.text = $"Sorting by: {PortPatches.missionSorting}";
        }
    }
}
