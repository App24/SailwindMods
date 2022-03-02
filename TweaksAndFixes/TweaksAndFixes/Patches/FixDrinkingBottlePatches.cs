using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Patches
{
    public static class FixDrinkingBottlePatches
    {
        [HarmonyPatch(typeof(BottleDrinking), "OnTriggerEnter")]
        public static class OnTriggerEnterPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Collider other, ShipItemBottle ___bottle)
            {
                if (Main.enabled)
                {
                    if (other.GetComponent<MouthCol>() && ___bottle.sold && ___bottle.held)
                    {
                        ___bottle.TryDrinkBottle();
                    }
                }
                return !Main.enabled;
            }
        }
    }
}
