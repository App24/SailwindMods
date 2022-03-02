using CoordinateViewer.Scripts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Patches
{
    internal static class SunPatches
    {
        [HarmonyPatch(typeof(Sun), "Start")]
        public static class StartPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Transform uiParent = GameObject.FindObjectOfType<NotificationUi>().transform.parent;
                SpawnCoordinateNotificationUI(uiParent);
                SpawnCoordinateRecorderUI(uiParent);
            }

            static void SpawnCoordinateRecorderUI(Transform uiParent)
            {
                GameObject notificationUIParent = GameObject.FindObjectOfType<NotificationUi>().gameObject;
                GameObject notificationUI = notificationUIParent.transform.GetChild(0).gameObject;

                GameObject placeholder = new GameObject();
                placeholder.transform.parent = uiParent;
                placeholder.transform.localPosition = notificationUIParent.transform.localPosition;
                placeholder.transform.localRotation = notificationUIParent.transform.localRotation;
                placeholder.transform.localScale = notificationUIParent.transform.localScale;

                GameObject ui = new GameObject();
                ui.transform.parent = placeholder.transform;
                ui.transform.localPosition = new Vector3(-1.7f, 3.3f, 1.042f);
                ui.transform.localRotation = Quaternion.identity;
                ui.transform.localScale = Vector3.one;
                placeholder.AddComponent<CoordinateRecorderUI>().UI = ui;

                GameObject textGameObject = new GameObject();
                textGameObject.transform.parent = ui.transform;
                textGameObject.transform.localPosition = new Vector3(-0.001f, 0f, -0.03f);
                textGameObject.transform.localRotation = Quaternion.Euler(0, -2.732076e-05f, 0);
                textGameObject.transform.localScale = new Vector3(0.01399471f, 0.01399469f, 0.01399471f);
                TextMesh text=textGameObject.AddComponent<TextMesh>();
                text.alignment = TextAlignment.Center;
                text.anchor = TextAnchor.MiddleCenter;
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.fontSize = 60;
                text.text = "Recording...";
            }

            static void SpawnCoordinateNotificationUI(Transform uiParent)
            {
                GameObject notificationUIParent = GameObject.FindObjectOfType<NotificationUi>().gameObject;
                GameObject notificationUI = notificationUIParent.transform.GetChild(0).gameObject;

                GameObject placeholder = new GameObject();
                placeholder.transform.parent = uiParent;
                placeholder.transform.localPosition = notificationUIParent.transform.localPosition;
                placeholder.transform.localRotation = notificationUIParent.transform.localRotation;
                placeholder.transform.localScale = notificationUIParent.transform.localScale;

                GameObject ui = GameObject.Instantiate(notificationUI);
                ui.transform.parent = placeholder.transform;
                ui.transform.localPosition = notificationUI.transform.localPosition;
                ui.transform.localRotation = notificationUI.transform.localRotation;
                ui.transform.localScale = Vector3.one;
                placeholder.AddComponent<CoordinateNotificationUI>().UI = ui;
            }
        }
    }
}
