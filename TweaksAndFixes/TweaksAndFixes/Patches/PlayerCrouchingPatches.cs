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
    internal static class PlayerCrouchingPatches
    {
        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                if (!Main.enabled) return true;
                return !Utils.GamePaused;
            }

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
