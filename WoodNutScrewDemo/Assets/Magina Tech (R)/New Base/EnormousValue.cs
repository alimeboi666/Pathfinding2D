using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MetricPrefix { None = 0, K = 3, M = 6, B = 9, T = 12, AA = 15, AB = 18, AC = 21, AD = 24, AE = 27, AF = 30, AG = 33, AH = 36, AI = 39, AJ = 42, AK = 45, AL = 48, AM = 51, AN = 54, AO = 57, AP = 60, AQ = 63, AR = 66, AS = 69, AT = 72, AU = 75, AV = 78, AW = 81, AX = 84, AY = 87, AZ = 90 }

[Serializable]
public struct EnormousValue
{
    [HorizontalGroup(LabelWidth = 55), HideLabel] public float Value;
    [HorizontalGroup(LabelWidth = 50, Width = 80), HideLabel] public MetricPrefix Prefix;

    public EnormousValue(float value, MetricPrefix prefix = MetricPrefix.None, bool isNormalize = true)
    {
        Value = value;
        Prefix = prefix;

        if (isNormalize) Normalize();
    }

    public void Normalize()
    {
        var sign = Mathf.Sign(Value);
        Value = Mathf.Abs(Value);
        while (Value < 1f && Prefix >= MetricPrefix.K)
        {
            Value *= 1000f;
            Prefix -= 3;
        }

        // If value is greater than or equal to 1000, increase prefix
        while (Value >= 1000f && Prefix < MetricPrefix.AZ)
        {
            Value /= 1000f;
            Prefix += 3;
        }
        Value = sign * Value;
    }

    public static implicit operator EnormousValue(float value)
    {
        MetricPrefix prefix = MetricPrefix.None;

        if (value >= 1e9f) // 1 B (billion)
        {
            prefix = MetricPrefix.B;
            value /= 1e9f;
        }
        else if (value >= 1e6f) // 1 M (million)
        {
            prefix = MetricPrefix.M;
            value /= 1e6f;
        }
        else if (value >= 1e3f) // 1 K (thousand)
        {
            prefix = MetricPrefix.K;
            value /= 1e3f;
        }

        return new EnormousValue(value, prefix);
    }

    public static implicit operator float(EnormousValue value)
    {
        // Chuẩn hóa giá trị EnormousValue về giá trị float tương ứng
        return value.Value * Mathf.Pow(10, (int)value.Prefix);
    }

    public static EnormousValue Abs(EnormousValue value)
    {
        return new EnormousValue(Math.Abs(value.Value), value.Prefix);
    }

    public float ToRealValue()
    {
        float realValue = Value;

        switch (Prefix)
        {
            case MetricPrefix.K:
                realValue *= 1e3f; // 1K = 1,000
                break;
            case MetricPrefix.M:
                realValue *= 1e6f; // 1M = 1,000,000
                break;
            case MetricPrefix.B:
                realValue *= 1e9f; // 1B = 1,000,000,000
                break;
            case MetricPrefix.T:
                realValue *= 1e12f; // 1T = 1,000,000,000,000
                break;
            case MetricPrefix.AA:
                realValue *= 1e15f; // 1Aa = 1,000,000,000,000,000
                break;
            case MetricPrefix.AB:
                realValue *= 1e18f; // 1Ab = 1,000,000,000,000,000,000
                break;
            case MetricPrefix.AC:
                realValue *= 1e21f; // 1Ac = 1,000,000,000,000,000,000,000
                break;
            case MetricPrefix.AD:
                realValue *= 1e24f; // 1Ad = 1,000,000,000,000,000,000,000,000
                break;
            case MetricPrefix.AE:
                realValue *= 1e27f; // 1Ae = 1,000,000,000,000,000,000,000,000,000
                break;
            default:
                break;
        }

        return realValue;
    }

    public static EnormousValue operator +(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return new EnormousValue(a.Value + b.Value, a.Prefix);
    }

    public static EnormousValue operator -(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        //Debug.Log(a + " " + b); 
        return new EnormousValue(a.Value - b.Value, a.Prefix);
    }

    public static EnormousValue operator *(EnormousValue left, EnormousValue right)
    {
        float resultValue = left.Value * right.Value;
        MetricPrefix resultPrefix = (MetricPrefix)Math.Min((int)left.Prefix + (int)right.Prefix, Enum.GetValues(typeof(MetricPrefix)).Cast<int>().Max());

        return new EnormousValue(resultValue, resultPrefix);
    }

    //public static EnormousValue operator /(EnormousValue dividend, EnormousValue divisor)
    //{
    //    float resultValue = dividend.Value / divisor.Value;
    //    MetricPrefix resultPrefix = (MetricPrefix)Math.Max((int)dividend.Prefix - (int)divisor.Prefix, 0);

    //    return new EnormousValue(resultValue, resultPrefix);
    //}
    //public static EnormousValue operator /(EnormousValue dividend, EnormousValue divisor)
    //{
    //    float resultValue = dividend.ToRealValue() / divisor.ToRealValue();
    //    MetricPrefix resultPrefix = MetricPrefix.None; // Always normalize to None after division

    //    return new EnormousValue(resultValue, resultPrefix);
    //}

    public static EnormousValue operator /(EnormousValue dividend, EnormousValue divisor)
    {
        float resultValue = dividend.Value / divisor.Value;
        MetricPrefix resultPrefix = (MetricPrefix)Math.Max((int)dividend.Prefix - (int)divisor.Prefix, 0);

        // Adjust the result prefix based on the actual numerical value
        while (resultValue >= 1000f && resultPrefix < MetricPrefix.AZ)
        {
            resultValue /= 1000f;
            resultPrefix++;
        }

        return new EnormousValue(resultValue, resultPrefix);
    }


