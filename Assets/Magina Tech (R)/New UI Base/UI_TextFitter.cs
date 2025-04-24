using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class UI_TextFitter : MonoBehaviour, IDriven
{
    private ContentSizeFitter control;
    private TMP_Text text;

    [Range(0, 1000), SerializeField] private float maxWidth;

    private void Awake()
    {
        if (control == null) control = GetComponentInParent<ContentSizeFitter>();
        if (text == null)
        {
            text = GetComponent<TMP_Text>();           
        }
    }

    private void Start()
    {
        //text.OnPreRenderText += TextFit;
    }

    void TextFit(TMP_TextInfo info = null)
    {
        control.enabled = true;
        control.SetLayoutHorizontal();
        var rect = control.transform as RectTransform;
        var size = rect.sizeDelta;
        if (size.x >= maxWidth)
        {
            control.enabled = false;
            size.x = maxWidth;
            rect.sizeDelta = size;
        }
        else
        {
            control.enabled = true;
        }
    }

    public void OnDriven()
    {
        //Awake();

        //TextFit();
    }
}
