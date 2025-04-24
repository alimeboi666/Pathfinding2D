
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hung.Core
{
    //public class Singleton<T> : SerializedMonoBehaviour, ISingletonRole where T : MonoBehaviour
    //{
    //    private static T instance;

    //    public static T Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                instance = FindObjectOfType(typeof(T)) as T;
    //                if (instance == null) Debug.Log("Turn on the " + typeof(T).ToString() + "!");
    //            }
    //            return instance;
    //        }
    //    }


    //}
    public class Singleton<T> : SerializedMonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static bool isQuitting = false;

        [SerializeField] private bool isDontDestroy = true;
        public bool IsDontDestroy
        {
            get => isDontDestroy;
            set
            {
                isDontDestroy = value;
                if (isDontDestroy && instance != null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
        }

        public static T Instance
        {
            get
            {
                if (isQuitting)
                {
                    Debug.LogWarning($"[{typeof(T)}] Instance already destroyed. Returning null.");
                    return null;
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();

                        Debug.Log($"[{typeof(T)}] Created new Singleton instance.");
                    }

                    if ((instance as Singleton<T>).isDontDestroy)
                    {
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (isDontDestroy) DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[{typeof(T)}] Duplicate instance detected. Destroying new instance.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            isQuitting = true;
        }
    }
}