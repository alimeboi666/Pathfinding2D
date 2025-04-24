/// <author>
/// Created by Hung Phan on June 27, 2024, as part of the Magina Tech Framework.
/// This class was developed to provide a flexible and dynamic way to manage resource values within the game.
/// </author>
/// <right>
/// Magina Tech Framework
/// All rights reserved. Unauthorized use or redistribution of this code is prohibited.
/// </right>

using Hung.Core;
using Hung.GameData;
using Hung.Pooling;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ValueSign
{
    Negative = -1,
    Depend = 0,
    Positive = 1
}
/// <summary>
/// Manages and displays resource values in the UI, including updates, visibility, and handling ads-based alternatives.
/// </summary>
public class ResourceValue : SerializedMonoBehaviour, IDataHolder<GameResource>, IToggleable, INotiCast, ICache
{
    [SerializeField] private HorizontalOrVerticalLayoutGroup layout;
    [field: SerializeField] public bool showName { get; set; }
    [field: SerializeField] public bool showPostFix { get; private set; }
    [field: SerializeField] public bool showUnitValue { get; private set; }

    [field: SerializeField] public bool showAdsWarning { get; private set; }

    [field: SerializeField] public bool showEnoughColor { get; private set; }

    [ShowIf("showEnoughColor"), SerializeField] private Color colorEnough, colorNotEnough;

    [SerializeField] private ValueSign showingSign;

    [Range(0.25f, 1), SerializeField] private float iconSize = 1;

    [SerializeField] private bool standaloneIcon;
    [ShowIf("standaloneIcon"), Range(1, 2), SerializeField] private float standaloneIconSizeScale = 2;

    [SerializeField] private Image icon;
    [SerializeField] private Image adsIcon;
    [SerializeField] private TMP_Text txtValue;
    [SerializeField] private TMP_Text textName;
    [field: ReadOnly, SerializeField] public GameResource info { get; private set; }

    [ReadOnly, SerializeField] bool _hasNotify;

    [field: ReadOnly, SerializeField] public bool isEnough { get; private set; }
    [field: ReadOnly, SerializeField] public bool adsMode { get; private set; }
    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    [ReadOnly, SerializeReference] IResource resource;

    bool NotInitValue => Application.isPlaying && !hasValue;
    [InfoBox("This resource hasn't been given value yet! Call SetValue() function to pass this error.", InfoMessageType.Error, "NotInitValue")]
    [SerializeField] private UnityEvent onEnough;
    [SerializeField] private UnityEvent onLess;

    private Action onAdsRewarded;

    event Action<bool> _notify;
    public event Action<bool> OnVisualChanged;

    public bool hasNotify
    {
        get
        {
            CheckEnough(resource.value);
            return _hasNotify;
        }
    }

    public event Action<bool> Notify
    {
        add
        {
            value(hasNotify);
            _notify += value;
        }

        remove
        {
            _notify -= value;
        }
    }

    public void OnCached()
    {

    }

    private void Awake()
    {
        adsIcon.sprite = DataCollection.GetAdsResource().Icon;
        Archetype.AdsIntegration.OnAdsRewardShowChanged += (bool show) =>
        {
            if (!show)
            {
                adsIcon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
            }
            else if (adsMode)
            {
                adsIcon.gameObject.SetActive(true);
                icon.gameObject.SetActive(false);
            }
        };

        //onEnough.AddListener(() => PostNoti(true));
        //onLess.AddListener(() => PostNoti(false));
    }

    public void PostNoti(bool isOn)
    {
        _notify?.Invoke(isOn);
        _hasNotify = isOn;
    }

