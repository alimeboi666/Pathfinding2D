using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hung.Core
{
    public class Hollow : Singleton<Hollow>, IModel
    {
        private Dictionary<Type, GameObject> map = new Dictionary<Type, GameObject>();

        private Type _type;
        internal T Get<T>() where T : Component
        {
            _type = typeof(T);
            if (!map.ContainsKey(_type))
            {
                GameObject obj = new GameObject(_type.ToString());
                obj.transform.SetParent(this.transform);
                map[_type] = obj;
                if (typeof(T) != typeof(Transform))
                {
                    return obj.AddComponent<T>();
                }
                else
                {
                    return obj.GetComponent<T>();
                }
            }
            else
            {
                return map[_type].GetComponent<T>();
            }
        }

        [SerializeField] private Transform disabledLocation;
        internal static Transform Location
        {
            get => Instance.disabledLocation;
        }

        public GameObject Model => disabledLocation.gameObject;
    }
}
