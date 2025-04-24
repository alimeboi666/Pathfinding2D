using Hung.GameData;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.GameData
{
    [CreateAssetMenu(menuName = "Hung/Data/Resource/Currency")]
    public sealed class CurrencyData : GameResource
    {
        [field: AssetsOnly, SerializeField] public DisableByTime spendVFX { get; private set; }

        [field: AssetsOnly, SerializeField] public AudioClip soundPurchase { get; private set; }

        [field: AssetsOnly, SerializeField] public AudioClip soundPartClaim { get; private set; }

        //[InlineButton("Add"), SerializeField] private EnormousValue addValue;
        //[InlineButton("Minus"), SerializeField] private EnormousValue minusValue;
        //[InlineButton("Multiply"), SerializeField] private EnormousValue mutilplyValue;
        //[InlineButton("Divide"), SerializeField] private EnormousValue divideValue;

        //void Add()
        //{
        //    Value += addValue;
        //    //Debug.Log(Value.ToString()); 
        //}

        //void Minus()
        //{
        //    Value -= minusValue;
        //}

        //void Multiply()
        //{
        //    Value *= mutilplyValue;
        //}

        //void Divide()
        //{
        //    Value /= divideValue;
        //}
    }   
}

