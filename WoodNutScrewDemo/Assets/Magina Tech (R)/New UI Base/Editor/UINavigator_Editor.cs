using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UINavigator))]
public class UINavigator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ((UINavigator)target).OnDriven();
    }
}

[CustomEditor(typeof(UITabItem))]
public class UITabItem_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ((UITabItem)target).OnDriven();
    }
}

