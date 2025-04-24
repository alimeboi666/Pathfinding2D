using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreen_Obsolete : MonoBehaviour
{
    [SerializeField] private bool isPopUp;
    [SerializeField] private Image bgMask;
    //[SerializeField] private VisualBehaviour behaviour;
    [SerializeField] private List<RectTransform> wipedList;
    [SerializeField] private List<Vector2> wipedPosList;
    [SerializeField] private List<Transform> poppedList;
    [SerializeField] private UnityEvent openingEvent, openedEvent, closedEvent;
    [SerializeField] private float popStepTime = 0.25f;

    public Image BgMask { get => bgMask; }
    public List<RectTransform> WipedList { get => wipedList; }
    public List<Transform> PoppedList { get => poppedList; }
    public List<Vector2> WipedPosList { get => wipedPosList; }
    public float PopStepTime { get => popStepTime; }
    public UnityEvent ClosedEvent { get => closedEvent; set => closedEvent = value; }
    public UnityEvent OpenedEvent { get => openedEvent; set => openedEvent = value; }
    public UnityEvent OpeningEvent { get => openingEvent; set => openingEvent = value; }
    public bool IsPopUp { get => isPopUp; set => isPopUp = value; }

    //public VisualBehaviour Behaviour { get => behaviour; }

    public void OnGameStart()
    {
        for (int i = 0; i < wipedList.Count; i++)
        {
            wipedList[i].anchoredPosition = wipedPosList[i];
        }
        foreach (Transform popTransform in poppedList)
        {
            popTransform.localScale = Vector3.zero;
        }

        gameObject.SetActive(true);
        gameObject.SetActive(false);
        if (isPopUp)
        {
            transform.localScale = Vector3.zero;
            if (bgMask != null) bgMask.color = Color.clear;
        }
    }

    public void OnStartOpening()
    {
        gameObject.SetActive(true);

        if (openingEvent != null) openingEvent.Invoke();

        foreach (Transform pop in poppedList)
        {
            pop.DOKill(true);
            pop.localScale = Vector3.zero;
        }
    }

    public void OnOpened()
    {
        if (openedEvent != null) openedEvent.Invoke();
    }

    public void OnClosed()
    {
        gameObject.SetActive(false);

        if (closedEvent != null) closedEvent.Invoke();
    }

    public void MaskBack()
    {
        if (bgMask != null)
        {
            bgMask.transform.SetParent(transform);
        }
    }
}