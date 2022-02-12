using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using System.Xml;

namespace SkipTutorial
{
    public class SkipTutorialSettings : UnityModManager.ModSettings
    {
        [XmlIgnore]
        public bool DeleteScroll = true;

        [XmlElement("DeleteScroll")]
        public string DeleteScrollSerialize
        {
            get { return DeleteScroll ? "1": "0"; }
            set { DeleteScroll = XmlConvert.ToBoolean(value); }
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
