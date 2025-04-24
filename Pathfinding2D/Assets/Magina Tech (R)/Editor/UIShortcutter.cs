using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.AI;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hung.Core;
using Hung.Testing;
using UnityEngine.UI;
using Hung.GameData;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
public static class GameCache
{
    static GameCache()
    {
        Debug.Log("GAME CACHING...");

        if (Archetype.GameFlow != null)
        {
            Archetype.GameFlow.OnCached();
            Type type = Archetype.GameFlow.GetType();
            PrefabUtility.RecordPrefabInstancePropertyModifications(Archetype.GameFlow.gameObject.GetComponent(type));
        }
        DataCollection.Instance.SerializeAll();
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        Debug.Log("...CACHED SUCCESSFULLY");
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("ON PLAY ENTERED");
            Archetype.GameFlow.OnCached();
            Type type = Archetype.GameFlow.GetType();
            PrefabUtility.RecordPrefabInstancePropertyModifications(Archetype.GameFlow.gameObject.GetComponent(type));

            //Test.Instance.OnCached();
            //PrefabUtility.RecordPrefabInstancePropertyModifications(Test.Instance);
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            
            
        }
    }


}

public class UIShortcutter: Editor
{
    [MenuItem("Jobs/Resolve and Play &s")]
    static public void ResolveAndPlay()
    {
        if (!EditorApplication.isPlaying)
        {
            //try
            //{
            

            //await Task.Delay(200);

            //}
            //catch
            //{

            //}
            EditorApplication.EnterPlaymode();
        }
        else
        {
            EditorApplication.ExitPlaymode();
        }      
    }

    static IModel modelHolder;
    [MenuItem("GameObject/Toggle Active &a")]
    static public void ToggleActive()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            modelHolder = obj.GetComponentInParent<IModel>();
            if (modelHolder != null)
            {
                if (modelHolder.Model == obj && modelHolder.gameObject != obj)
                {
                    if (!obj.activeSelf) obj.SetActive(true);
                    //Debug.Log("LuL"); 
                    return;
                }
            }

