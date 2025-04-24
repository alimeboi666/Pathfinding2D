using System;
using System.Collections.Generic;
using Hung.Core;
using UnityEngine;

namespace Hung.Testing
{
    public interface IKeyNumberListener : IMono
    {
        void OnKeyNumberPress(int index);
    }

    public class Test : Singleton<Test>, ICache
    {
        public delegate void Action();

        private static Action testAction, testAction1, testAction2, testAction3;
        private static Action<bool> signalAction;

        [SerializeField] internal bool signal;

        public void SwapSignal()
        {
            signal = !signal;
        }

        public void Test_Function(Vector2 input)
        {
            //Debug.Log(input); 
        }

        public void Test_Function2(Vector3 input)
        {
            Debug.Log(input);
        }

        private void Awake()
        {
            testAction = null;
            testAction1 = null;
            testAction2 = null;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (testAction != null)
                {
                    testAction();
                    Debug.Log("T has been actually pressed to cast " + testAction.GetInvocationList().Length + " actions !");
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                testAction1();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                testAction2();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                testAction3();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (signalAction != null) signalAction(signal);
                signal = !signal;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                //WeaponController.Instance.Click_Reload();
            }
            else
            {
                for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha8; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        keyListeners.ForEach(listener => listener.GetComponent<IKeyNumberListener>().OnKeyNumberPress(i - (int)KeyCode.Alpha1));
                    }
                }
            }
#endif
        }

        [SerializeField] List<GameObject> keyListeners;

        public void OnCached()
        {
            keyListeners = TypeFinder.FindGameObjectsOfComponent<IKeyNumberListener>();
        }

        public static void T_Only(Action action)
        {
            testAction = action;
        }

        public static void T(Action action)
        {
            testAction += action;
        }

        public static void F(Action action)
        {
            testAction1 += action;
        }

        public static void G(Action action)
        {
            testAction2 += action;
        }

        public static void J(Action action)
        {
            testAction3 += action;
        }

        public static void Signal(Action<bool> action)
        {
            signalAction += action;
        }
    }

    public interface ITest
    {
        void OnTest();
    }
}

