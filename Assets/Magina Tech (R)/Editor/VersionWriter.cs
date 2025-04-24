using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class VersionWriter : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        string buildDate = DateTime.Now.ToString("yyMMddHHmm");
        string version = PlayerSettings.bundleVersion;

        string buildInfo = $"v{version}.{buildDate}";

        Debug.Log(buildInfo);

        Archetype.Automation.SetVersionInfo(buildInfo);
    }
}
