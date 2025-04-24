using Hung.Pooling;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : WorldUI
{
    [SerializeField] TextMeshProUGUI m_text;
    [SerializeField] private float appearTime;
    [SerializeField] private float flyingSpeed;

    public void ShowAt(string content, Vector3 position, Vector2 viewOffset = default, Color color = default)
    {
        m_text.text = content;
        if (color != default) m_text.color = color;
        ToggleOn();
        StartCoroutine(FollowAndFly(position, viewOffset));
    }


    IEnumerator FollowAndFly(Vector3 position, Vector2 viewOffset)
    {
        float _timer = appearTime;
        Offset = viewOffset;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            Offset += Time.deltaTime * Vector3.up * flyingSpeed;
            LocateAt(position);

            yield return null;
        }
        ToggleOff();
        Pool.BackToPool(this);
    }

    public override void ToggleOn()
    {
        gameObject.SetActive(true);
    }

    public override void ToggleOff()
    {
        gameObject.SetActive(false);
    }
}