    public static bool operator >(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value > b.Value;
    }

    public static bool operator <(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value < b.Value;
    }

    public static bool operator >=(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value >= b.Value;
    }

    public static bool operator <=(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value <= b.Value;
    }

    public static bool operator ==(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value == b.Value;
    }

    public static bool operator !=(EnormousValue a, EnormousValue b)
    {
        Normalize(ref a, ref b);
        return a.Value != b.Value;
    }

    public static EnormousValue operator +(EnormousValue a, float b)
    {
        EnormousValue B = b;
        return a + B;
    }

    public static EnormousValue operator -(EnormousValue a, float b)
    {
        EnormousValue B = b;
        return a - B;
    }

    public static EnormousValue operator *(EnormousValue a, float b)
    {
        EnormousValue B = b;
        return a * B;
    }

    public static EnormousValue operator /(EnormousValue a, float b)
    {
        EnormousValue B = b;
        return a / B;
    }

    public static EnormousValue operator +(float a, EnormousValue b)
    {
        return b + a;
    }

    public static EnormousValue operator -(float a, EnormousValue b)
    {
        return new EnormousValue(a - b.Value, b.Prefix);
    }

    public static EnormousValue operator *(float a, EnormousValue b)
    {
        return new EnormousValue(a * b.Value, b.Prefix);
    }

    public static EnormousValue operator /(float a, EnormousValue b)
    {
        return new EnormousValue(a / b.Value, b.Prefix);
    }

    public static EnormousValue operator +(EnormousValue a, int b)
    {
        return a + (float)b;
    }

    public static EnormousValue operator -(EnormousValue a, int b)
    {
        return a - (float)b;
    }

    public static EnormousValue operator *(EnormousValue a, int b)
    {
        return a * (float)b;
    }

    public static EnormousValue operator /(EnormousValue a, int b)
    {
        return a / (float)b;
    }

    public static EnormousValue operator +(int a, EnormousValue b)
    {
        return new EnormousValue(a + b.Value, b.Prefix);
    }

    public static EnormousValue operator -(int a, EnormousValue b)
    {
        return new EnormousValue(a - b.Value, b.Prefix);
    }

    public static EnormousValue operator *(int a, EnormousValue b)
    {
        return (float)a * b;
    }

    public static EnormousValue operator /(int a, EnormousValue b)
    {
        return new EnormousValue(a / b.Value, b.Prefix);
    }

    public static bool operator >(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) > b;
    }

    public static bool operator <(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) < b;
    }

    public static bool operator >=(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) >= b;
    }

    public static bool operator <=(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) <= b;
    }

    public static bool operator ==(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) == b;
    }

    public static bool operator !=(EnormousValue a, float b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) != b;
    }

    public static bool operator >(float a, EnormousValue b)
    {
        return a  > b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator <(float a, EnormousValue b)
    {
        return a  < b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator >=(float a, EnormousValue b)
    {
        return a  >= b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator <=(float a, EnormousValue b)
    {
        return a <= b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator ==(float a, EnormousValue b)
    {
        return a == b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator !=(float a, EnormousValue b)
    {
        return a != b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator >(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) > b;
    }

    public static bool operator <(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) < b;
    }

    public static bool operator >=(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) >= b;
    }

    public static bool operator <=(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) <= b;
    }

    public static bool operator ==(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) == b;
    }

    public static bool operator !=(EnormousValue a, int b)
    {
        return a.Value * Mathf.Pow(10, (int)a.Prefix) != b;
    }

    public static bool operator >(int a, EnormousValue b)
    {
        return a > b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator <(int a, EnormousValue b)
    {
        return a < b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator >=(int a, EnormousValue b)
    {
        return a >= b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator <=(int a, EnormousValue b)
    {
        return a <= b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator ==(int a, EnormousValue b)
    {
        return a == b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static bool operator !=(int a, EnormousValue b)
    {
        return a != b.Value * Mathf.Pow(10, (int)b.Prefix);
    }

    public static EnormousValue operator -(EnormousValue a)
    {
        return new EnormousValue(-a.Value, a.Prefix);
    }

    private static void Normalize(ref EnormousValue a, ref EnormousValue b)
    {
        if (a.Prefix == b.Prefix)
            return;

        if (a.Prefix < b.Prefix)
        {
            b = new EnormousValue(b.Value * (float)Math.Pow(10, (int)(b.Prefix - a.Prefix)), a.Prefix, false);         
        }
        else
        {
            a = new EnormousValue(a.Value * (float)Math.Pow(10, (int)(a.Prefix - b.Prefix)), b.Prefix, false);
        }
        //Debug.Log(a + "/" + b);
    }

    public override bool Equals(object obj)
    {
        if (obj is EnormousValue other)
        {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ Prefix.GetHashCode();
    }

    public override string ToString()
    {
        if (float.IsInfinity(Value))
        {
            return "∞";
        }
        if (Prefix == MetricPrefix.None)
        {
            return $"{Value.ToString("0.##")}";
        }
        else
        {         
            return $"{Value.ToString("0.##")}{Prefix}";
        }
    }

    public string ToString(string format)
    {
        if (float.IsInfinity(Value))
        {
            return "∞";
        }
        if (Prefix == MetricPrefix.None)
        {
            return $"{Value.ToString(format)}";
        }
        else
        {
            return $"{Value.ToString("0.##")}{Prefix}";
        }
    }
}