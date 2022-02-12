using System;
using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using UnityEngine;
using System.Collections.Generic;

namespace InfiniteRum
{
    internal static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static bool enabled;
        public static UnityModManager.ModEntry mod;
    }
}