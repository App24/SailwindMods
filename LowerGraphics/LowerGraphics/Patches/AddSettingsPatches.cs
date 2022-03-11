using HarmonyLib;
using LowerGraphics.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LowerGraphics.Patches
{
    internal static class AddSettingsPatches
    {
        static GameObject shadowToggle;

        [HarmonyPatch(typeof(Sun), "Start")]
        public static class AddSettingsPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Main.enabled)
                {
                    if (!PlayerPrefs.HasKey("enableShadows"))
                    {
                        PlayerPrefs.SetInt("enableShadows", 1);
                    }

                    if (PlayerPrefs.GetInt("enableShadows") <= 0)
                    {
                        ShadowsController.DisableShadows();
                    }

                    StartMenu startMenu = GameObject.FindObjectOfType<StartMenu>();
                    if (startMenu)
                    {
                        GameObject settingsUi = (GameObject)Traverse.Create(startMenu).Field("settingsUI").GetValue();
                        foreach (Transform child in settingsUi.transform)
                        {
                            if (child.name == "chekbox (3)")
                            {
                                shadowToggle = GameObject.Instantiate(child.gameObject);
                                shadowToggle.name = "shadows checkbox";
                                shadowToggle.transform.parent = settingsUi.transform;
                                shadowToggle.transform.localPosition = new Vector3(-0.022f, -0.32f, -0.002f);
                                shadowToggle.transform.localRotation = child.localRotation;
                                shadowToggle.transform.localScale = child.localScale;

                                GameObject.Destroy(shadowToggle.GetComponent<GPButtonSettingsCheckbo>());
                                GPButtonSettingsShadowCheckbox shadowButton = shadowToggle.AddComponent<GPButtonSettingsShadowCheckbox>();
                                shadowButton.setting = "enableShadows";
                                shadowButton.text = shadowToggle.GetComponentInChildren<TextMesh>();
                                shadowButton.Initialize();
                            }else if (child.name == "text")
                            {
                                TextMesh textMesh = child.GetComponent<TextMesh>();
                                textMesh.text += "\n\nenable Shadows";
                            }
                        }
                    }
                }
            }
        }
    }
}
