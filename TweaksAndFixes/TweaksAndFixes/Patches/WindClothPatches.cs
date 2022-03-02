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
    internal static class WindClothPatches
    {
        [HarmonyPatch(typeof(WindCloth), "Update")]
        public static class UpdatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Cloth ___cloth)
            {
                if (Main.enabled)
                {
                    if (Utils.GamePaused) return false;
                    if (GameInput.GetKeyDown(InputName.CameraMode))
                    {
                        return false;
                    }
                    if (GameInput.GetKeyDown(InputName.Hint))
                    {
                        ___cloth.enabled = !___cloth.enabled;
                    }
                }
                return true;
            }
        }
    }
}
