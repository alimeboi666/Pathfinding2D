using Hung.GameData.RPG;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hung.GameData.Resources
{
    [CreateAssetMenu(menuName = "Hung/Data/Resources")]
    public partial class DataResources : SOSingleton<DataResources>
    {
        [SerializeField] [PreviewField] private Sprite[] statIcons;       

        public Sprite GetStatIcon(UnitStat stat)
        {
            return statIcons[(byte)stat];
        }
    }

}

