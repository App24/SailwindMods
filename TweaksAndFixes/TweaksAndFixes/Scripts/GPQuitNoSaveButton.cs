using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Scripts
{
    internal class GPQuitNoSaveButton : GoPointerButton
    {
        public StartMenu startMenu;

        public override void OnActivate()
        {
            Application.Quit();
        }
    }
}
