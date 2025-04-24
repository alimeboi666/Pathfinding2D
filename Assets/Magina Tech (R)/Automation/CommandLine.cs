using Hung.UI.Extensions;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Hung.Automation
{
    public record CommandLine
    {
        public string[] keys;
        public Action<string> executeAction;

        public CommandLine(Action<string> execute, params string[] keys)
        {
            this.keys = keys;
            this.executeAction = execute;
        }

        public void Execute(string value)
        {
            try
            {
                executeAction?.Invoke(value);
            }
            catch (Exception ex)
            {
                if (ex.Message == "") throw new Exception("Failed to execute command: " + keys);
                else
                {
                    throw new Exception("Failed to execute command: " + ex.Message);
                }
            }
        }
    }
    public abstract record CommandTable
    {
        public abstract void Init(ref List<CommandLine> lines);

        public bool AutoClose
        {
            get
            {
                return PlayerPrefs.GetInt("command_line-auto_close", 0) == 1;
            }

            set
            {
                PlayerPrefs.SetInt("command_line-auto_close", value ? 1 : 0);
            }
        }
    }
}