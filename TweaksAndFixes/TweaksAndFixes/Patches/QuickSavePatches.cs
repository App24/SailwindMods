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
    internal static class QuickSavePatches
    {
        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        private static class UpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Input.GetKeyDown(KeyCode.F8) && GameState.playing && !Utils.GamePaused)
                {
                    if (SaveLoadManager.readyToSave)
                    {
                        SaveLoadManager.instance.SaveGame(true);
                        NotificationUi.instance.ShowNotification("Game Saved!");
                    }
                }
            }
        }
    }
}
