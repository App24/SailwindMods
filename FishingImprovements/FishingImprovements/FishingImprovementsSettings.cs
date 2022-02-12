using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace FishingImprovements
{
    public class FishingImprovementsSettings : UnityModManager.ModSettings
    {
        public float PickUpDistance = 0.1f;
        public float ScrollSensitivity = 0.75f;

        [XmlIgnore]
        public bool DestroyHook = true;

        [XmlIgnore]
        public bool FishCanEscape = true;

        [XmlIgnore]
        public bool InstantFishCatch = false;

        [XmlIgnore]
        public bool InstantScroll = false;


        [XmlElement("DestroyHook")]
        public string DestroyHookSerialize
        {
            get
            {
                return DestroyHook ? "1" : "0";
            }
            set
            {
                DestroyHook = XmlConvert.ToBoolean(value);
            }
        }

        [XmlElement("FishCanEscape")]
        public string FishCanEscapeSerialize
        {
            get
            {
                return FishCanEscape ? "1" : "0";
            }
            set
            {
                FishCanEscape = XmlConvert.ToBoolean(value);
            }
        }

        [XmlElement("InstantFishCatch")]
        public string InstantFishCatchSerialize
        {
            get
            {
                return InstantFishCatch ? "1" : "0";
            }
            set
            {
                InstantFishCatch = XmlConvert.ToBoolean(value);
            }
        }

        [XmlElement("InstantScroll")]
        public string InstantScrollSerialize
        {
            get
            {
                return InstantScroll ? "1" : "0";
            }
            set
            {
                InstantScroll = XmlConvert.ToBoolean(value);
            }
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
