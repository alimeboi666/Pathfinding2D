using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Counting : MonoBehaviour
{
    [SerializeField] private Text textNumber;
    [SerializeField] private CountingStep countingType;
    [SerializeField] private TextFormat format;
    [SerializeField] private int startValue, endValue;
    [SerializeField] private UnityEvent endEvent;
    [SerializeField] private bool selfShutdown;
    [SerializeField] private string fixedString;

    private Action<int> textShow;
    private object delay;
    private float step;
    private bool pauseSignal;

    internal int currentValue;

    private void OnEnable()
    {
        int stepValue;
        if (countingType != CountingStep.Dynamic)
        {
            stepValue = Math.Sign(startValue - endValue);
            StartCoroutine(Count(startValue, endValue, stepValue));
        }
    }

    public void Pause()
    {
        pauseSignal = true;
        delay = new WaitUntil(() => !pauseSignal);
    }

    public void Resume()
    {
        //Debug.Log("Kekw"); 
        pauseSignal = false;
        if (countingType == CountingStep.RealTime)
        {
            delay = new WaitForSecondsRealtime(Mathf.Abs(step));
        }
        else if (countingType == CountingStep.TimeScaler)
        {
            delay = new WaitForSeconds(Mathf.Abs(step));
        }
    }

    private IEnumerator Count(int startValue, int endValue, int step)
    {
        //Debug.Log("Pog"); 
        this.step = step;
        delay = new object();
        Resume();

        if (format == TextFormat.Simple)
        {
            textShow = delegate (int n)
            {
                textNumber.text = n.ToString() + fixedString;
            };
        }
        else if (format == TextFormat.MM_SS)
        {
            textShow = delegate (int n)
            {
                textNumber.text = TimeSpan.FromSeconds(n).ToString("mm':'ss");
            };
        }

        currentValue = startValue;
        while (pauseSignal || Math.Sign(currentValue - endValue) == Math.Sign(step))
        {
            if (pauseSignal)
            {
                goto end;
            }
            textShow(currentValue);
            currentValue -= step;

            if (pauseSignal)
            {
                currentValue += step;
                goto end;
            }
        //if (progress != null) progress.Reset(1);            
        end:
            {

            }
            yield return delay;
        }
        if (selfShutdown) gameObject.SetActive(false);
        endEvent.Invoke();
    }
    public void Cast(UnityAction endCallback = null)
    {
        gameObject.SetActive(true);
        endEvent.AddListener(endCallback);
    }

    public enum CountingStep
    {
        RealTime,
        TimeScaler,
        Dynamic
    }

    public enum TextFormat
    {
        Simple,
        MM_SS
    }
}