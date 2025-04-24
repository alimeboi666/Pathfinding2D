using System.Collections;
using System.Collections.Generic;
using Hung.StatSystem;
using TMPro;
using UnityEngine;
using static Hung.StatSystem.Stat;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text txtDes;
    [SerializeField] private TMP_Text txtValue;
    
    public void Set(string des, Modifier modifier)
    {
        gameObject.SetActive(true);
        txtDes.text = des;
        if (modifier.percent > 1)
        {
            var percent = Mathf.RoundToInt(((modifier.percent * modifier.multiplier - 1) * 100));
            if (percent == 0) gameObject.SetActive(false);
            else txtValue.text = $"{(percent >= 0 ? "+" : "")}{percent}%";
        }
        else
        {
            if (modifier.multiplier == 1) gameObject.SetActive(false);
            else txtValue.text = $"x{modifier.multiplier:0.##}";
        }
    }
}