using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class StartMenuPatches
    {
        [HarmonyPatch(typeof(StartMenu), "EnableQuitConfirmMenu")]
        public static class EnableQuitConfirmMenuPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                SunPatches.quitWithoutSavingButton.SetActive(GameState.playing);
                SunPatches.quitButton.SetActive(false);
                SunPatches.quitButton.transform.localPosition = GameState.playing ? new Vector3(0.486f, -0.311f, 0.036f) : SunPatches.originalQuitPos;
                SunPatches.quitButton.SetActive(true);
            }
        }
    }
}
