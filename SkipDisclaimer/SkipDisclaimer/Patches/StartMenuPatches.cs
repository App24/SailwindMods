using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SailwindModdingHelper;

namespace SkipDisclaimer.Patches
{
    public static class StartMenuPatches
    {

        [HarmonyPatch(typeof(StartMenu), "StartNewGame")]
        private static class SkipDisclaimerNewGamePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(StartMenu __instance, ref bool ___fPressed, ref int ___animsPlaying, int ___currentRegion, Transform ___startApos, Transform ___startEpos, Transform ___startMpos)
            {
                if (Main.enabled)
                {
                    ___fPressed = true;
                    ___animsPlaying++;
                    Transform transform = null;
                    if (___currentRegion == 0)
                    {
                        transform = ___startApos;
                        GameState.newGameRegion = PortRegion.alankh;
                    }
                    else if (___currentRegion == 1)
                    {
                        transform = ___startEpos;
                        GameState.newGameRegion = PortRegion.emerald;
                    }
                    else
                    {
                        transform = ___startMpos;
                        GameState.newGameRegion = PortRegion.medi;
                    }

                    __instance.InvokePrivateMethod("DisableIslandMenu");
                    __instance.StartCoroutine(MovePlayerToStartPos(__instance, transform));
                    return false;
                }
                return true;
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

        /*[HarmonyPatch(typeof(StartMenu), "MovePlayerToStartPos")]
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
        }*/

        public static IEnumerator MovePlayerToStartPos(StartMenu instance, Transform startPos)
        {
            instance.GetPrivateField<GameObject>("logo").SetActive(false);
            instance.GetPrivateField<Transform>("playerObserver").transform.parent = instance.transform.parent;
            float animTime = Main.settings.AnimationSpeed;
            Juicebox.juice.TweenPosition(instance.GetPrivateField<Transform>("playerObserver").gameObject, startPos.position, animTime, JuiceboxTween.quadraticInOut);
            for (float t = 0f; t < animTime; t += Time.deltaTime)
            {
                instance.GetPrivateField<Transform>("playerObserver").rotation = Quaternion.Lerp(instance.GetPrivateField<Transform>("playerObserver").rotation, startPos.rotation, Time.deltaTime * 0.35f);
                yield return new WaitForEndOfFrame();
            }
            instance.GetPrivateField<Transform>("playerObserver").rotation = startPos.rotation;
            instance.GetPrivateField<GameObject>("playerController").transform.position = instance.GetPrivateField<Transform>("playerObserver").position;
            instance.GetPrivateField<GameObject>("playerController").transform.rotation = instance.GetPrivateField<Transform>("playerObserver").rotation;
            yield return new WaitForEndOfFrame();
            instance.GetPrivateField<GameObject>("playerController").GetComponent<CharacterController>().enabled = true;
            instance.GetPrivateField<GameObject>("playerController").GetComponent<OVRPlayerController>().enabled = true;
            instance.GetPrivateField<Transform>("playerObserver").gameObject.GetComponent<PlayerControllerMirror>().enabled = true;
            MouseLook.ToggleMouseLookAndCursor(true);
            instance.GetPrivateField<PurchasableBoat[]>("startingBoats")[instance.GetPrivateField<int>("currentRegion")].LoadAsPurchased();
            instance.StartCoroutine(Blackout.FadeTo(1f, 0.2f));
            yield return new WaitForSeconds(0.2f);
            yield return new WaitForEndOfFrame();
            instance.GetPrivateField<GameObject>("disclaimer").SetActive(true);
            instance.SetPrivateField("waitingForFInput", true);
            while (!instance.GetPrivateField<bool>("fPressed"))
            {
                yield return new WaitForEndOfFrame();
            }
            instance.GetPrivateField<GameObject>("disclaimer").SetActive(false);
            instance.StartCoroutine(Blackout.FadeTo(0f, 0.3f));
            yield return new WaitForEndOfFrame();
            SaveLoadManager.readyToSave = true;
            GameState.playing = true;
            GameState.justStarted = true;
            MouseLook.ToggleMouseLook(true);
            int animsPlaying = (int)Traverse.Create(instance).Field("animsPlaying").GetValue();
            Traverse.Create(instance).Field("animsPlaying").SetValue(animsPlaying - 1);
            yield return new WaitForSeconds(1f);
            GameState.justStarted = false;
            yield break;
        }
    }
}
