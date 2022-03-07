using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.MonoBehaviourScripts;
using UnityEngine;
using UnityModManagerNet;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace TweaksAndFixes.Patches
{
    internal static class MissionSortButtonPatches
    {
        private static TextMesh cargo;
        private static Dictionary<string, string> cargoNames = new Dictionary<string, string>()
        {
            {"very_big_crate", "Very Big Crate" },
            {"big_crate", "Big Crate" },
            {"crate", "Small Crate" },
            {"barrel_closed", "Barrel" },
        };

        public static void SortMissions(Port instance)
        {
            MissionStoring missionStoringcomponent = instance.gameObject.GetComponent<MissionStoring>();
            Mission[] missions = instance.GetPrivateField<Mission[]>("missions");
            for (int k = 0; k < missions.Length; k++)
            {
                if (k + missionStoringcomponent.page < missionStoringcomponent.missions.Count)
                {
                    missions[k] = missionStoringcomponent.missions[k + missionStoringcomponent.page];
                }
            }
            instance.SetPrivateField("missions", missions);
        }

        [HarmonyPatch(typeof(MissionDetailsUI), "Start")]
        private static class MissionDetailsUIStartPatch
        {
            private static bool done;

            [HarmonyPrefix]
            public static void Prefix(MissionDetailsUI __instance, GameObject ___UI)
            {
                if (Main.enabled)
                {
                    if (done) return;
                    foreach (Transform item in ___UI.transform)
                    {
                        foreach (Transform item2 in item)
                        {
                            TextMesh textMesh = item2.GetComponent<TextMesh>();
                            if (textMesh)
                            {
                                if (textMesh.text.Contains("Cargo"))
                                {
                                    List<string> texts = new List<string>(textMesh.text.Split('\n'));
                                    texts.Insert(1, "Size:");
                                    textMesh.text = string.Join("\n", texts);
                                }
                                else if (item2.name.Contains("weight"))
                                {
                                    item2.localPosition = new Vector3(item2.localPosition.x, item2.localPosition.y - 0.093f, item2.localPosition.z);
                                }
                                else if (item2.name.Contains("amount"))
                                {
                                    GameObject go = GameObject.Instantiate(item2.gameObject);
                                    go.transform.parent = item;
                                    go.transform.localPosition = new Vector3(0.5f, item2.localPosition.y, item2.localPosition.z);
                                    go.transform.localScale = item2.transform.localScale;
                                    go.name = item2.name.Replace("amount", "size");
                                    cargo = go.GetComponent<TextMesh>();
                                    item2.localPosition = new Vector3(item2.localPosition.x, item2.localPosition.y - 0.106f, item2.localPosition.z);
                                }
                            }
                        }
                    }
                    foreach (Transform item in ___UI.transform.parent)
                    {
                        if (item.name == "current mission buttons")
                        {
                            foreach (Transform item2 in item)
                            {
                                if (item2.name == "page buttons")
                                {
                                    GameObject backButtons = item2.Find("mission button (back)").gameObject;
                                    GameObject gameObject = GameObject.Instantiate(backButtons);
                                    gameObject.transform.parent = item2;
                                    gameObject.transform.localPosition = new Vector3(-0.654f, -0.336f, 0.022f);
                                    gameObject.transform.localRotation = backButtons.transform.localRotation;
                                    gameObject.transform.localScale = backButtons.transform.localScale;
                                    gameObject.name = "sort button";
                                    GameObject buttonGameobject = gameObject.GetComponentInChildren<GoPointerButton>().gameObject;
                                    GameObject.Destroy(buttonGameobject.GetComponent<GoPointerButton>());
                                    GPMissionSortButton button = buttonGameobject.AddComponent<GPMissionSortButton>();
                                    button.text = gameObject.GetComponentInChildren<TextMesh>();
                                    button.UpdateText();
                                }
                            }
                        }
                    }
                    done = true;
                }
            }
        }

        [HarmonyPatch(typeof(MissionDetailsUI), "ClickButton")]
        private static class ClickButtonPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(MissionDetailsUI __instance, bool ___clickable, bool ___mapZoomedIn, Mission ___currentMission, GameObject ___UI)
            {
                if (!Main.enabled) return true;
                if (!___clickable)
                {
                    return true;
                }
                if (___mapZoomedIn)
                {
                    __instance.InvokePrivateMethod("ZoomMap", false);
                    return false;
                }
                if (___currentMission.missionIndex == -1)
                {
                    PlayerMissions.AcceptMission(___currentMission);
                    MissionListUI.instance.DisplayMissions(___currentMission.originPort.GetMissions(MissionListUI.instance.GetPrivateField<int>("currentPage")));
                    ___UI.SetActive(false);
                    __instance.InvokePrivateMethod("UpdateTexts");
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(MissionDetailsUI), "UpdateTexts")]
        private static class MissionDetailsUIPatch
        {
            [HarmonyPostfix]
            public static void Postfix(TextMesh ___due, Mission ___currentMission)
            {
                if (___currentMission != null)
                {
                    string meshName = ___currentMission.goodPrefab.GetComponent<MeshFilter>().sharedMesh.name;
                    if (!cargoNames.TryGetValue(meshName, out string cargoName))
                    {
                        cargoName = meshName;
                    }
                    cargo.text = cargoName;
                }
            }
        }

        [HarmonyPatch(typeof(PortDude), "ActivateMissionListUI")]
        private static class ActivateMissionListUIPatch
        {
            [HarmonyPostfix]
            public static void Postfix(PortDude __instance)
            {
                GPMissionSortButton.currentPort = __instance.GetPort();
            }
        }

        [HarmonyPatch(typeof(PortDude), "DeactivateMissionListUI")]
        private static class DeactivateMissionListUIPatch
        {
            [HarmonyPostfix]
            public static void Postfix(PortDude __instance)
            {
                GPMissionSortButton.currentPort = null;
            }
        }

        [HarmonyPatch(typeof(Port), "Start")]
        private static class PortStartPatch
        {
            [HarmonyPostfix]
            public static void Postfix(Port __instance)
            {
                __instance.gameObject.AddComponent<MissionStoring>();
            }
        }

        [HarmonyPatch(typeof(Port), "GenerateMissions")]
        private static class GenerateMissionsPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Port __instance, int page, ref Mission[] ___missions, ref int ___currentMissionCount, ref Port[] ___destinationPorts, GameObject[] ___producedGoodPrefabs)
            {
                if (!Main.enabled) return true;
                ___missions = new Mission[5];
                ___currentMissionCount = 0;
                int num = page * ___missions.Length;
                List<Mission> list = new List<Mission>();
                foreach (Port port in ___destinationPorts)
                {
                    foreach (GameObject gameObject in ___producedGoodPrefabs)
                    {
                        int prefabIndex = gameObject.GetComponent<SaveablePrefab>().prefabIndex;
                        if (port.island.GetDemand(prefabIndex) > 0)
                        {
                            Good component = gameObject.GetComponent<Good>();
                            bool flag = component.nativeRegion != port.region || (port.hubPort && !__instance.hubPort);
                            if (component.requiredRepLevel > PlayerReputation.GetRepLevel(port.region))
                            {
                                flag = false;
                            }
                            if (Mission.GetDistance(__instance, port) > PlayerReputation.GetMaxDistance(__instance.region))
                            {
                                flag = false;
                            }
                            if (flag)
                            {
                                int demand = port.island.GetDemand(prefabIndex);

                                int totalPrice = __instance.InvokePrivateMethod<int>("GetTotalPrice", prefabIndex, port, demand);
                                int dueDay = __instance.InvokePrivateMethod<int>("GetDueDay", port, component);
                                Mission item = new Mission(__instance, port, gameObject, demand, totalPrice, 1f, 0, dueDay);
                                list.Add(item);
                            }
                        }
                    }
                }
                ___currentMissionCount = list.Count;
                MissionStoring missionStoringcomponent = __instance.gameObject.GetComponent<MissionStoring>();
                missionStoringcomponent.page = num;
                missionStoringcomponent.missions = list;
                list.Sort(SortMissions);
                MissionSortButtonPatches.SortMissions(__instance);
                return false;
            }

            private static int SortMissions(Mission s2, Mission s1)
            {
                switch (GPMissionSortButton.missionSorting)
                {
                    case MissionSorting.PricePerMile:
                        {
                            return s1.pricePerKm.CompareTo(s2.pricePerKm);
                        }
                    case MissionSorting.TotalPrice:
                        {
                            return s1.totalPrice.CompareTo(s2.totalPrice);
                        }
                    case MissionSorting.GoodCount:
                        {
                            return s1.goodCount.CompareTo(s2.goodCount);
                        }
                    case MissionSorting.Distance:
                        {
                            return s1.distance.CompareTo(s2.distance);
                        }
                    default:
                        {
                            return s1.pricePerKm.CompareTo(s2.pricePerKm);
                        }
                }
            }
        }
    }
}
