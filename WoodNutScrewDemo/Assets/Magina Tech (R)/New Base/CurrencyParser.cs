using Hung.GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CurrencyParser
{
    public static ICurrencyResource Parse(string input)
    {
        var regex = new Regex(@"(?<value>[-+]?\d*\.?\d+)(?<prefix>[kKmMbBtT]?)(?<postfix>gem|voucher|wood|hammer|\$)?");
        var match = regex.Match(input);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid format");
        }

        float value = float.Parse(match.Groups["value"].Value);
        string prefixStr = match.Groups["prefix"].Value;
        string postfix = match.Groups["postfix"].Value;

        MetricPrefix prefix = MetricPrefix.None;
        if (!string.IsNullOrEmpty(prefixStr))
        {
            prefix = (MetricPrefix)Enum.Parse(typeof(MetricPrefix), prefixStr.ToUpper());
        }

        EnormousValue enormousValue = new EnormousValue(value, prefix);
        //Debug.Log("Postfix: " + postfix);
        if (postfix == "gem")
        {
            return new Diamond(enormousValue);
        }
        else if (postfix == "hammer" || postfix == "voucher")
        {
            return new AdsOrTicket(enormousValue);
        }
        else if (postfix == "wood")
        {
            return new CookingMaterial(enormousValue);
        }
        else if (postfix == "$" || string.IsNullOrEmpty(postfix))
        {
            return new Dollar(enormousValue);
        }

        else
        {
            throw new ArgumentException("Invalid currency type");
        }
    }
}