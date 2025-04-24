using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Hung.UI;
//using M1racle.House;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

namespace Hung.Automation
{

    public class Automation : MonoBehaviour, IAutomation, IGameplayStart
    {
        public ExecuteLayer ExecuteLayer => ExecuteLayer.Default;

        [SerializeField] UIScreen screenCommandLine;
        [SerializeField] GameObject fpsCounter;
        [SerializeField] GameObject debugConsole;
        [SerializeField] private TMP_Text txtVersionAngle;
        [SerializeField] private TMP_Text txtVersionCenter;

        public bool showFPS
        {
            get => fpsCounter.activeInHierarchy;
            set
            {
                fpsCounter.SetActive(value);
                PlayerPrefs.SetInt("show-fps", value ? 1 : 0);
            }
        }

        public bool showDebugger
        {
            get => debugConsole.activeInHierarchy;
            set
            {
                debugConsole.SetActive(value);
                PlayerPrefs.SetInt("show-debugger", value ? 1 : 0);
            }
        }


        private void Awake()
        {
            fpsCounter.SetActive(PlayerPrefs.GetInt("show-fps") == 1);
            debugConsole.SetActive(PlayerPrefs.GetInt("show-debugger") == 1);
            screenCommandLine.ToggleOff();

            txtVersionAngle.gameObject.SetActive(false);
            txtVersionCenter.gameObject.SetActive(true);
            var ver = Resources.Load<TextAsset>("build_info").text;
            txtVersionAngle.text = ver;
            txtVersionCenter.text = "Version: " + ver;            
        }


        public void OnGameplayStart()
        {
            txtVersionAngle.gameObject.SetActive(true);
            txtVersionCenter.gameObject.SetActive(false);
        }


        public void ToggleCommandLine()
        {
            if (!screenCommandLine.isVisible) screenCommandLine.VisualOn();
            else screenCommandLine.VisualOff();
        }

        [Button]
        public void SetVersionInfo(string info)
        {
#if UNITY_EDITOR
            string buildInfoPath = "Assets/Resources/build_info.txt";
            File.WriteAllText(buildInfoPath, info);
            AssetDatabase.Refresh();
#endif
        }
    }
}
