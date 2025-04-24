using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Hung.GameData
{
    [CreateAssetMenu(menuName = "Hung/Data Collection")]
    public class DataCollection : SOSingleton<DataCollection>
    {
        [SerializeField] private GameData ads;

        [SerializeField] private List<GameResource> resources;

        [SerializeField] private List<GameData> data;

        [SerializeField] private Dictionary<string, GameData> mapping;

        static Dictionary<Type, GameResource> resourceDict = new();

        public static GameData GetAdsResource() => Instance.ads;

        public static bool TryGetDataByNameID<T>(string nameID, out T data) where T : GameData
        {
            if (Instance.mapping.ContainsKey(nameID))
            {
                data = Instance.mapping[nameID] as T;
                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }

        public static GameResource GetResourceData(IResource resource)
        {
            var resourceType = resource.GetType();
            if (resourceDict.ContainsKey(resourceType)) return resourceDict[resourceType];
            else
            {
                var resourceData = Instance.resources.Where(data =>
                {
                    //Debug.Log(data.Represent.GetType().ToString() + " :: " + resourceType.ToString());
                    return data.Represent.GetType() == resourceType;
                }).First();
                resourceDict[resourceType] = resourceData;
                return resourceData;
            }
        }

        public static event Action<GameData> OnInitData;

        public static void Initialize()
        {
            Instance.data.ForEach(d =>
            {
                d.Init();
                OnInitData?.Invoke(d);
            });
        }

#if UNITY_EDITOR
        [Button]
        public void SerializeAll()
        {
            // Tạo một danh sách để chứa tất cả các GameData tìm được
            data = new List<GameData>();
            resources = new List<GameResource>();
            mapping = new Dictionary<string, GameData>();
            // Tìm tất cả các đường dẫn của các assets loại GameData
            string[] guids = AssetDatabase.FindAssets("t:GameData");

            // Duyệt qua từng guid và tải các assets tương ứng
            int id = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameData gameData = AssetDatabase.LoadAssetAtPath<GameData>(path);
                if (gameData != null && !gameData.IsBanned)
                {
                    data.Add(gameData);
                    gameData.Serialize(id++);
                    mapping[gameData.name] = gameData;
                    if (gameData is GameResource currency)
                    {
                        resources.Add(currency);
                    }
                }
            }

            // Hiển thị kết quả trong console
            //Debug.Log("Found " + allGameData.Count + " GameData assets.");
            //foreach (GameData gameData in allGameData)
            //{
            //    Debug.Log("GameData: " + gameData.name);
            //}
        }
#endif
    }

}

