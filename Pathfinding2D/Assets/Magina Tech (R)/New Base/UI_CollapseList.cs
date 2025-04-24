using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;

public class UI_CollapseList : MonoBehaviour
{
    [ReadOnly, SerializeField] private bool isCollapsing;
    [SerializeField] private float cellSize;
    [SerializeField] private float spacing;
    [SerializeField] private float collapseTime;
    [SerializeField] private float maxSpeed;
    [Range(0.0001f, 1f)] [SerializeField] private float smoothTime;

    [SerializeField] private AnimationCurve popEase;
    [SerializeField] private List<UI_LinkedItem> m_list;

    [ReadOnly, SerializeField] private List<UI_LinkedItem> m_wait;
    [ReadOnly, SerializeField] private List<UI_LinkedItem> m_put;
    
    float paddingTop => cellSize / 2 + spacing;

    public event Action<int> OnListChange;

    private void OnEnable()
    {
        if (!isCollapsing) return;

        isCollapsing = false;
        foreach (var item in m_list)
        {
            var aboveY = item.above != null ? item.above.rect.anchoredPosition : paddingTop * Vector2.up;
            aboveY.x = 0;
            item.rect.anchoredPosition = aboveY - (cellSize + spacing) * Vector2.up;
        }

        foreach (var item in m_put)
        {
            PutItemAtLast(item);
            PopItem(item);
        }
        ClearLastState();
    }

    private void OnDisable()
    {
        if (m_put.Count > 0)
        {
            foreach (var item in m_put)
            {
                item.ToggleOff();
            }
            OnListChange?.Invoke(m_list.Count);
        }      
    }

    public void ClearLastState()
    {
        m_put.Clear();
        m_wait.Clear();
    }

    public void SetList(List<UI_LinkedItem> list)
    {
        isCollapsing = false;
        m_list = list;
        for (int i = 0; i < m_list.Count; i++)
        {
            m_list[i].above = (i > 0) ? m_list[i - 1] : null;

            m_list[i].below = (i < m_list.Count - 1) ? m_list[i + 1] : null;

            m_list[i].name = "Upgrade Item: " + (i + 1);
        }
        OnListChange?.Invoke(m_list.Count);
    }

    public void OnCached()
    {
        m_list = transform.GetComponentsInChildren<UI_LinkedItem>(includeInactive: true).ToList();
        for (int i = 0; i < m_list.Count; i++)
        {
            m_list[i].above = (i > 0) ? m_list[i - 1] : null;

            m_list[i].below = (i < m_list.Count - 1) ? m_list[i + 1] : null;

            m_list[i].name = "Upgrade Item: " + (i + 1);
        }
    }

    public void StartDredge(UI_LinkedItem item)
    {
        m_list.Remove(item);
        m_put.Add(item);
        m_wait.Add(item);
        if (item.above != null)
        {
            item.above.below = item.below;
        }
        if (item.below != null) 
        {
            item.below.above = item.above;
        }        
    }

    public void DoneDredge(UI_LinkedItem item)
    {
        m_wait.Remove(item);
    }

    public void TryToCollapse()
    {
        if (!isCollapsing && m_wait.Count == 0)
        {
            isCollapsing = true;
            if (gameObject.activeInHierarchy) StartCoroutine(Collapsing());
        }
    }

    Vector2 _v;
    IEnumerator Collapsing()
    {
        var timer = collapseTime;
        while (timer > 0)
        {
            yield return null;

            foreach (var item in m_list)
            {
                var aboveY = item.above != null ? item.above.rect.anchoredPosition : paddingTop * Vector2.up;
                aboveY.x = 0;
                item.rect.anchoredPosition = Vector2.SmoothDamp(item.rect.anchoredPosition, aboveY - (cellSize + spacing) * Vector2.up, ref _v, smoothTime);
            }

            timer -= Time.deltaTime;
        }
        isCollapsing = false;
        foreach(var item in m_put)
        {
            PutItemAtLast(item);
            //PopItem(item);
        }
        m_put.Clear();
    }

    void PutItemAtLast(UI_LinkedItem item)
    {
        item.below = null;
        if (m_list.Count > 0)
        {
            item.above = m_list.Last();
            m_list.Last().below = item;
        }
        else
        {
            item.above = null;
        }
        
        item.transform.SetSiblingIndex(m_list.Count + m_put.Count - 1);
        //m_list.Add(item);
        item.rect.anchoredPosition = (m_list.Count * -(cellSize + spacing) + paddingTop) * Vector2.up;
        item.ToggleOff();

        OnListChange?.Invoke(m_list.Count);
    }

    void PopItem(UI_LinkedItem item)
    {
        item.transform.localScale = Vector3.zero;
        item.transform.DOScale(1, 0.2f).SetEase(popEase);
    }
}
