using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweaksAndFixes
{
    public class TAFSaveContainer
    {
        public List<HookItemSaveable> HookItems { get; set; } = new List<HookItemSaveable>();
    }

    public class HookItemSaveable
    {
        public int prefabIndex;
        public int hookItemIndex;

        public HookItemSaveable(int prefabIndex, int hookItemIndex)
        {
            this.prefabIndex = prefabIndex;
            this.hookItemIndex = hookItemIndex;
        }

        public HookItemSaveable()
        {

        }
    }
}
