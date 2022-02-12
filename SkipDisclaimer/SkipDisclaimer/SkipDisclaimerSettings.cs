using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SkipDisclaimer
{
    public class SkipDisclaimerSettings : UnityModManager.ModSettings
    {
        public float AnimationSpeed = 7f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
