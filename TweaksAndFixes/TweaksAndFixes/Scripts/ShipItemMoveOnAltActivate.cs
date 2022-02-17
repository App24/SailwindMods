using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Scripts
{
    internal class ShipItemMoveOnAltActivate : MonoBehaviour
    {
        public bool moving;

        public ShipItem shipItem;

        public float targetDistance = 2f;

        public float defaultDistance;

        void Awake()
        {
            shipItem = GetComponent<ShipItem>();
            defaultDistance = shipItem.holdDistance;
        }
    }
}
