using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SkipDisclaimer.Patches
{
    public static class StartMenuPatches
    {

        [HarmonyPatch(typeof(StartMenu), "StartNewGame")]
        private static class SkipDisclaimerNewGamePatch
        {
            [HarmonyPrefix]
            public static void Prefix(StartMenu __instance, ref bool ___fPressed)
            {
                if (Main.enabled)
                {
                    ___fPressed = true;
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "LoadGame")]
        private static class SkipDisclaimerLoadGamePatch
        {
            [HarmonyPrefix]
            public static void Prefix(StartMenu __instance, ref bool ___fPressed)
            {
                if (Main.enabled)
                {
                    ___fPressed = true;
                }
            }
        }

        [HarmonyPatch(typeof(StartMenu), "MovePlayerToStartPos")]
        public static class QuickStartPosPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(StartMenu __instance, Transform startPos, ref GameObject ___logo, ref Transform ___playerObserver, ref GameObject ___playerController, ref PurchasableBoat[] ___startingBoats, ref int ___currentRegion, ref GameObject ___disclaimer, ref bool ___waitingForFInput, ref bool ___fPressed)
            {
                if (Main.enabled)
                {
                    __instance.StartCoroutine(MovePlayerToStartPos(__instance, startPos, ___logo, ___playerObserver, ___playerController, ___startingBoats, ___currentRegion, ___disclaimer, ___fPressed));
                    return false;
                }
                return true;
            }

            public static IEnumerator MovePlayerToStartPos(StartMenu instance, Transform startPos, GameObject logo, Transform playerObserver, GameObject playerController, PurchasableBoat[] startingBoats, int currentRegion, GameObject disclaimer, bool fPressed)
            {
                logo.SetActive(false);
                playerObserver.transform.parent = instance.transform.parent;
                float animTime = Main.settings.AnimationSpeed;
                Juicebox.juice.TweenPosition(playerObserver.gameObject, startPos.position, animTime, JuiceboxTween.quadraticInOut);
                for (float t = 0f; t < animTime; t += Time.deltaTime)
                {
                    playerObserver.rotation = Quaternion.Lerp(playerObserver.rotation, startPos.rotation, Time.deltaTime * 0.35f);
                    yield return new WaitForEndOfFrame();
                }
                playerObserver.rotation = startPos.rotation;
                playerController.transform.position = playerObserver.position;
                playerController.transform.rotation = playerObserver.rotation;
                yield return new WaitForEndOfFrame();
                playerController.GetComponent<CharacterController>().enabled = true;
                playerController.GetComponent<OVRPlayerController>().enabled = true;
                playerObserver.gameObject.GetComponent<PlayerControllerMirror>().enabled = true;
                MouseLook.ToggleMouseLookAndCursor(true);
                startingBoats[currentRegion].LoadAsPurchased();
                instance.StartCoroutine(Blackout.FadeTo(1f, 0.2f));
                yield return new WaitForSeconds(0.2f);
                yield return new WaitForEndOfFrame();
                disclaimer.SetActive(true);
                Traverse.Create(instance).Field("waitingForFInput").SetValue(true);
                while (!fPressed)
                {
                    yield return new WaitForEndOfFrame();
                }
                disclaimer.SetActive(false);
                instance.StartCoroutine(Blackout.FadeTo(0f, 0.3f));
                yield return new WaitForEndOfFrame();
                SaveLoadManager.readyToSave = true;
                GameState.playing = true;
                GameState.justStarted = true;
                MouseLook.ToggleMouseLook(true);
                int animsPlaying= (int)Traverse.Create(instance).Field("animsPlaying").GetValue();
                Traverse.Create(instance).Field("animsPlaying").SetValue(animsPlaying-1);
                yield return new WaitForSeconds(1f);
                GameState.justStarted = false;
                yield break;
            }
        }
    }
}
