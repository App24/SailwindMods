using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace CoordinateViewer
{
    public class CoordinateViewerSettings : UnityModManager.ModSettings
    {
        public float DecimalPrecision = 3;
        public float RecordTimer = 30;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
