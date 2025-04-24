using Sirenix.OdinInspector;
using UnityEngine;
using AssetIcons;
using Hung.StatSystem;
using Hung.GameData.RPG;

namespace Hung.GameData
{
    public abstract class GameData : SerializedScriptableObject, IIdentifiedData, IIAPPurchasable
    {
        [field:SerializeField] public bool IsBanned { get; private set; }

        [field: ReadOnly, SerializeField] public int ID { get; private set; }

        [field: SerializeField] public string Name { get; protected set; }

        [field: SerializeField][field: PreviewField][AssetIcon] public Sprite Icon { get; private set; }

        [field: TextArea][field: SerializeField] public string Description { get; protected set; }

        [ReadOnly, SerializeField] protected bool hasInit;

        public virtual void OnIAPPurchased(int amount = 1)
        {
            Debug.Log("Purchase " + name + " for " + amount + " amounts");
        }

        public void Serialize(int ID)
        {
            this.ID = ID;
        }

        public virtual void Init()
        {
            hasInit = false;
        }

        public float PreloadCloudStat(Stat stat)
        {
            var key = name + "-" + stat.GetType().Name.ToLower();
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
            else
            {
                PlayerPrefs.SetFloat(key, stat.current);
                return stat.current;
            }
        }

        public void LoadCloudStat(Stat stat)
        {
            var key = name + "-" + stat.GetType().Name.ToLower();

            if (PlayerPrefs.HasKey(key))
            {
                stat.SetCurrent(PlayerPrefs.GetFloat(key));
            }
            else
            {
                PlayerPrefs.SetFloat(key, stat.current);
            }
        }

        public void SaveCloudStat(Stat stat)
        {
            PlayerPrefs.SetFloat(name + "-" + stat.GetType().Name.ToLower(), stat.current);
        }
    }

}

