using System;
using UnityEngine;

namespace Sheeps.Checkers
{
    public class MovingChecker : AbstractChecker
    {
        public event Action<bool> OnMovingChanged;

        private void Start()
        {
            _lastPos = transform.position;
            _lastPos.y = 0;
        }

        private void Update()
        {
            Vector3 curPos = transform.position;
            curPos.y = 0;

            SetValue(Vector3.Distance(_lastPos, curPos) > .0001f);
            _lastPos = curPos;
        }

        protected override void OnValueChanged(bool value)
        {
            OnMovingChanged(value);
        }

        private Vector3 _lastPos;
    }
}