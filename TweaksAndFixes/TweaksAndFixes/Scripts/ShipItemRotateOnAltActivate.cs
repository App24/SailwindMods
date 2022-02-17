using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TweaksAndFixes.Scripts
{
    internal class ShipItemRotateOnAltActivate : MonoBehaviour
    {
        public bool rotating;

        public ShipItem shipItem;

        public float targetAngle = -22f;

        void Awake()
        {
            shipItem = GetComponent<ShipItem>();
        }
    }
}
