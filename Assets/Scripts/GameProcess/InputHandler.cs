using System;
using UniRx;
using UnityEngine;

namespace GameProcess
{
    public class InputHandler : MonoBehaviour
    {
        public static event Action OnLMBPressed;
        public static event Action<string> OnKeyPressed;

        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("More than one Input Handlers!");
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
                .Subscribe(key => { OnLMBPressed(); });

            //Observable.EveryUpdate()
            //    .Where(_ => Input.anyKeyDown)
            //    .Select(_ => Input.inputString)
            //    .Subscribe(key => { OnKeyPressed(key); });
        }

        private static InputHandler _instance;
    }
}