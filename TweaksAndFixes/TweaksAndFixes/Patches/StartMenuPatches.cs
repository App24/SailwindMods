using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes.Patches
{
    internal static class StartMenuPatches
    {
        [HarmonyPatch(typeof(StartMenu), "GameToSettings")]
        public static class GameToSettingsPatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                if (Main.enabled)
                {
                    Main.paused = true;
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "SettingsToGame")]
        public static class SettingsToGamePatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                if (Main.enabled)
                {
                    Main.paused = false;
                }
            }
        }

        /*[HarmonyPatch(typeof(StartMenu), "EnableSettingsMenu")]
        public static class EnableSettingsMenuPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                //SunPatches.buttonGameObject.SetActive(GameState.playing);
            }
        }*/
    }
}
