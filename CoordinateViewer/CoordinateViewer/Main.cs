using CoordinateViewer.Scripts;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace CoordinateViewer
{
    public static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            mod = modEntry;

            settings=UnityModManager.ModSettings.Load<CoordinateViewerSettings>(modEntry);
            settings.DecimalPrecision = Mathf.RoundToInt(settings.DecimalPrecision);
            settings.DecimalPrecision = Mathf.Max(1, settings.DecimalPrecision);

            settings.RecordTimer = Mathf.Max(1, settings.RecordTimer);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Decimal Precision", GUILayout.ExpandWidth(false));
            settings.DecimalPrecision = GUILayout.HorizontalSlider(Mathf.RoundToInt(settings.DecimalPrecision), 1, 5);
            GUILayout.Label($"{Mathf.RoundToInt(settings.DecimalPrecision)}", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Seconds between every recording", GUILayout.ExpandWidth(false));
            settings.RecordTimer = GUILayout.HorizontalSlider(Mathf.RoundToInt(settings.RecordTimer), 1, 60);
            GUILayout.Label($"{Mathf.RoundToInt(settings.RecordTimer)}", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            if (!enabled)
            {
                CoordinateNotificationUI.instance.UI.SetActive(false);
                CoordinateRecording.StopRecording();
            }
            return true;
        }

        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static CoordinateViewerSettings settings;
    }
}
