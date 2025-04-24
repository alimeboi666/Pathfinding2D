using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Hung/Config/Game Config")]
public partial class GameConfig : SOSingleton<GameConfig>
{
    [field:Range(0,10)][field:SerializeField] public float timeScale { get; set; }
      
    [field: SerializeField] public GUIStyle textStyleGizmos { get; private set; }
   
    [field:SerializeField] public Color colorGizmos { get; private set; }

    [field: SerializeField, Range(0,100)] public float sizeIcon { get; private set; }
}
