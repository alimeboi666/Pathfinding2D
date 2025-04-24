using Hung.Core;
using Hung.StatSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.GameData
{
    public delegate bool ResourceClaimer(ResourceClaim resourceClaim, int claimTime, EnormousValue eachClaim);

    public class GameResource : GameData
    {
        [field: SerializeField] public IResource Represent { get; private set; }

        //[SerializeField] private EnormousValue currentValue;
        [field: SerializeField] public float flexibleValue { get; private set; }

        protected event Action<EnormousValue> OnValueChange;

        public event ResourceClaimer OnVisualClaim;

        public void VisualClaim(ResourceClaim resourceClaim, int claimTime, EnormousValue eachValue)
        {
            if (OnVisualClaim == null) return;
            //GameController.Instance.StartCoroutine(AssignClaimer(resourceClaim, claimTime, eachValue));
        }

        IEnumerator AssignClaimer(ResourceClaim resourceClaim, int claimTime, EnormousValue eachValue)
        {
            yield return new WaitUntil(() => OnVisualClaim(resourceClaim, claimTime, eachValue));
        }

        //public EnormousValue Value
        //{
        //    get => currentValue;
        //    set
        //    {
        //        if (currentValue!= value)
        //        {
        //            //currentValue = (float)Math.Ceiling(clamp * 1000) /1000;
        //            currentValue = value;
        //            OnValueChange?.Invoke(value);
        //        }
        //    }
        //}

        public EnormousValue Value
        {
            get => new EnormousValue(PlayerPrefs.GetFloat(name + "-value", Represent.value.Value), (MetricPrefix)PlayerPrefs.GetInt(name + "-metrix", (int)Represent.value.Prefix));
            set
            {
                if (Value != value)
                {
                    PlayerPrefs.SetFloat(name + "-value", value.Value);
                    PlayerPrefs.SetInt(name + "-metrix", (int)value.Prefix);

                    OnValueChange?.Invoke(value);
                }
            }
        }

        [Button]
        public void ListenerCount()
        {
            Debug.Log(OnValueChange.GetInvocationList().Length);
        }

        public bool AttempToConsume(IResource signResource)
        {
            if (Represent.GetType() != signResource.GetType()) return false;

            return signResource.value + Value >= 0;
        }

        public bool TryToConsume(IResource signResource)
        {
            if (Represent.GetType() != signResource.GetType()) return false;
            if (signResource.value + Value >= 0)
            {
                Value += signResource.value;
                return true;
            }
            return false;
        }

        public void AddAndCheckValueChange(Action<EnormousValue> listener)
        {
            listener(Value);
            OnValueChange += listener;
        }

        public void RemoveValueChange(Action<EnormousValue> listener)
        {
            OnValueChange -= listener;
        }
    }

    public interface IResource
    {
        AdvertimentClickType adReplaceableType { get; }

        bool noticeable { get; }

        EnormousValue value { get; set; }

        string postFix { get; }
    }

    public interface ICurrencyResource : IResource
    {

    }

    public class Coin : ICurrencyResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public string postFix => "coin";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.None;

        public bool noticeable => true;

        public Coin(EnormousValue value)
        {
            this.value = value;
        }
    }

    public class Dollar : ICurrencyResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public string postFix => "$";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.None;

        public bool noticeable => true;

        public Dollar(EnormousValue value)
        {
            this.value = value;
        }
    }

    public class Diamond : ICurrencyResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public string postFix => "gem";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.None;

        public bool noticeable => false;

        public Diamond(EnormousValue value)
        {
            this.value = value;
        }
    }

    public class AdsOrTicket : ICurrencyResource
    {
        public AdsOrTicket()
        {
            value = -1;
        }

        public AdsOrTicket(EnormousValue value)
        {
            this.value = value;
        }

        [field: HideLabel, SerializeField] public EnormousValue value { get; set; } = -1;

        public string postFix => "ticket";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.Rewarded;

        public bool noticeable => true;
    }

    public class Hammer : ICurrencyResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public string postFix => "hammer";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.Rewarded;

        public bool noticeable => true;

        public Hammer(EnormousValue value)
        {
            this.value = value;
        }
    }

    public class AdsOnly : ICurrencyResource
    {
        [field: SerializeField] public AdvertimentClickType adReplaceableType { get; private set; }

        public AdsOnly()
        {
            value = -1;
            adReplaceableType = AdvertimentClickType.Rewarded;
        }

        public AdsOnly(AdvertimentClickType adType)
        {
            value = -1;
            adReplaceableType = adType;
        }

        [field: HideLabel, SerializeField] public EnormousValue value { get; set; } = -1;

        public string postFix => "";



        public bool noticeable => false;
    }

    public class CookingMaterial : ICurrencyResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public string postFix => "wood";

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.None;

        public bool noticeable => true;

        public CookingMaterial(EnormousValue value)
        {
            this.value = value;
        }
    }
}

