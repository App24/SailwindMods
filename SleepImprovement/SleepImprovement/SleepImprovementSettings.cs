using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace SleepImprovement
{
    public class SleepImprovementSettings : UnityModManager.ModSettings
    {
        [XmlIgnore]
        public bool WakeUpAnytime = false;

        [XmlElement("WakeUpAnytime")]
        public string WakeUpAnytimeSerialize
        {
            get { return WakeUpAnytime ? "1" : "0"; }
            set { WakeUpAnytime = XmlConvert.ToBoolean(value); }
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
