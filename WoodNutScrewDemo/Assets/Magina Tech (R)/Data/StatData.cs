using Hung.Core;
using Hung.GameData;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hung.GameData
{
    [CreateAssetMenu(menuName = "Hung/Data/Resource/Stat")]
    public sealed class StatData : GameResource
    {

    }

    public interface IStatResource: IResource
    {

    }

    public class Rating : IStatResource
    {
        [field: HideLabel, SerializeField] public EnormousValue value { get; set; }

        public AdvertimentClickType adReplaceableType => AdvertimentClickType.None;

        public bool noticeable => false;

        public string postFix => "";

        public Rating(float value)
        {
            this.value = value;
        }
    }

    //public class Satisfaction : IStatResource
    //{
    //    [field:SerializeField] public EnormousValue value { get; set; }
    //    public string postFix => "";

    //    public void GetNegativeValue()
    //    {
    //        if (value > 0) value = -value;
    //    }

    //    public void GetPositiveValue()
    //    {
    //        if (value < 0) value = -value;
    //    }
    //}

    //public class ShipperSpeed: IStatResource
    //{
    //    [field: SerializeField] public EnormousValue value { get; set; }

    //    public string postFix => "%";

    //    public void GetNegativeValue()
    //    {
    //        if (value > 0) value = -value;
    //    }

    //    public void GetPositiveValue()
    //    {
    //        if (value < 0) value = -value;
    //    }
    //}
}