            if (obj.GetComponent<ISingletonRole>() != null)
            {
                //Debug.Log("Kekw"); 
                obj.SetActive(true);
                
            }
            else
            {
                Undo.RegisterCompleteObjectUndo(obj, "kekw");
                //Debug.Log(obj.activeSelf);
                obj.SetActive(!obj.activeSelf);
                
            }
        }
    }
        
    [MenuItem("GameObject/Reset #r")]
    public static void Reset()
    {
        bool involved;
        foreach (var selectedObject in Selection.gameObjects)
        {
            if (selectedObject is GameObject)
            {
                GameObject gameObj = (GameObject)selectedObject;
                involved = false;
                foreach (ICache resetter in gameObj.GetComponents<ICache>())
                {
                    involved = true;
                    resetter.OnCached();
                    Type type = resetter.GetType();
                    PrefabUtility.RecordPrefabInstancePropertyModifications(gameObj.GetComponent(type));
                    Undo.RegisterCompleteObjectUndo(gameObj, "Reset data");
                }

                if (!involved)
                {
                    if (gameObj.tag == "ResetOnParent")
                    {
                        ICache resetter = gameObj.GetComponentInParent<ICache>();
                        resetter.OnCached();
                        Type type = resetter.GetType();
                        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObj.GetComponentInParent(type));
                    }
                    else
                    {
                        Transform trx = gameObj.transform;
                        Undo.RegisterCompleteObjectUndo(trx, "Reset game object to origin");

                        trx.localPosition = Vector3.zero;
                        trx.localRotation = Quaternion.identity;
                        trx.localScale = Vector3.one;
                    }
                }

            }
            //else if (selectedObject is ICache)
            //{
            //    ((ICache)selectedObject).OnCached();
            //}
        }
                  
    }

    [MenuItem("GameObject/target Transformizable #w")]

    static public void TargetTransformizable()
    {
        GameObject target = Selection.gameObjects[0].GetComponentInParent<ITransformizable>()?.gameObject;
        if (target == null)
        {
            target = FindParentWithTransformizableTag(Selection.gameObjects[0].transform);
        }
        if (target != null) Selection.SetActiveObjectWithContext(target, null);
    }

    static GameObject FindParentWithTransformizableTag(Transform current)
    {
        if (current != null)
        {
            if (current.tag == "Transformizable") return current.gameObject;
            else return FindParentWithTransformizableTag(current.parent);
        }           
        else return null;
    }

    [MenuItem("GameObject/Test Action #t")]

    static public void TestAction()
    {
        Selection.gameObjects[0].GetComponent<ITest>().OnTest();

    }

    [MenuItem("UI/Open Command Line %#c")]
    static public void OpenCommandLine()
    {
        Archetype.Automation.ToggleCommandLine();
    }

    [MenuItem("GameObject/Gizmos Toggle #g")]
    static public void GizmosToggle()
    {
        IGizmos gizmos;
        foreach (GameObject obj in Selection.gameObjects)
        {
            gizmos = obj.GetComponent<IGizmos>();
            if (gizmos != null)
            {
                gizmos.debugMode = !gizmos.debugMode;
            }
        }      
    }


    [MenuItem("GameObject/target Player #p")]
    static public void TargetPlayer()
    {
        Selection.SetActiveObjectWithContext(Archetype.Player.gameObject, null);
    }

    [MenuItem("GameObject/target Main Camera #c")]
    static public void TargetMainCamera()
    {
        Selection.SetActiveObjectWithContext(Archetype.Cameraman.gameObject, null);
    }

    [MenuItem("GameObject/target Game Flow #f")]
    static public void TargetGameFlow()
    {
        Selection.SetActiveObjectWithContext(Archetype.GameFlow.gameObject, null);
    }

    [MenuItem("UI/Image/Set Icon Pivot ^#p")]
    static public void SetIconPivot()
    {
        if (Selection.activeGameObject.TryGetComponent(out Image image))
        {
            image.Normalize();
        }           
    }

    static void IconPivot(Vector2 direction)
    {
        if (Selection.activeGameObject.TryGetComponent(out Image image))
        {
            string path = AssetDatabase.GetAssetPath(image.sprite.texture);
            TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(path);
            Vector2 newPivot = image.sprite.GetPivot() + 0.003f * direction;
            ti.spritePivot = newPivot;
            TextureImporterSettings texSettings = new TextureImporterSettings();
            ti.ReadTextureSettings(texSettings);
            texSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            ti.SetTextureSettings(texSettings);
            ti.SaveAndReimport();

            var sizeDelta = image.rectTransform.sizeDelta;
            var anchorPos = image.rectTransform.anchoredPosition;

            image.rectTransform.pivot = image.sprite.GetPivot();

            image.rectTransform.sizeDelta = sizeDelta;
            image.rectTransform.anchoredPosition = anchorPos;
        }
    }

    [MenuItem("UI/Image/Icon Pivot Left ^#[")]
    static public void IconPivotLeft()
    {
        IconPivot(Vector2.left);
    }

    [MenuItem("UI/Image/Icon Pivot Right ^#]")]
    static public void IconPivotRight()
    {
        IconPivot(Vector2.right);
    }

    [MenuItem("UI/Image/Icon Pivot Up ^#=")]
    static public void IconPivotUp()
    {
        IconPivot(Vector2.up);
    }

    [MenuItem("UI/Image/Icon Pivot Down ^#'")]
    static public void IconPivotDown()
    {
        IconPivot(Vector2.down);
    }

    private const string MAIN_SCENE_PATH = "Assets/Scenes/Main Scene.unity"; 

    [MenuItem("Shortcuts/Open Main Scene %#m")] 
    public static void OpenMainScene()
    {
        if (!File.Exists(MAIN_SCENE_PATH))
        {
            Debug.LogError("Main Scene not found at path: " + MAIN_SCENE_PATH);
            return;
        }

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(MAIN_SCENE_PATH);
            Debug.Log("Opened Main Scene.");
        }
    }
}
