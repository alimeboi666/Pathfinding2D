
using Unity.Mathematics;
using UnityEngine;

public static class GeometryUtils
{
    public static float GetAngle2D(float3 from, float3 to)
    {

        var _dir = to - from;
        if (_dir.z == 0) return _dir.x >= 0 ? (math.PI * 0.5f) : (math.PI * 1.5f);

        float res;
        if (_dir.z > 0)
        {
            res = NormalizeRadianAngle(math.atan(_dir.x / _dir.z));
        }
        else
        {
            res = NormalizeRadianAngle(math.atan(_dir.x / _dir.z) + math.PI);
        }
        //UnityEngine.Debug.Log(res * 180/math.PI);
        return res;
    }

    public static float Plus(float angle, float plusValue)
    {
        return NormalizeRadianAngle(angle + plusValue);
    }

    public static bool IsSmallerThan(float angle1, float angle2)
    {
        var angleBetween = math.abs(angle1 - angle2);

        if (angle1 >= angle2) return angleBetween >= math.PI;
        else return angleBetween < math.PI;
    }

    public static float GetMiddleAngle2D(float min, float max)
    {
        if (max >= min) return min + (max - min) / 2;
        else return NormalizeRadianAngle(math.PI - (min - max) / 2 + min);
    }

    public static float NormalizeRadianAngle(float angle)
    {
        angle = angle % (2 * math.PI);
        if (angle < 0)
        {
            angle += 2 * math.PI;
        }
        return angle;
    }

    public static Vector3 GetDirection2D(float angle)
    {
        return new Vector3(math.sin(angle), 0, math.cos(angle));
    }
}