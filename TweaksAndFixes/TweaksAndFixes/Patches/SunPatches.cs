using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Scripts;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class SunPatches
    {
        /*public static GameObject buttonGameObject;

        [HarmonyPatch(typeof(Sun), "Start")]
        public static class StartPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                StartMenu startMenu = GameObject.FindObjectOfType<StartMenu>();
                if (startMenu)
                {
                    GameObject settingsUi = (GameObject)Traverse.Create(startMenu).Field("settingsUI").GetValue();
                    foreach (Transform child in settingsUi.transform)
                    {
                        if (child.name == "button quit")
                        {
                            buttonGameObject = GameObject.Instantiate(child.gameObject);
                            buttonGameObject.name = "button mod menu";
                            buttonGameObject.transform.parent = settingsUi.transform;
                            buttonGameObject.transform.localPosition = new Vector3(3.55f, 2.8f, 0.037f);
                            buttonGameObject.transform.localRotation = Quaternion.Euler(180, 1.366038e-05f,180);
                            buttonGameObject.transform.localScale = Vector3.one;
                            GameObject gameObject = buttonGameObject.GetComponentInChildren<StartMenuButton>().gameObject;
                            GameObject.Destroy(gameObject.GetComponent<StartMenuButton>());
                            gameObject.AddComponent<ModMenuButton>();
                            TextMesh textMesh= buttonGameObject.GetComponentInChildren<TextMesh>();
                            textMesh.text = "Mod Menu";
                        }
                    }
                }
            }
        }*/

        /*public class GoPointerPatches
        {
            [HarmonyPatch(typeof(GoPointer), "ThrowItemAfterDelay")]
            public static class GoPointerPatch
            {
                private static float force_multiplier = 100;

                [HarmonyPrefix]
                public static bool Prefix(GoPointer __instance, Rigidbody heldRigidbody, ref float force)
                {
                    force *= force_multiplier;
                    __instance.StartCoroutine(ThrowItemAfterDelay(__instance, heldRigidbody, force));
                    return false;
                }

                private static IEnumerator ThrowItemAfterDelay(GoPointer instance, Rigidbody heldRigidbody, float force)
                {
                    yield return new WaitForFixedUpdate();
                    heldRigidbody.AddForce(instance.transform.forward * instance.throwForce * force * heldRigidbody.mass);
                    yield break;
                }
            }
        }*/
    }
}
