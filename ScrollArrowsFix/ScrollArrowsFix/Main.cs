using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace ScrollArrowsFix
{
    public static class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;

            modEntry.OnToggle = OnToggle;

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled= value;
            return true;
        }

        public static bool enabled;
        public static UnityModManager.ModEntry mod;
    }
}