    private void OnValidate()
    {
        icon.transform.localScale = Vector3.one * iconSize * (standaloneIcon ? standaloneIconSizeScale : 1);
        //    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    public void SetStandaloneText(string content)
    {
        if (info != null) info.RemoveValueChange(CheckEnough);
        txtValue.text = content;
        icon.gameObject.SetActive(false);
    }

    public IResource GetValue() => resource;

    /// <summary>
    /// Updates the displayed value of the resource and adjusts the UI accordingly.
    /// </summary>
    /// <param name="value">The resource value to be displayed.</param>
    public void UpdateValue(EnormousValue value)
    {
        value.Normalize();
        resource.value = value;

        string valueStr = "";
        if (showingSign == 0)
        {
            valueStr = value >= 0 ? "+" + value : value.ToString();
        }
        else
        {
            valueStr = (int)showingSign * EnormousValue.Abs(value) + "";
        }
        txtValue.text = valueStr + (showPostFix ? resource.postFix : "");

        CheckTextValueState(value);

        //if (iconSize < 0) iconSize = icon.rectTransform.localScale.x;
        if (standaloneIcon)
        {
            if (!txtValue.gameObject.activeSelf && !textName.gameObject.activeSelf)
            {
                icon.rectTransform.localScale = standaloneIconSizeScale * iconSize * Vector3.one;
            }
            else
            {
                icon.rectTransform.localScale = iconSize * Vector3.one;
            }
        }

    }
    /// <summary>
    /// Sets the current resource and updates UI elements such as the icon, name, and value.
    /// </summary>
    /// <param name="resource">The resource to be set.</param>
    bool hasValue;
    [Button]
    public void SetValue(IResource resource)
    {
        if (resource == null)
        {
            gameObject.SetActive(false);
            return;
        }
        hasValue = Application.isPlaying;
        gameObject.SetActive(true);

        if (info != null) info.RemoveValueChange(CheckEnough);
        info = DataCollection.GetResourceData(resource);
        this.resource = resource;
        icon.sprite = info.Icon;

        UpdateValue(resource.value);

        if (showName)
        {
            textName.gameObject.SetActive(true);
            textName.text = info.Name + ":";
        }
        else
        {
            textName.gameObject.SetActive(false);
        }

        info.AddAndCheckValueChange(CheckEnough);

    }

    /// <summary>
    /// Checks whether the resource is sufficient to afford the cost.
    /// </summary>
    /// <returns>Returns true if the resource is sufficient or if an ad can be used as an alternative.</returns>
    public bool AttempToAfford()
    {
        if (info == null) return true;
        return info.AttempToConsume(resource);
    }

    /// <summary>
    /// Attempts to consume the resource and executes the success action if successful, or displays an ad if the resource is insufficient.
    /// </summary>
    /// <param name="onSuccess">Action to be executed when the transaction is successful.</param>
    /// <returns>Returns true if the transaction is successful or if an ad is successfully displayed.</returns>
    public bool TryToAfford(Action onSuccess)
    {
        if (info == null)
        {
            onSuccess?.Invoke();
            return true;
        }
        else if (info.TryToConsume(resource))
        {
            if (info is CurrencyData currency)
            {
                if (currency.spendVFX != null) Pool.Spawn(currency.spendVFX, transform.position);
                //if (currency.soundPurchase != null) SingletonRole.MasterSound.PlayUISound(currency.soundPurchase);
            }
            onSuccess?.Invoke();
            return true;
        }
        else
        {
            //Debug.Log("Check Ads Mode");
            if (adsMode && Archetype.AdsIntegration.canShowAd)
            {
                onSuccess += onAdsRewarded;
                if (!showAdsWarning)
                {
                    if (resource.adReplaceableType == AdvertimentClickType.RewardedInterstitial)
                        Archetype.AdsIntegration.TryToShowRewardedInterstitialAd(onSuccess);
                    else Archetype.AdsIntegration.TryToShowRewardedAd(onSuccess);
                }
                else
                {
                    Archetype.UIManager.ShowHelper("Do you want to watch an ad to get this reward?", "Warning", () =>
                    {
                        if (resource.adReplaceableType == AdvertimentClickType.RewardedInterstitial)
                            Archetype.AdsIntegration.TryToShowRewardedInterstitialAd(onSuccess);
                        else Archetype.AdsIntegration.TryToShowRewardedAd(onSuccess);
                    });
                }
            }
            return adsMode && Archetype.AdsIntegration.canShowAd;
        }
    }

    /// <summary>
    /// Displays a visual effect representing the collection of resources.
    /// </summary>
    public void VisualClaim()
    {
        if (info == null) return;
        CommonPool.Spawn(out ResourceClaim claimer, transform);
        //var claimer = Pool.Spawn(GameController.Instance.resourceClaim, transform);
        claimer.transform.SetParent(transform.root);
        claimer.transform.SetAsLastSibling();
        claimer.Drop(transform.position, resource);
    }

    [Button]
    public void ForceCheckEnough()
    {
        if (info != null)
        {
            CheckEnough(info.Value);
        }
    }

    public void ForceAds(Action bonusReward, string valueDescription = "")
    {
        var formerResource = resource;
        void OnForceAdsRewarded()
        {
            SetValue(formerResource);
            bonusReward();
            onAdsRewarded = null;
        }
        SetValue(new AdsOnly());
        if (true)
        {
            txtValue.text = valueDescription;
            txtValue.gameObject.SetActive(true);
        }

        onAdsRewarded = OnForceAdsRewarded;
    }

    public void SetReverse(bool reverse)
    {
        if (!layout.enabled) Debug.LogError($"layout is disabled, please check");
        layout.reverseArrangement = reverse;
    }

    /// <summary>
    /// Checks if the current resource value is sufficient and adjusts the UI and status accordingly.
    /// </summary>
    /// <param name="current">The current resource value to check against.</param>
    void CheckEnough(EnormousValue current)
    {
        //if(gameObject.tag == "Testing") Debug.Log("Check: " + resource.value + "/" + current); 
        //if (!gameObject.activeSelf) return;
        //Debug.Log(current + " == " + resource.value); 
        if (info == null || info.AttempToConsume(resource))
        {
            //if (gameObject.tag == "Testing") Debug.Log("Enough: " + (current + resource.value)); 
            isEnough = true;
            onEnough?.Invoke();
            if (info == null || info.Represent.noticeable) PostNoti(true);
            if (showEnoughColor) txtValue.color = colorEnough;
            adsMode = false;
            //CheckTextValueState(current);
        }
        else
        {
            //if (gameObject.tag == "Testing") Debug.Log("Not Enough: " + (current + resource.value));
            if (info.Represent.adReplaceableType != AdvertimentClickType.None)
            {
                if (Archetype.AdsIntegration.canShowAd)
                {
                    onEnough?.Invoke();
                    PostNoti(false);
                    if (showEnoughColor) txtValue.color = colorEnough;
                }

                //Debug.Log("Check Ads");
                adsMode = true;
                //txtValue.gameObject.SetActive(false);
            }
            else
            {
                adsMode = false;
                //CheckTextValueState(current);
                isEnough = false;
                onLess?.Invoke();
                PostNoti(false);
                if (showEnoughColor) txtValue.color = colorNotEnough;
            }
        }
        if (adsMode && Archetype.AdsIntegration.canShowAd)
        {
            icon.gameObject.SetActive(false);
            adsIcon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(true);
            adsIcon.gameObject.SetActive(false);
        }
    }

    void CheckTextValueState(EnormousValue value)
    {
        //Debug.Log(value);
        if (showUnitValue)
        {
            txtValue.gameObject.SetActive(value != 0);
        }
        else
        {
            txtValue.gameObject.SetActive(value != 0 && EnormousValue.Abs(value) != 1);
        }
        //txtValue.gameObject.SetActive(true);
    }

    public void ToggleOn()
    {
        isVisible = true;
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }

    //private void OnEnable()
    //{
    //    if (targetResource != null) targetResource.AddAndCheckListener(CheckEnough);
    //}

    //private void OnDisable()
    //{
    //    //if (targetResource != null) targetResource.RemoveListener(CheckEnough);
    //    //PostNoti(isEnough);
    //}
}
