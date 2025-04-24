using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

//public interface IVerify<T>
//{
//    void Verify(IEnumerable<T> obj);
//}

//public class NotiVerify : IVerify<INoti>
//{
//    public void Verify(IEnumerable<INoti> noti)
//    {
        
//    }
//}

public class UINotification : SerializedMonoBehaviour, INotiReceive, ICache, IToggleable
{
    [SerializeField] bool isManual;
    [field: ShowIf("showProperty"), SerializeField] public RectTransform redNoti { get; private set; }
    [field: ShowIf("showProperty"), NonSerialized, OdinSerialize] public List<INotiCast> checkers { get; private set; }
    [HideInEditorMode, ReadOnly, ShowInInspector] Dictionary<INotiCast, bool> _dict;
    bool showProperty => isManual || Application.isPlaying;

    public void OnCached()
    {
        checkers = GetComponentsInChildren<INotiCast>(includeInactive: true).Where(noti => noti.gameObject.tag != "Ignore Noti").ToList();
        //if (verify != null) verify.Verify(crawl);
        //checkers = crawl.ToDictionary(item => item, item => false);  
    }

    public void AddChecker(INotiCast noti)
    {
        if (!hasDict)
        {
            hasDict = true;
            DictAll();
        }
        checkers.Add(noti);
        _dict.Add(noti, false);
        if (hasRegister) noti.Notify += (bool hasNotify) => SetNotify(noti, hasNotify);
    }

    bool hasDict;
    bool hasRegister;

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event Action<bool> OnVisualChanged;

    void Awake()
    {
        if (!hasDict)
        {
            hasDict = true;
            DictAll();
        }

        if (!hasRegister)
        {
            hasRegister = true;
            ShowNoti(false);
            RegisterAll();
        }       
    }

    void DictAll()
    {
        if (checkers == null)
        {
            checkers = new();
            _dict = new();
            return;
        }
        _dict = checkers.ToDictionary(noti => noti, noti => false);
    }

    void RegisterAll()
    {
        if (_dict == null) return;
        foreach (var checker in _dict.ToList())
        {
            checker.Key.Notify += (bool hasNotify) => SetNotify(checker.Key, hasNotify);
        }
    }

    void SetNotify(INotiCast noti, bool hasNotify)
    {
        if (!_dict.ContainsKey(noti)) return;
        _dict[noti] = hasNotify;
        if (hasNotify && CheckVisible(noti.transform)) 
        {
            ShowNoti(true);
            return;
        }
        ForceCheck();
    }

    bool CheckVisible(Transform element)
    {
        var parent = element;
        while (parent != null && parent != transform)
        {
            if (!parent.gameObject.activeSelf) return false;
            parent = parent.parent;
        }
        return true;
    }

    void ShowNoti(bool isOn)
    {
        if (redNoti != null)
        redNoti.gameObject.SetActive(isOn);
    }

    [Button]
    public void ForceCheck()
    {
        if (Application.isPlaying) Awake();
        foreach (var checker in _dict)
        {
            if (checker.Value && CheckVisible(checker.Key.transform))
            {
                ShowNoti(true);
                return;
            }
        }
        ShowNoti(false);
    }

    public void ToggleOn()
    {
        redNoti.GetComponent<Image>().enabled = true;
    }

    public void ToggleOff()
    {
        redNoti.GetComponent<Image>().enabled = false;
    }
}
