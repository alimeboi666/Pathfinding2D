using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public enum CommonTween
{

    Yoyo,
    ThinYoyo,
    LightYoyo,
    YoyoAndShake,
    Rotate,
    MoveAndFade
}

public class CommonTweener : MonoBehaviour
{
    [SerializeField] private CommonTween tween;

    GameObject model;

    private void Awake()
    {
        if (TryGetComponent(out IModel modeler))
        {
            model = modeler.Model;
        }
        else
        {
            model = gameObject;
        }
    }

 

    float shakeDuration = 1f;
    float shakeStrength = 10f;
    Vector2 _originalPosition;
    Vector3 _originalScale;


    private void OnEnable()
    {
        switch (tween)
        {
            case CommonTween.Yoyo:
                model.transform.localScale = Vector3.one;
                model.transform.DOScale(1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;

            case CommonTween.ThinYoyo:
                model.transform.localScale = Vector3.one;
                model.transform.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;
            case CommonTween.LightYoyo:
                model.transform.localScale = Vector3.one;
                model.transform.DOScale(1.5f, 0.35f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                break;

            case CommonTween.Rotate:
                model.transform.DOLocalRotate(Vector3.forward * 360, 1.5f, RotateMode.FastBeyond360)
                    .SetUpdate(true).SetLoops(-1).SetEase(Ease.Linear);
                break;
            case CommonTween.MoveAndFade:
                model.transform.localScale = Vector3.one;
                _originalPosition = model.transform.position;

                Sequence moveAndFadeSequence = DOTween.Sequence();
                moveAndFadeSequence.Append(
                    model.transform.DOMoveY(_originalPosition.y + 2f, 1.5f)
                        .SetEase(Ease.Linear)
                )
                .Join(
                    model.GetComponent<SpriteRenderer>().DOFade(0f, 1.5f)
                        .SetEase(Ease.Linear)
                )
                .Append(
                    model.transform.DOMove(_originalPosition, 0f)
                )
                .Join(
                    model.GetComponent<SpriteRenderer>().DOFade(1f, 0.1f)
                )
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true)
                .SetDelay(0);
                break;


        }
    }



    private void OnDisable()
    {
        model.transform.DOKill();
    }
}
