using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityModManagerNet.UnityModManager;

namespace TweaksAndFixes.Patches
{
    internal static class UseMissionGoodsPatches
    {
        [HarmonyPatch(typeof(ShipItemCrate), "OnAltActivate")]
        private static class UseMissionGoodsPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItemCrate __instance, GoPointer activatingPointer)
            {
                if (Main.enabled)
                {
                    Good good = __instance.GetPrivateField<Good>("goodC");
                    if (good.GetMissionIndex() > -1)
                    {
                        Mission mission = PlayerMissions.missions[good.GetMissionIndex()];
                        if ((int)mission.InvokePrivateMethod("GetDeliveryPrice") > 0)
                        {
                            mission.totalPrice = Mathf.RoundToInt(mission.totalPrice * 0.8f);
                            SpawnContainedPrefab(__instance, activatingPointer);
                            __instance.UpdateLookText();
                        }
                    }
                }
            }

            static void SpawnContainedPrefab(ShipItemCrate instance, GoPointer activatingPointer)
            {
                GameObject gameObject = GameObject.Instantiate(instance.GetContainedPrefab(), instance.transform.position + new Vector3(0, 100.5f, 0), instance.transform.rotation);
                gameObject.GetComponent<SaveablePrefab>().RegisterToSave();
                if(instance.smokedFood && gameObject.GetComponent<CookableFood>())
                {
                    gameObject.GetComponent<ShipItem>().amount = 1.01f;
                    gameObject.GetComponent<CookableFood>().UpdateMaterial();
                }
                instance.itemRigidbodyC.UpdateMass();
                instance.StartCoroutine((System.Collections.IEnumerator)instance.InvokePrivateMethod("PickUpSpawnedItem", gameObject, activatingPointer));
            }
        }

        [HarmonyPatch(typeof(ShipItemBottle), "OnItemClick")]
        private static class OnItemClickPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItemBottle __instance, PickupableItem heldItem)
            {
                if (Main.enabled)
                {
                    ShipItemBottle component = heldItem.GetComponent<ShipItemBottle>();
                    if (component)
                    {
                        Good good = __instance.GetPrivateField<Good>("goodC");
                        Good componentGood = component.GetPrivateField<Good>("goodC");
                        if (good && good.GetMissionIndex() > -1)
                        {
                            Mission mission = PlayerMissions.missions[good.GetMissionIndex()];
                            if ((int)mission.InvokePrivateMethod("GetDeliveryPrice") > 0)
                            {
                                if (component.GetCapacity() <= __instance.GetCapacity())
                                {
                                    component.FillBottle(__instance.amount, __instance.health);
                                    mission.totalPrice = Mathf.RoundToInt(mission.totalPrice * 0.8f);
                                    __instance.UpdateLookText();
                                    __instance.itemRigidbodyC.UpdateMass();
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ShipItem), "UpdateLookText")]
        private static class ShipItemUpdateLookTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItem __instance)
            {
                if (!Main.enabled) return true;
                Good good = __instance.GetPrivateField<Good>("good");
                if (good)
                {
                    Mission mission = PlayerMissions.missions[good.GetMissionIndex()];
                    __instance.lookText = $@"{__instance.name}
to {mission.destinationPort.GetPortName()}
due: {mission.GetDueText()}
reward: {mission.InvokePrivateMethod("GetDeliveryPrice")}";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemCrate), "UpdateLookText")]
        private static class ShipItemCrateUpdateLookTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemCrate __instance)
            {
                if (!Main.enabled) return true;
                if (!__instance.sold) return true;
                Good good = __instance.GetPrivateField<Good>("goodC");
                if (good && good.GetMissionIndex() > -1)
                {
                    Mission mission = PlayerMissions.missions[good.GetMissionIndex()];
                    __instance.lookText = $@"{__instance.name}
to {mission.destinationPort.GetPortName()}
due: {mission.GetDueText()}
reward: {mission.InvokePrivateMethod("GetDeliveryPrice")}";
                    return false;
                }
                return true;
            }
        }
    }
}
