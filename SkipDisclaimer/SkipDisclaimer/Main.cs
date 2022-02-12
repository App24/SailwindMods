using HarmonyLib;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace SkipDisclaimer
{
    internal static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;

            settings = SkipDisclaimerSettings.Load<SkipDisclaimerSettings>(mod);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("New Game Animation Speed (0-7) ", GUILayout.ExpandWidth(false));
            settings.AnimationSpeed = GUILayout.HorizontalSlider(settings.AnimationSpeed, 0, 7);
            GUILayout.Label(settings.AnimationSpeed.ToString("0.00"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Animation Speed", GUILayout.ExpandWidth(false)))
            {
                settings.AnimationSpeed = 7f;
            }
            GUILayout.EndHorizontal();
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static SkipDisclaimerSettings settings;
    }
}