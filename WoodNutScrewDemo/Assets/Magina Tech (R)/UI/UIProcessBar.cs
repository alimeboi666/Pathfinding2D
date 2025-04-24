using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class UIProcessBar : MonoBehaviour, IToggleable
{
    [SerializeField] RectTransform m_rect;
    [SerializeField] TMP_Text m_processText;
    [field:SerializeField] public float TotalDuration { get; set; }
    [SerializeField] private AnimationCurve curve;
    [Range(0, 20)][SerializeField] private int changeStep;
    //[OnValueChanged("ShowProcess"), Range(0, 1), SerializeField] private float process;

    private Vector2 _anchorMax;
    private Vector2 _anchorPos;

    public float percent => m_rect.anchorMax.x;

    [field:SerializeField] public bool isVisible { get; private set; }

    private void Awake()
    {
        _anchorMax = m_rect.anchorMax;
        _anchorPos = m_rect.anchoredPosition;
    }

    //void ShowProcess()
    //{
    //    InstantSet(process);
    //}

    public void InstantSet(float percentValue)
    {
        _anchorMax.x = Mathf.Clamp01(percentValue);
        m_rect.anchorMax = _anchorMax;
        m_rect.anchoredPosition = _anchorPos;
        if (m_processText != null) m_processText.text = (int)(percentValue * 100) + "%";
    }

    public void VisualSet(float toPercentValue, Action endAction = null)
    {
        //_anchorMax.x = Mathf.Clamp01(toPercentValue);
        //m_rect.DOAnchorMax(_anchorMax, duration).SetEase(curve).OnUpdate(() =>
        //{
        //    m_rect.anchoredPosition = _anchorPos;
        //    if (m_processText != null) m_processText.text = (int)(m_rect.anchorMax.x * 100) + "%";
        //}).OnComplete(() => 
        //{
        //    if (endAction != null) endAction();
        //});

        StartCoroutine(ProcessTo(toPercentValue, endAction));
    }

    IEnumerator ProcessTo(float toPercent, Action callback = null)
    {
        float timer = TotalDuration * (percent - toPercent);
        //Debug.Log(timer + " " + toPercent);
        while( timer > 0 && percent > toPercent)
        {
            //Debug.Log(timer + ": " + percent);
            InstantSet(percent - Time.deltaTime/timer);
            timer-= Time.deltaTime;    
            yield return null;
        }
        callback?.Invoke();
    }

    public void VisualChange(float signedChangedPercent)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ProcessChanging(signedChangedPercent));
        }
        
    }

    WaitForFixedUpdate delta = new WaitForFixedUpdate();

    public event Action<bool> OnVisualChanged;

    IEnumerator ProcessChanging(float signedChangedPercent)
    {
        int timer = changeStep;
        float deltaPercent = signedChangedPercent / timer;
        //Debug.Log("delta: " + deltaPercent); 
        Vector2 anchorMax;
        while (timer-- > 0)
        {
            anchorMax = m_rect.anchorMax;
            //Debug.Log("anchor: " + anchorMax.x);
            anchorMax.x = Mathf.Clamp01(anchorMax.x + deltaPercent);
            m_rect.anchorMax = anchorMax;
            m_rect.anchoredPosition = _anchorPos;
            yield return delta;
        }
        
    }

    public void ToggleOn()
    {
        isVisible = true;
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }
}
