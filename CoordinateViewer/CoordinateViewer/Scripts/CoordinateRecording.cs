using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoordinateViewer.Scripts
{
    internal static class CoordinateRecording
    {
        static float recordDelay;
        static bool justStarted;
        static int currentDay = -1;
        static Vector2 lastPos;

        public static void StartRecording()
        {
            CoordinateRecorderUI.instance.UI.SetActive(true);
            justStarted = true;
        }

        public static void StopRecording()
        {
            CoordinateRecorderUI.instance.UI.SetActive(false);
            lastPos = Vector2.zero;
        }

        public static void ToggleRecording()
        {
            if (CoordinateRecorderUI.instance.UI.activeSelf)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }

        public static void RecordCoords()
        {
            if (CoordinateRecorderUI.instance.UI.activeSelf && !Utils.GamePaused)
            {
                recordDelay += Time.deltaTime / Time.timeScale;
                if (recordDelay >= Main.settings.RecordTimer || justStarted)
                {
                    justStarted = false;
                    recordDelay = 0;
                    if (currentDay != GameState.day)
                    {
                        currentDay = GameState.day;
                        File.AppendAllText(Path.Combine(Main.mod.Path, $"coords_{SaveSlots.currentSlot}.txt"), $"Day: {currentDay}\n");
                    }
                    Vector3 globePos = FloatingOriginManager.instance.GetGlobeCoords(Utils.PlayerTransform);
                    float lat = Mathf.Round(globePos.z * 100000f) / 100000f;
                    float longi = Mathf.Round(globePos.x * 100000f) / 100000f;
                    if (lastPos != new Vector2(longi, lat))
                    {
                        File.AppendAllText(Path.Combine(Main.mod.Path, $"coords_{SaveSlots.currentSlot}.txt"), $"{lat} {longi}\n");
                        lastPos = new Vector2(longi, lat);
                    }
                }
            }
        }
    }
}
