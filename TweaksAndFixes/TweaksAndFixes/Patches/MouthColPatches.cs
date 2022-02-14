using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    public static class MouthColPatches
    {
        [HarmonyPatch(typeof(MouthCol), "OnTriggerEnter")]
        public static class OnTriggerEnterPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Collider other, ref ShipItemFood ___currentFood)
            {
                if (Main.enabled)
                {
                    ShipItemFood component = other.GetComponent<ShipItemFood>();
                    if (component && component.sold && component.held)
                    {
                        ___currentFood = component;
                    }
                }
                return !Main.enabled;
            }
        }
    }
}
