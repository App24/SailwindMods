using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweaksAndFixes.Scripts;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    public static class SaveablePrefabPatches
    {
		[HarmonyPatch(typeof(SaveablePrefab), "Load")]
		public static class LoadPatch
		{
			public static void Postfix(SaveablePrefab __instance, ShipItem ___item, SavePrefabData data)
			{
				indexCounter++;
				saveablePrefabs.Add(indexCounter, __instance);
			}

			public static void AssignHookItems()
            {
				Main.saveContainer.HookItems.ForEach(h =>
				{
					if(saveablePrefabs.TryGetValue(h.prefabIndex, out var prefab))
                    {
						if(saveablePrefabs.TryGetValue(h.hookItemIndex, out var hookItem))
                        {
							prefab.GetComponent<ShipItemLampHookItemHooked>().hookedItem = hookItem.GetComponent<PickupableItem>();
                        }
                    }
				});
            }

			public static Dictionary<int, SaveablePrefab> saveablePrefabs = new Dictionary<int, SaveablePrefab>();
			public static int indexCounter;
		}

		[HarmonyPatch(typeof(SaveablePrefab), "PrepareSaveData")]
		public static class PrepareSaveDataPatch
		{
			public static void Prefix(ShipItem ___item)
			{
				indexCounter++;
				ShipItemLampHookItemHooked component = ___item.GetComponent<ShipItemLampHookItemHooked>();
				if (component && component.hookedItem)
				{
					SaveablePrefab saveablePrefab = component.hookedItem.GetComponent<SaveablePrefab>();
					if (saveablePrefab && saveablePrefabs.TryGetValue(saveablePrefab, out int index))
						Main.saveContainer.HookItems.Add(new HookItemSaveable(indexCounter, index));
				}
			}

			public static Dictionary<SaveablePrefab, int> saveablePrefabs = new Dictionary<SaveablePrefab, int>();
			public static int indexCounter;
		}
	}
}
