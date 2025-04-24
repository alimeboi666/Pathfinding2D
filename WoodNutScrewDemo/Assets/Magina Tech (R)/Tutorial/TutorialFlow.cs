using System;
using System.Collections;
using Hung.Core;
using Hung.GameData;
using Hung.Pooling;
using Hung.UI;
using Hung.UI.Button;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IClickable: IMono
{
    event Action OnFirstClick;
}

public abstract class TutorialExecute : ICore
{
    private bool signal;

    public void Init()
    {

    }

    public abstract IEnumerator Execute();

    public virtual void OnPass(ITutorial handler)
    {

    }

    protected bool Mask
    {
        get => Archetype.Tutorial.GetMask();

        set => Archetype.Tutorial.SetMask(value);
    }

    protected bool BlockAll
    {
        set => Archetype.Tutorial.SetBlockAll(value);
    }

    public ScreenMaskOpacity MaskOpacity
    {
        set => Archetype.Tutorial.mask.SetOpacity(value);
    }

    protected ObjectFollower SpawnIndicator(Transform transform)
    {
        return Archetype.Tutorial.ShowIndicatorAt(transform);
    }

    protected void TravelRect<T>(T component) where T : Component
    {
        Archetype.Tutorial.TravelRect(component.transform);
    }

    protected void TravelBack<T>(T component) where T: Component
    {
        Archetype.Tutorial.TravelBack(component.transform);
    }

    protected void TravelToTarget<T>(T component1, T component2) where T : Component
    {
        Archetype.Tutorial.TravelToTarget(component1.transform, component2.transform);
    }



    protected WaitUntil WaitToClick(IClickable clickable, bool hasTravel = false, bool showIndicator = true, Action firstClickAction = null, Vector2 indicatorOffset = default)
    {
        signal = false;
        if (firstClickAction != null)
        {
            Debug.Log("Before Click");
            clickable.OnFirstClick += firstClickAction;
        }

        if (hasTravel && clickable.transform is RectTransform rect)
        {
            TravelRect(rect);
            clickable.OnFirstClick += () => TravelBack(rect);
        }
        clickable.OnFirstClick += ReleaseSignal;
        if (showIndicator)
        {
            var hand = Archetype.Tutorial.ShowIndicatorAt(clickable.transform);
            hand.Offset = indicatorOffset;
            clickable.OnFirstClick += () =>
            {
                hand.BackToPool();
            };
        }
      
        return WaitForSignal();
    }


    protected WaitUntil WaitToClick2(Transform current, Transform target, IClickable clickable, bool hasTravel = false, bool showIndicator = true, Action firstClickAction = null, Vector2 indicatorOffset = default)
    {
        signal = false;
        if (firstClickAction != null)
        {
            Debug.Log("Before Click");
            clickable.OnFirstClick += firstClickAction;
        }

        if (hasTravel)
        {
            TravelToTarget(current, target);
            clickable.OnFirstClick += () => TravelBack(current); 
        }
        clickable.OnFirstClick += ReleaseSignal;
        if (showIndicator)
        {
            var hand = Archetype.Tutorial.ShowIndicatorAt(clickable.transform);
            hand.Offset = indicatorOffset;
            clickable.OnFirstClick += () =>
            {
                hand.BackToPool();
            };
        }

        return WaitForSignal();
    }

   


    protected WaitForSeconds WaitForSeconds(float seconds)
    {
        return new WaitForSeconds(seconds);
    }

    protected void ReleaseSignal()
    {
        signal = true;
    }

    protected WaitUntil WaitForSignal()
    {
        signal = false;
        return new WaitUntil(() => signal);
    }

    protected WaitUntil WaitUntil(Func<bool> precate)
    {        
        return new WaitUntil(precate);
    }

    protected WaitUntil WaitForSystemFree(Func<bool> addition = null)
    {
        if (addition == null) return new WaitUntil(() => Archetype.isAllFree);
        else
        {
            WaitUntil waiter = new WaitUntil(() => Archetype.isAllFree && addition());
            return waiter;
        }

    }
}

[CreateAssetMenu(menuName = "Hung/Tutorial/Tutorial Data")]
public sealed class TutorialFlow : SerializedScriptableObject
{
    [SerializeField] private TutorialExecute Core;

    public event Action OnPassThrough;
    
    public IEnumerator Execute()
    {
        //var setting_screen = Archetype.ScreenManager.GetScreen<UIScreen_Setting>();

        WaitUntil systemFree = new WaitUntil(() => Archetype.isAllFree);

        if (PlayerPrefs.GetInt(name, 0) == 0) 
        {
            yield return systemFree;
            Debug.Log($"[TUTORIAL] {name} STARTED TO CAST");
         
            var execute = Core.Execute();
            int step = 0;

            while (execute.MoveNext())
            {
                
                Debug.Log($"[TUTORIAL] {name} Step: " + step++);
                yield return execute.Current;
            }
            Archetype.Tutorial.SetMask(false);
            PlayerPrefs.SetInt(name, 1);
            Debug.Log("Remove Runtime Config: " + name);
            OnPassThrough?.Invoke();
            Core.OnPass(Archetype.Tutorial);
        }
        else
        {
            OnPassThrough?.Invoke();
            Core.OnPass(Archetype.Tutorial);
        }        
    }
}

