using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using Crest;

namespace FishingImprovements
{
    public class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            mod = modEntry;

            settings = FishingImprovementsSettings.Load<FishingImprovementsSettings>(modEntry);

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
            GUILayout.Label("Destroy Hook: ", GUILayout.ExpandWidth(false));
            settings.DestroyHook = GUILayout.Toggle(settings.DestroyHook, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Fish can escape: ", GUILayout.ExpandWidth(false));
            settings.FishCanEscape = GUILayout.Toggle(settings.FishCanEscape, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Catch fish instantly: ", GUILayout.ExpandWidth(false));
            settings.InstantFishCatch = GUILayout.Toggle(settings.InstantFishCatch, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("No cooldown when reeling in: ", GUILayout.ExpandWidth(false));
            settings.InstantScroll = GUILayout.Toggle(settings.InstantScroll, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Fish picking distance ", GUILayout.ExpandWidth(false));
            settings.PickUpDistance = GUILayout.HorizontalSlider(settings.PickUpDistance, 0.1f, 100f);
            GUILayout.Label(settings.PickUpDistance.ToString("0.00"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset picking distance", GUILayout.ExpandWidth(false)))
            {
                settings.PickUpDistance = 0.1f;
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Fishing rod scroll sensitivy ", GUILayout.ExpandWidth(false));
            settings.ScrollSensitivity = GUILayout.HorizontalSlider(settings.ScrollSensitivity, 0.1f, 2.5f);
            GUILayout.Label(settings.ScrollSensitivity.ToString("0.00"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset scroll sensitivy", GUILayout.ExpandWidth(false)))
            {
                settings.ScrollSensitivity = 0.75f;
            }
            GUILayout.EndHorizontal();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static FishingImprovementsSettings settings;
    }
}
