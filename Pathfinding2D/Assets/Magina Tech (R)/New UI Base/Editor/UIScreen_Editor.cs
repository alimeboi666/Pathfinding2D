using System.Collections;
using System.Collections.Generic;
using Hung.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIScreen))]
public class UIScreen_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ((UIScreen)target).OnDriven();   
    }
}

[CustomEditor(typeof(UI_TextFitter))]
public class UIContentFitter_Editor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ((UI_TextFitter)target).OnDriven();
    }
}
