using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    internal static class StoreFoodPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnItemClick")]
        private static class OnItemClickPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItem __instance, PickupableItem heldItem, ref bool __result)
            {
                if (!Main.enabled) return true;
                if(__instance is ShipItemCrate itemCrate)
                {
                    ShipItemFood foodItem = heldItem.GetComponent<ShipItemFood>();
                    if (foodItem)
                    {
                        Good foodGood = foodItem.GetPrivateField<Good>("good");
                        if ((foodGood && foodGood.GetMissionIndex() == -1) || !foodGood)
                        {
                            if (foodItem.name == itemCrate.name)
                            {
                                itemCrate.amount++;
                                foodItem.SetPrivateField("currentMesh", 0);
                                foodItem.health = 3;
                                foodItem.DestroyItem();
                                __result = false;
                                return false;
                            }
                            else if (itemCrate.amount <= 0)
                            {
                                GameObject prefab = Prefabs.GetPrefab(foodItem.name);
                                if (prefab)
                                {
                                    itemCrate.name = foodItem.name;
                                    itemCrate.amount++;
                                    foodItem.SetPrivateField("currentMesh", 0);
                                    foodItem.health = 3;
                                    if (foodGood)
                                    {
                                        GameObject.Destroy(itemCrate.gameObject.GetComponent<Good>());
                                        Good good = itemCrate.gameObject.AddComponent(foodGood);
                                        itemCrate.SetPrivateField("goodC", good);
                                    }
                                    itemCrate.SetPrivateField("containedPrefab", prefab);
                                    foodItem.DestroyItem();
                                    __result = false;
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemCrate), "UpdateLookText")]
        private static class UpdateLookTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemCrate __instance)
            {
                if (!Main.enabled) return true;
                if (!__instance.sold) return true;
                if (__instance.GetPrivateField<Good>("goodC") && __instance.GetPrivateField<Good>("goodC").GetMissionIndex() > -1) return true;
                if(__instance.amount <= 0)
                {
                    __instance.lookText = "empty";
                    return false;
                }
                return true;
            }
        }
    }
}
