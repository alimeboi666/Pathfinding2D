using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pickSprite;


    [Header("Parameters")]

    [SerializeField] private float pickTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AnimatePick(bool isPick)
    {
        if (!DOTween.IsTweening(transform.parent))
        {
            OnAnimatePick(isPick);
            return;
        }

        StartCoroutine(WaitForCompletionAnim(isPick));
    }


    private IEnumerator WaitForCompletionAnim(bool isPick)
    {
        while (DOTween.IsTweening(transform.parent))
            yield return null;

        OnAnimatePick(isPick);
    }

    private void OnAnimatePick(bool isPick)
    {
        if (isPick)
        {
            spriteRenderer.sprite = pickSprite;
            transform.DOLocalMoveY(transform.parent.position.y + 1f, pickTime).SetEase(Ease.OutBack);
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
            transform.DOLocalMoveY(transform.parent.position.y, 0.5f).SetEase(Ease.OutBack);
        }
    }

}
