using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweener_Home : UITweener
{
    [SerializeField] private RectTransform mainBtn; 

    public override void VisualOn()
    {
        mainBtn.localScale = Vector3.zero;
        //mainBtn.DOScale(1, 1)  

    }

    public override void VisualOff()
    {
        
    }  
}
    
