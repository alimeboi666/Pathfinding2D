using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteUtilitis
{
    public static Vector2 GetPivot(this Sprite sprite)
    {
        return sprite.pivot / sprite.rect.size;
    }

    public static void Normalize(this Image image)
    {
        var sizeDelta = image.rectTransform.sizeDelta;
        var anchorPos = image.rectTransform.anchoredPosition;

        image.rectTransform.pivot = image.sprite.GetPivot();

        image.rectTransform.sizeDelta = sizeDelta;
        image.rectTransform.anchoredPosition = anchorPos;
    }
}
