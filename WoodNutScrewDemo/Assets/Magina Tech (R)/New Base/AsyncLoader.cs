using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Hung.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour/*, Hieu.GameFlow.IGameplayLoad, ISceneLoader*/
{
    private static bool isInitialized = false   ;
    [field:SerializeField] public Canvas targetCanvas { get; private set; }

    [SerializeField] private UISubScreen sceneLoading;

    [SerializeField] private UISubScreen functionLoading;

    [SerializeField] private TMP_Text txtLoading;

    [SerializeField] private Image progressImg;

    //[field:SerializeField] public Hieu.GameFlow.ExecuteLayer ExecuteLayer { get; private set; }

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event Action<bool> OnVisualChanged;

    [ReadOnly, HideInEditorMode, SerializeField] UISubScreen _current;

    public void Awake()
    {
        //gameObject.SetActive(true);
        //ShowSceneLoading();
        //sceneLoading.ToggleOn();
        //functionLoading.ToggleOff();
        if (!isInitialized)
        {
            ShowSceneLoading();
            sceneLoading.ToggleOn();
            functionLoading.ToggleOff();

            isInitialized = true;
        }
    }

    public void OnGameplayLoad()
    {
        //Hide();
        
    }

    Coroutine LoadingText;

    private IEnumerator CorouLoading()
    {
        progressImg.fillAmount = 0;
        LoadingText = StartCoroutine(LoadingTxt());

        ShowLoadingProgress(0.1f);
        yield return new WaitForSeconds(0.2f);
        ShowLoadingProgress(0.2f);
        yield return new WaitForSeconds(0.2f);
        ShowLoadingProgress(0.3f);
        yield return new WaitForSeconds(0.2f);
        ShowLoadingProgress(0.4f);
        yield return new WaitForSeconds(0.4f);
        StopCoroutine(LoadingText);
        ShowLoadingProgress(0.9f);
        yield return new WaitForSeconds(0.3f);
        txtLoading.text = "COMPLETE!";
        yield return new WaitForSeconds(0.2f);
        ShowLoadingProgress(1f);
        Hide();
    }

    private void ShowLoadingProgress(float percent)
    {
        DOTween.To(() => progressImg.fillAmount, x => progressImg.fillAmount = x, percent, 0.5f);
    }

    public void Hide()
    {
        Debug.Log("Turn off loading screen");
        targetCanvas.gameObject.SetActive(false);
        isVisible = false;
    }

    public void ShowSceneLoading()
    {
        targetCanvas.gameObject.SetActive(true);
        isVisible = true;
        if (_current != null) _current.ToggleOff();
        sceneLoading.ToggleOn();
        _current = sceneLoading;
        StartCoroutine(CorouLoading());
    }

    public void RunAwait(Action longAction)
    {
        targetCanvas.gameObject.SetActive(true);
        isVisible = true;

        if (_current != null) _current.ToggleOff();
        functionLoading.ToggleOn();
        _current = functionLoading;

        StartCoroutine(WaitTask(longAction));
    }

    IEnumerator WaitTask(Action longAction)
    {       
        yield return null;
        longAction();
        yield return null;
        Hide();
    }

    IEnumerator LoadingTxt()
    {
        while (true)
        {
            txtLoading.text = "LOADING.";
            yield return new WaitForSeconds(0.2f);
            txtLoading.text = "LOADING..";
            yield return new WaitForSeconds(0.2f);
            txtLoading.text = "LOADING...";
            yield return new WaitForSeconds(0.2f);
        }
    }

}
