using System;
using System.Collections.Generic;
using Hung.Automation;
using Hung.Core;
using TMPro;
using UnityEngine;

namespace Hung.UI.Extensions
{
    public class UIScreen_CommandLine : UIScreen
    {
        [SerializeReference] private CommandTable ExecuteTable;

        [SerializeField] TMP_InputField inputField;
        [SerializeField] TMP_Text txtAnnounce;

        [SerializeField] GameObject lineAnnounce;

        /*[ReadOnly, SerializeField] */
        List<CommandLine> commandLines;

        /*[ReadOnly, ShowInInspector] */
        Dictionary<string, CommandLine> dict;

        public override void ToggleOn()
        {
            base.ToggleOn();
            Archetype.TimeControl.Pause();
            lineAnnounce.SetActive(false);
            inputField.text = "";
            inputField.Select();
        }

        public override void ToggleOff()
        {
            base.ToggleOff();
            Archetype.TimeControl.Resume();
        }

        private void Awake()
        {
            inputField.onValueChanged.AddListener((value) => lineAnnounce.SetActive(false));

            inputField.onSubmit.AddListener((value) => ExecuteCommand());

            ExecuteTable.Init(ref commandLines);
         
            ConvertListToDict();
        }

        void ConvertListToDict()
        {
            dict = new();
            foreach (var commandline in commandLines)
            {
                foreach (var key in commandline.keys)
                {
                    dict[key] = commandline;
                }
            }
        }

        (string, string) SplitCommand(string input)
        {
            int spaceIndex = input.IndexOf(' ');

            if (spaceIndex == -1)
            {
                return (input, string.Empty);
            }

            string firstPart = input.Substring(0, spaceIndex);
            string secondPart = input.Substring(spaceIndex + 1);

            return (firstPart, secondPart);
        }

        public void ExecuteCommand()
        {
            lineAnnounce.SetActive(true);
            try
            {
                var res = SplitCommand(inputField.text);

                if (!dict.ContainsKey(res.Item1))
                {
                    throw new Exception("Command not found: " + res.Item1);
                }

                dict[res.Item1].Execute(res.Item2);
                txtAnnounce.text = "Execute command successfully!";
                txtAnnounce.color = Color.yellow;
                if (ExecuteTable.AutoClose && isVisible) Archetype.Automation.ToggleCommandLine();
            }
            catch (Exception ex)
            {
                // Catch all exceptions and show the error message
                //Debug.LogError("Error executing command: " + ex.Message);

                txtAnnounce.text = ex.Message;
                txtAnnounce.color = Color.red;
            }
            //LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }   
}

