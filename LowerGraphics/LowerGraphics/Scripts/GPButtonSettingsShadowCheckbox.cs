using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowerGraphics.Scripts
{
    internal class GPButtonSettingsShadowCheckbox : GPButtonSettingsCheckbo
    {
        public override void OnActivate()
        {
            base.OnActivate();
            if (on)
            {
                ShadowsController.EnableShadows();
            }
            else
            {
                ShadowsController.DisableShadows();
            }
        }
    }
}
