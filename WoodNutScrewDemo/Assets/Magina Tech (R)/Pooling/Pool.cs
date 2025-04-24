using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using UnityEngine;

namespace Hung.Pooling
{
    public class PoolPartition: MonoBehaviour
    {
        [SerializeField] private GameObject origin;
        [SerializeField] private Stack<GameObject> partition = new();

        public T Init<T>(T origin, Transform parent, Vector3 position) where T: IPoolable
        {
            origin.pooling = this;
            this.origin = origin.gameObject;
            return Clone<T>(parent, position);
        }

        public T Clone<T>(Transform parent, Vector3 position) where T: IPoolable
        {
            var pooling = Instantiate(origin.gameObject, position, Quaternion.identity, parent).GetComponent<T>();
            pooling.pooling = this;
            pooling.isNewPooling = true;
            return pooling;
        }

        public void Back(IPoolable pooling)
        {
            partition.Push(pooling.gameObject);
            pooling.transform.SetParent(transform);
            pooling.gameObject.SetActive(false);
            //pooling.ToggleOff();
        }

        public T Peek<T>(Transform parent, Vector3 position) where T: IPoolable
        {
            T pooling;
            if (partition.Count == 0)
            {
                pooling = Clone<T>(parent, position);
            }
            else
            {
                //pooling = partition[partition.Count - 1].GetComponent<T>();
                //partition.Remove(pooling.gameObject);
                pooling = partition.Pop().GetComponent<T>();
                pooling.isNewPooling = false;
                pooling.transform.SetParent(parent);
            }
            
            pooling.transform.position = position;
            

            return pooling;
        }
    }

    [CreateAssetMenu(menuName = "Hung/Pooling/Singleton")]
    public class Pool : SOSingleton<Pool>
    {
        //[SerializeField] private List<GameObject> origins = new(10);
        //static int POOLING_ID;

        public static T Spawn<T>(T origin, Transform parent) where T: IPoolable
        {
            var res = Spawn(origin, parent.position, parent);
            res.transform.SetParent(parent);
            res.transform.localRotation = Quaternion.identity;
            return res;
        }

        public static T Spawn<T>(T origin, Vector3 position = default, Transform parent = null) where T : IPoolable
        {
            var pooling = origin.pooling;
            T res;

            if (parent == null && origin is ICanvas ui)
            {
                //Debug.Log("Attach to canvas: " + (ui.targetCanvas != null ? ui.targetCanvas.name: "nothing"));
                if (ui.targetCanvas == null) parent = Archetype.WorldCanvas.targetCanvas.transform;
                else parent = ui.targetCanvas.transform;
            }

            if (pooling == null)
            {
                var partition = new GameObject("=> Partition: " + origin.gameObject.name).AddComponent<PoolPartition>();
                res = partition.Init(origin, parent, position);
            }
            else
            {
                res = pooling.Peek<T>(parent, position);
            }

            //Debug.Log("Pool off: " + res.gameObject.name);
            //if (res is ICanvas ui && ui.targetCanvas == null)
            //{
            //    Debug.Log("Attach to canvas");
            //    ui.transform.SetParent(WorldCanvas.Instance.targetCanvas.transform, true);
                
            //}
            //else res.transform.SetParent(null);
            res.gameObject.SetActive(true);
            //res.ToggleOn();
            return res;
        }

        public static void BackToPool(IPoolable poolable)
        {
            if (poolable == null) return;
            if (poolable.pooling != null)
            {
                poolable.pooling.Back(poolable);
                //foreach (var cleanup in poolable.gameObject.GetComponents<IPoolCleanup>())
                //{
                //    cleanup.Dispose();
                //}
            }
            else
            {
                Debug.LogWarning("Can't pooling off: " + poolable.gameObject.name);
                poolable.gameObject.SetActive(false);
                if (poolable is IDisposable disposer)
                {
                    disposer.Dispose();
                }
                //poolable.ToggleOff();
            }           
        }
    }

    public static class PoolUltilitis
    {
        public static void BackToPool(this IPoolable poolable)
        {
            Pool.BackToPool(poolable);
            poolable = null;
        }
    }
}
