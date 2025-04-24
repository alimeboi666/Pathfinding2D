using UnityEngine;

public static class JsonExtension
{
    public static T Load<T>(string fileName, string resourcePath)
    {
        //Debug.Log(Resources.Load<TextAsset>(resourcePath + fileName).ToString()); 
        return JsonUtility.FromJson<T>(Resources.Load<TextAsset>(resourcePath + fileName).ToString());
    }

    public static string Create(object obj)
    {
        return JsonUtility.ToJson(obj);      
    }
}

