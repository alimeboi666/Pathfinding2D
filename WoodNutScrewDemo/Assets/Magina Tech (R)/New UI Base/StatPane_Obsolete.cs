using Hung.GameData.Resources;
using Hung.GameData.RPG;
using Hung.StatSystem.Binding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI
{
    public class StatPane_Obsolete : MonoBehaviour
    {
        [SerializeField] private CharacterStatHolderGameEvent visualDataChange;
        [SerializeField] private UnitStat bindingStat;

        [SerializeField] private Image iconStat;
        [SerializeField] private TMP_Text textStatName;
        [SerializeField] private TMP_Text textStatValue;

        private void OnEnable()
        {
            visualDataChange.AddListener(OnDataChange);
        }

        private void OnDisable()
        {
            visualDataChange.RemoveListener(OnDataChange);
        }

        private void Awake()
        {
            //iconStat.sprite = DataResources.Instance.GetStatIcon(bindingStat);
            textStatName.text = bindingStat.ToString();
        }

        private void OnDataChange(IStatHolder<UnitBindingStat> statHolder)
        {
            statHolder.StatHolder.TryToGetValue(bindingStat, out float value);
            textStatValue.text = value.ToString();
        }
    }

}

