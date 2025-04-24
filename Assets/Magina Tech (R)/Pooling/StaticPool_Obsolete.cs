using Hung.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hung.Pooling
{
    public partial class StaticPool_Obsolete : Singleton<StaticPool_Obsolete>
    {
        [SerializeField] private Partition[] partitions;

        private Dictionary<Type, AmmoType> ammoTypeMap = new Dictionary<Type, AmmoType>()
        {
            {
                typeof(ArrowPoint),
                AmmoType.ArrowPoint
            }
        };

        private Dictionary<Type, BulletType> projectileMap = new Dictionary<Type, BulletType>()
        {
        
        };

        private Dictionary<string, int> offsetIndexMap = new Dictionary<string, int>();

        public void Awake()
        {
            offsetIndexMap[typeof(AmmoType).ToString()] = 0;
            offsetIndexMap[typeof(BulletType).ToString()] = Enum.GetNames(typeof(AmmoType)).Length;
            offsetIndexMap[typeof(OtherPoolable).ToString()] = offsetIndexMap[typeof(BulletType).ToString()] + Enum.GetNames(typeof(BulletType)).Length;
            //offsetIndexMap[typeof(WorldUIType).ToString()] = offsetIndexMap[typeof(EnemySpawnType).ToString()] + Enum.GetNames(typeof(EnemySpawnType)).Length;
          

            Awake_Implemented();
            //foreach (KeyValuePair<string, int> pair in offsetIndexMap)
            //{
            //    Debug.Log(pair.Key + ": " + pair.Value);
            //}
        }

        partial void Awake_Implemented();

        internal void SetRealtimeOrigin<T>(T origin) where T : MonoBehaviour, IPoolable
        {
            if (origin is BaseProjectile)
            {
                BulletType projectileType = projectileMap[typeof(T)];
                partitions[GetPartitionIndex(projectileType)].origin = origin.gameObject;
            }
        }

        private int GetPartitionIndex<T>(T enumType) where T : Enum
        {
            //Debug.Log(enumType.GetType().ToString());
            //Debug.Log(offsetIndexMap.Count);
            //foreach (KeyValuePair<string, int> entry in offsetIndexMap)
            //{P
            //    Debug.Log(entry.Key.ToString());
            //}
            //Debug.Log((int)(object)enumType + " " + offsetIndexMap[enumType.GetType().ToString()]);            
            return (int)(object)enumType + offsetIndexMap[enumType.GetType().ToString()];
        }

        protected void Spawn_Core<T>(int partitionIndex, out T spawnling, out bool isNew) where T : IPoolable
        {
            Transform storage = partitions[partitionIndex].storage;
            if (storage.childCount > 0)
            {
                //ammoTransform.SetParent(null);
                spawnling = storage.GetChild(0).GetComponent<T>();
                spawnling.transform.SetParent(null, true);
                isNew = false;
            }
            else
            {
                GameObject origin = partitions[partitionIndex].origin;
                if (origin.transform.parent == null)
                {
                    spawnling = Instantiate(origin, Hollow.Location).GetComponent<T>();
                    spawnling.gameObject.SetActive(false); 
                    spawnling.transform.SetParent(null);
                }
                else
                {
                    spawnling = Instantiate(origin, origin.transform.parent, true).GetComponent<T>();
                }
                //spawnling.pooling = storage;
                isNew = true;
            }
        }  

        internal void Spawn<T>(AmmoType type, Vector3 position, out T spawnling) where T : PoolAmmo
        {
            int partitionIndex = GetPartitionIndex(type);

            Spawn_Core(partitionIndex, out spawnling, out _);
            spawnling.Cast(position);
        }

        internal void Spawn<T>(OtherPoolable type, out T spawnling) where T : IPoolable
        {
            int partitionIndex = GetPartitionIndex(type);
            Spawn_Core(partitionIndex, out spawnling, out _);
        }

        internal void Spawn<T>(AmmoType type, out T spawnling) where T : PoolAmmo
        {
            int partitionIndex = GetPartitionIndex(type);

            Spawn_Core(partitionIndex, out spawnling, out _);
        }

        internal void Spawn<T>(BulletType type, out T spawnling) where T : BaseProjectile
        {
            int partitionIndex = GetPartitionIndex(type);

            Spawn_Core(partitionIndex, out spawnling, out bool isNew);
        }

        internal void Spawn<T>(WorldUIType type, out T spawnling) where T: WorldUI
        {
            int partitionIndex = GetPartitionIndex(type);

            Spawn_Core(partitionIndex, out spawnling, out _);

            //spawnling.AttachToCanvas();
        }

        internal void Spawn(OtherPoolable type, Vector3 position)
        {
            int partitionIndex = GetPartitionIndex(type);

            Spawn_Core(partitionIndex, out IPoolable spawnling, out bool _);

            spawnling.transform.position = position;
            spawnling.gameObject.SetActive(true);
        }       
           
        internal T Spawn<T>(AmmoType type, Vector3 position) where T : PoolAmmo
        {
            Spawn(type, position, out T spawnling);
            return spawnling;
        }

        internal T Spawn<T>(AmmoType type) where T : PoolAmmo
        {
            Spawn(type, out T spawnling);
            return spawnling;
        }

        internal T Spawn<T>(BulletType type) where T : BaseProjectile
        {
            Spawn(type, out T spawnling);
            return spawnling;
        }

        internal PoolAmmo Spawn(AmmoType type, Vector3 position)
        {
            Spawn(type, position, out PoolAmmo spawnling);
            //spawnling.ToggleOn();
            return spawnling;

        }
        internal BaseProjectile Spawn(BulletType type)
        {
            Spawn(type, out BaseProjectile spawnling);
            return spawnling;
        }

        internal T Spawn<T>() where T: PoolAmmo
        {
            return Spawn<T>(ammoTypeMap[typeof(T)]);
        }
    }

    public abstract class BaseProjectile : PoolAmmo
    {

    }

    public enum AmmoType
    {
        HitPoint,
        ArrowPoint,
        ArrowPoint2,
        ExplosionVFX,
        MineExplosionVFX,
        LightningBolt,
        BrokenGlass,
        IceVFX,
        BossPortal,
        UnitVoxel,
        BossVoxelVFX,
        MoneyVFX,
        BulletSparks
    }

    public enum BulletType
    {             
        _5mm56,
        _7mm89,
        Missile,
        None
    }

    public enum OtherPoolable
    {
        Mine,
        SnowVFX,
        VirtualBullet,
        FlyingBuilding
    }

    public enum WorldUIType
    {
        HealthBar,
        DebugText,
        WarningIcon,
        OffscreenNavigator
    }

    [Serializable]
    public struct Partition
    {
        public GameObject origin;
        public Transform storage;
    }


    public interface IPoolable : IMono
    {
        PoolPartition pooling { get; set; }

        bool isNewPooling { get; set; }
    }
}
