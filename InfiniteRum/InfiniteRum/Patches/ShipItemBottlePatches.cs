using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRum.Patches
{
    public static class ShipItemBottlePatches
    {

        public static List<string> alcoholNames = new List<string>()
        {
            "rum",
            "wine",
            "coconut wine",
            "honey beer",
            "rice beer"
        };

        [HarmonyPatch(typeof(ShipItemBottle), "OnItemClick")]
        private static class OnItemClickPatch
        {
            static float health;
            [HarmonyPrefix]
            public static void Prefix(ShipItemBottle __instance)
            {
                if (Main.enabled && alcoholNames.Contains(Liquids.GetLiquidName(__instance.amount).ToLower()))
                {
                    health = __instance.health;
                }
            }

            [HarmonyPostfix]
            public static void Postfix(ShipItemBottle __instance)
            {
                if (Main.enabled && alcoholNames.Contains(Liquids.GetLiquidName(__instance.amount).ToLower()))
                {
                    __instance.health = health;
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemBottle), "Drink")]
        private static class DrinkPatch
        {
            static float health;
            [HarmonyPrefix]
            public static void Prefix(ShipItemBottle __instance, ref float ___capacity)
            {
                if (Main.enabled && ___capacity >= 5 && alcoholNames.Contains(Liquids.GetLiquidName(__instance.amount).ToLower()))
                {
                    health = __instance.health;
                }
            }

            [HarmonyPostfix]
            public static void Postfix(ShipItemBottle __instance, ref float ___capacity)
            {
                if (Main.enabled && ___capacity >= 5 && alcoholNames.Contains(Liquids.GetLiquidName(__instance.amount).ToLower()))
                {
                    __instance.health = health;
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemBottle), "UpdateLookText")]
        private static class UpdateLookTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemBottle __instance, ref float ___capacity, ref Good ___goodC)
            {
                if (Main.enabled && ___capacity >= 5 && alcoholNames.Contains(Liquids.GetLiquidName(__instance.amount).ToLower()))
                {
                    if (!__instance.sold || (___goodC != null && ___goodC.GetMissionIndex() > -1) || __instance.amount == 0f)
                    {
                        return true;
                    }

                    __instance.lookText = "";
                    string text = "bottle";
                    text = ((___capacity < 5f) ? "mug" : ((!(___capacity < 30f)) ? "barrel" : "bottle"));

                    string liquidName = Liquids.GetLiquidName(__instance.amount);
                    __instance.description = text + " of " + liquidName + "\n∞ / ∞";
                    return false;
                }
                return true;
            }
        }
    }
}
