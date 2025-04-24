using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Hung.Pooling;
using Hung.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Tutorial : SerializedMonoBehaviour, ITutorial, IGameplayStart
{
    [field:SerializeField] public ExecuteLayer ExecuteLayer { get; private set; }

    public Canvas targetCanvas { get; private set; }

    [ReadOnly, SerializeField] int castingIndex;

    [field:SerializeField] public List<TutorialFlow> castings { get; private set; }

    [field: NonSerialized, OdinSerialize] public List<IConfig> configs { get; private set; } 

    [field: SerializeField] public UI_ScreenMask mask { get; private set; }
    [SerializeField] public UI_ScreenMask block;

    [SerializeField] private UISubScreen screen;

    [SerializeField] private ObjectFollower handIndicator;

    public event Action TutorialConfiguration;


    private void Awake()
    {
        targetCanvas = GetComponent<Canvas>();

        SetMask(false);
        handIndicator.ToggleOff();
        SetBlockAll(false);
    }


    public void OnGameplayStart()
    {
        Debug.Log("Start TUTORIAL");
        //Archetype.SceneLoader.ToggleOn();
        foreach (var config in configs)
        {
            config.Config();
            config.AssignConfig(ref TutorialConfiguration);
        }

        //Debug.Log("CASTING" + castings.Count);

        StartCoroutine(Casting());

        TutorialConfiguration?.Invoke();
        //Archetype.SceneLoader.ToggleOff();
    }

    IEnumerator Casting()
    {
        for(castingIndex = 0; castingIndex < castings.Count; castingIndex++)
        {
            var casting = castings[castingIndex];
            var execute = casting.Execute();
            while (execute.MoveNext())
            {
                yield return execute.Current;
            }
            //Debug.Log("Reload Config: " + casting.name);
            //TutorialConfiguration?.Invoke();
        }
        castingIndex++;
    }

    public bool CheckPass(TutorialFlow flow)
    {
        if (!castings.Contains(flow)) return true;
        else
        {
            return castings.IndexOf(flow) < castingIndex;
        }
    }

    public ObjectFollower ShowIndicatorAt(Transform transform)
    {
        var hand = Pool.Spawn(handIndicator);
        hand.ToggleOn();
        //hand.SetTarget(transform);
        hand.target = transform;
        
        //Debug.Log("Show Hand Indicator");
        return hand;
    }

    [Button]
    public void TravelRect(Transform transform)
    {
        RectTransform rect = transform as RectTransform;
        
        if (rect.TryGetComponent(out ICanvasTraveller traveller))
        {
            traveller.OnTraveled();
            
        }

        rect.SetParent(screen.transform, true);
    }

    public void TravelBack(Transform transform)
    {

        if (transform.root != targetCanvas.transform) return;
        Debug.Log("Try to travel back: " + transform.name); 
        if (transform.TryGetComponent(out ICanvasTraveller traveller))
        {
            traveller.OnTravelBack();
        }
    }

    public void SetBlockAll(bool isOn)
    {
        block.gameObject.SetActive(isOn);
    }

    public void SetMask(bool isOn)
    {
        mask.gameObject.SetActive(isOn);
    }

    public bool GetMask()
    {
        return mask.gameObject.activeSelf;
    }

    public ObjectFollower ShowIndicatorToMove(Transform current, Transform target)
    {
        var hand = Pool.Spawn(handIndicator);
        hand.ToggleOn();
        //hand.SetTarget(transform);
        hand.target = transform;

        //Debug.Log("Show Hand Indicator");
        return hand;
    }

    public void TravelToTarget(Transform current, Transform target)
    {
        
    }
}

public interface ICanvasTraveller
{
    void OnTraveled();

    void OnTravelBack();
}
