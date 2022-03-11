using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LowerGraphics.Scripts
{
    internal static class ShadowsController
    {
        public static void DisableShadows()
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }

        public static void EnableShadows()
        {
            QualitySettings.shadows = ShadowQuality.All;
        }
    }
}
