using Hung.Core;
using Hung.GameData;
using Hung.Pooling;
using Magina.Coffee;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPane : MonoBehaviour
{
    [field: SerializeField] public bool showPostFix { get; private set; }
    [field:SerializeField] public bool showSign { get; private set; }

    [SerializeField] private GameResource currency;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private UIParticleAttractor attractor;

    private void Awake()
    {
        icon.sprite = currency.Icon;
    }

    private void OnEnable()
    {
        currency.AddAndCheckValueChange(OnCurrentChange);
        currency.OnVisualClaim += AssignClaimingResource;
        OnCurrentChange(currency.Value);
    }

    [ReadOnly, SerializeField] EnormousValue visualPart;
    public void ClaimEachPart()
    {
        currency.Value += visualPart;
        if (currency is CurrencyData currencyData)
        {
            Archetype.MasterSound.PlayUISound(currencyData.soundPartClaim);
        }
    }

    bool AssignClaimingResource(ResourceClaim resourceClaim, int claimTime, EnormousValue eachValue)
    {
        if (attractor.particleSystem != null)
        {
            return false;
        }
        else
        {
            attractor.SetAttraction(resourceClaim.particle, claimTime, () => Pool.BackToPool(resourceClaim));
            //attractor.particleSystem = resourceClaim.particle; 
            visualPart = eachValue;
            return true;
        }
        
    }

    private void OnDisable()
    {
        currency.RemoveValueChange(OnCurrentChange);
        currency.OnVisualClaim -= AssignClaimingResource;
        attractor.particleSystem = null;
    }

    int _last;
    void OnCurrentChange(EnormousValue current)
    {
        current.Normalize();
        currencyText.text = (showSign && current >= 0 ? "+" : "") + current.ToString("0") + (showPostFix ? currency.Represent.postFix : "");
        if (float.IsInfinity(current.Value))
        {
            currencyText.enableAutoSizing = false;
            currencyText.fontSize = 45;
        }
        else
        {
            currencyText.enableAutoSizing = true;
            //currencyText.fontSize = 27;
        }
    }
}

public enum StarterPack
{
    Gem,
    Ticket
}