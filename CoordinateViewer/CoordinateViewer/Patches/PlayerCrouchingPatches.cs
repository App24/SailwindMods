using CoordinateViewer.Scripts;
using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Patches
{
    public static class PlayerCrouchingPatches
    {
        [HarmonyPatch(typeof(PlayerCrouching), "Update")]
        public static class UpdatePatch
        {
            static float delay;
            static bool released = true;

            [HarmonyPostfix]
            public static void Postfix(PlayerCrouching __instance)
            {
                if (!GameState.playing || !Main.enabled || Utils.GamePaused)
                {
                    return;
                }

                if (Input.GetKey(KeyCode.N) && released)
                {
                    delay += Time.deltaTime;
                }

                if(!Input.GetKey(KeyCode.N) && released)
                {
                    delay = 0;
                }

                if (Input.GetKeyUp(KeyCode.N) && !released)
                {
                    released = true;
                }

                if(delay >= 2 && released)
                {
                    delay = 0;
                    released = false;
                    CoordinateRecording.ToggleRecording();
                }

                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    CoordinateNotificationUI.instance.Toggle();
                }

                CoordinateNotificationUI.instance.ShowCoords();
                CoordinateRecording.RecordCoords();
            }
        }
    }
}
