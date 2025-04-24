using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Hung.Core;

public class DelayAction : Singleton<DelayAction>, IComplexCast<MonoBehaviour>, IComplexCast<Action>
{
    public void Cast(MonoBehaviour target, Action callback = null)
    {
        target.gameObject.SetActive(false);

        if (callback != null) StartCoroutine(DelayToAct(callback));
        else StartCoroutine(DelayToAct(() => target.gameObject.SetActive(true)));
    }

    WaitForSeconds m_delay;
    IEnumerator DelayToAct(Action act) 
    {
        m_delay = new WaitForSeconds(Random.Range(0, 0.75f));

        yield return m_delay;

        act();
    }

    public void Precast()
    {
        
    }

    public void Cast(Action input, Action callback = null)
    {
        
    }
}
