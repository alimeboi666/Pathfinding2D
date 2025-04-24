using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class RectTransformExtension
{
    //public static void SetRectFromViewport(this RectTransform rect, Vector2 viewPort)
    //{
    //    rect.anchoredPosition = viewPort / Ref.Instance.WorldCanvas.scaleFactor;
    //}

    public static void LookAtDirection2D(this Transform transform, Vector2 direction)
    {
        transform.rotation = (Quaternion.LookRotation(Vector3.forward, direction));
    }

    public static void LookAtDirection3D(this Transform transform, Vector3 direction, Space relative = Space.World)
    {
        transform.rotation = Quaternion.LookRotation(direction, relative == Space.World ? Vector3.up : transform.up);

    }
}

