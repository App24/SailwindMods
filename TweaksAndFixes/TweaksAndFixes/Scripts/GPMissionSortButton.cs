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
    internal class GPMissionSortButton : GoPointerButton
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
            UpdateMissions();
            UpdateText();
        }

        public override void OnAltActivate()
        {
            PortPatches.missionSorting--;
            if (PortPatches.missionSorting < 0)
            {
                PortPatches.missionSorting = MissionSorting.Last-1;
            }
            UpdateMissions();
            UpdateText();
        }

        private void UpdateMissions()
        {
            if (currentPort != null)
            {
                PortPatches.GenerateMissionsPatch.SortMissions(currentPort);
                MissionListUI.instance.ChangePage(-(int)Traverse.Create(MissionListUI.instance).Field("currentPage").GetValue());
                MissionDetailsUI.instance.GetPrivateField<GameObject>("UI").SetActive(false);
                MissionDetailsUI.instance.SetPrivateField("currentMission", null);
                MissionDetailsUI.instance.InvokePrivateMethod("ZoomMap", false);
                MissionDetailsUI.instance.InvokePrivateMethod("UpdatePageCountText");
            }
        }

        public void UpdateText()
        {
            text.text = $"Sorting by: {PortPatches.missionSorting}";
        }
    }
}
