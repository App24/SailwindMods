using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Scripts
{
    internal class CoordinateRecorderUI : MonoBehaviour
    {
        public static CoordinateRecorderUI instance;
        public GameObject UI;
        TextMesh text;

        bool reverse;

        float timer;

        void Start()
        {
            instance = this;
            UI.SetActive(false);
            text = UI.GetComponentInChildren<TextMesh>();
        }

        void Update()
        {
            if (UI.activeSelf)
            {
                if (timer <= 0)
                {
                    timer = 0.5f;
                    reverse = !reverse;
                }

                if (!reverse)
                {
                    text.color = Color.Lerp(text.color, Color.red, (Time.deltaTime / Time.timeScale) * 20f);
                }
                else
                {
                    text.color = Color.Lerp(text.color, Color.white, (Time.deltaTime / Time.timeScale) * 20f);
                }
                timer -= Time.deltaTime / Time.timeScale;
            }
        }
    }
}
