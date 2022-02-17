﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using Log = UnityModManagerNet.UnityModManager.Logger;

namespace TweaksAndFixes.Patches
{
    internal static class MissionDetailsUIPatches
	{
		static TextMesh cargo;

		static Dictionary<string, string> cargoNames = new Dictionary<string, string>()
		{
			{"very_big_crate", "Very Big Crate" },
			{"big_crate", "Big Crate" },
			{"crate", "Small Crate" },
			{"barrel_closed", "Barrel" },
		};

		[HarmonyPatch(typeof(MissionDetailsUI), "Start")]
		public static class StartPatch
		{
			static int done;

			[HarmonyPrefix]
			public static void Prefix(MissionDetailsUI __instance, GameObject ___UI)
			{
				if (Main.enabled)
                {
					if (done>=3) return;
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
									done++;
                                }else if (item2.name.Contains("weight"))
                                {
									item2.localPosition = new Vector3(item2.localPosition.x, item2.localPosition.y- 0.093f, item2.localPosition.z);
									done++;
								}
								else if (item2.name.Contains("amount"))
                                {
									GameObject go = GameObject.Instantiate(item2.gameObject);
									go.transform.parent = item;
									go.transform.localPosition = new Vector3(0.5f, item2.localPosition.y, item2.localPosition.z);
									go.transform.localScale = item2.transform.localScale;
									go.name = item2.name.Replace("amount", "size");
									cargo=go.GetComponent<TextMesh>();
									item2.localPosition = new Vector3(item2.localPosition.x, item2.localPosition.y - 0.106f, item2.localPosition.z);
									done++;
								}
							}
                        }
						if (done>=3) break;
                    }
                }
			}
		}

		[HarmonyPatch(typeof(MissionDetailsUI), "ClickButton")]
		public static class ClickButtonPatch
		{
			[HarmonyPrefix]
			public static bool Prefix(MissionDetailsUI __instance, bool ___clickable, bool ___mapZoomedIn, Mission ___currentMission, GameObject ___UI)
			{
				if (!Main.enabled) return true;
				Log.Log(___currentMission.goodPrefab.GetComponent<MeshFilter>().sharedMesh.name);
				if (!___clickable)
				{
					return true;
				}
				if (___mapZoomedIn)
				{
					AccessTools.Method(typeof(MissionDetailsUI), "ZoomMap").Invoke(__instance, new object[] { false });
					return false;
				}
				if (___currentMission.missionIndex == -1)
				{
					PlayerMissions.AcceptMission(___currentMission);
					MissionListUI.instance.DisplayMissions(___currentMission.originPort.GetMissions((int)Traverse.Create(MissionListUI.instance).Field("currentPage").GetValue()));
					___UI.SetActive(false);
					AccessTools.Method(typeof(MissionDetailsUI), "UpdateTexts").Invoke(__instance, new object[] { });
					return false;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(MissionDetailsUI), "UpdateTexts")]
		public static class MissionDetailsUIPatch
        {
			[HarmonyPostfix]
			public static void Postfix(TextMesh ___due, Mission ___currentMission)
            {
				if (___currentMission != null)
				{
					string meshName = ___currentMission.goodPrefab.GetComponent<MeshFilter>().sharedMesh.name;
					if(!cargoNames.TryGetValue(meshName, out string cargoName))
                    {
						cargoName = meshName;
                    }
					cargo.text = cargoName;
				}
			}
        }

	}
}
