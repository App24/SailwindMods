using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Scripts
{
    internal class CoordinateNotificationUI : MonoBehaviour
    {
        void Start()
        {
            instance = this;
            text = UI.GetComponentInChildren<TextMesh>();
            text.GetComponent<Renderer>().material.renderQueue = 3002;
        }

        public void ShowCoords()
        {
            if (!UI.activeSelf) return;
            Vector3 globePos = FloatingOriginManager.instance.GetGlobeCoords(Utils.PlayerTransform);
            text.text = $"Longitude: {globePos.x.ToString($"n{Main.settings.DecimalPrecision}")}\nLatitude: {globePos.z.ToString($"n{Main.settings.DecimalPrecision}")}";
        }

        public void Toggle()
        {
            UI.SetActive(!UI.activeSelf);
            UISoundPlayer.instance.PlayParchmentSound();
        }

        void Update()
        {
            if (UI.activeSelf)
            {
                UI.transform.localScale = Vector3.Lerp(UI.transform.localScale, Vector3.one, Time.deltaTime * 5f);
            }
            else
            {
                UI.transform.localScale = new Vector3(0f, 1f, 1f);
            }
        }

        public static CoordinateNotificationUI instance;

        public GameObject UI;
        private TextMesh text;
    }
}
