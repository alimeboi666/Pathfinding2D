//using Hung.Core;
//using ScriptableObjectArchitecture;
//using Sirenix.OdinInspector;
//using UnityEngine;

//public class TimeController : MonoBehaviour, IArchChrono
//{
//    [field: Range(0, 15), SerializeField] public float timeScale { get; set; }

//    [ReadOnly, SerializeField] bool isPause;

//    private void Awake()
//    {
//        isPause = false;
//        timeScale = 1;
//    }

//    private void LateUpdate()
//    {
//        if (!isPause) Time.timeScale = timeScale;
//    }

//    [Button]
//    public void Pause()
//    {
//        isPause = true;
//        Time.timeScale = 0;
//    }

//    [Button]
//    public void Resume()
//    {
//        isPause = false;
//    }
//}
