using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Hung.UI
{
    public class UI_LateContent : MonoBehaviour, IModel
    {
        [field:SerializeField] public GameObject Model { get; private set; }

        [SerializeField] private float delayTime;

        private void OnEnable()
        {
            StartCoroutine(WaitToShow());
        }

        IEnumerator WaitToShow()
        {
            Model.SetActive(false);
            Model.transform.localScale = Vector3.zero;
            yield return new WaitForSecondsRealtime(delayTime);
            Model.SetActive(true);
            Model.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}

