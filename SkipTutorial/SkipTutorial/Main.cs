using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace SkipTutorial
{
    internal static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;

            settings = SkipTutorialSettings.Load<SkipTutorialSettings>(mod);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Delete Navigation Scroll", GUILayout.ExpandWidth(false));
            settings.DeleteScroll = GUILayout.Toggle(settings.DeleteScroll, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static SkipTutorialSettings settings;
    }
}