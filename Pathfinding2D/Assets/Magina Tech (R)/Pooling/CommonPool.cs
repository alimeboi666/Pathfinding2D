using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.Pooling
{
    public interface ICommonPoolable: IPoolable
    {

    }

    [CreateAssetMenu(menuName = "Hung/SO Singleton/Common Pool")]
    public class CommonPool : SOSingleton<CommonPool>
    {
        [SerializeReference] private List<ICommonPoolable> pool;

        //[field: NonSerialized, OdinSerialize][HideLabel][SerializeField] private Dictionary<ICommonPoolable, GameObject> poolDict;

        private static Dictionary<Type, ICommonPoolable> indexDict = new();

        public static void Spawn<T>(out T spawnling, Transform parent = null) where T: ICommonPoolable
        {
            int index;
            if (!indexDict.ContainsKey(typeof(T)))
            {
                index = Instance.pool.FindIndex(poolable => poolable.gameObject.GetComponent<T>() != null);
                if (index < 0)
                {
                    spawnling = default(T);
                    return;
                }
                indexDict[typeof(T)] = Instance.pool[index].gameObject.GetComponent<T>();
            }
            if (parent == null) spawnling = Pool.Spawn((T)indexDict[typeof(T)]);
            else spawnling = Pool.Spawn((T)indexDict[typeof(T)], parent);
        }
    }
}

namespace Hung.Pooling.Core
{
    public class Type_TextGuide: ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field: SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();
    }

    public class Type_ResourceClaim : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field:SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();
    }

    public class Type_Point : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field:SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();
    }

    public class Type_AudioSourcer : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field:SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();

    }

    public class Type_WorldIcon : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field: SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();

    }

    public class Type_Projectile : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isNewPooling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [field: SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new NotImplementedException();

    }

    public class Type_AOE : ICommonPoolable
    {
        public PoolPartition pooling { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool isNewPooling { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        [field:SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new System.NotImplementedException();

    }

    public class Type_HealthBar : ICommonPoolable
    {

        public bool enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        [field: SerializeField] public GameObject gameObject { get; private set; }

        public Transform transform => throw new System.NotImplementedException();

        public PoolPartition pooling { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool isNewPooling { get; set; }

        public void BackToPool()
        {
            throw new System.NotImplementedException();
        }
    }
}