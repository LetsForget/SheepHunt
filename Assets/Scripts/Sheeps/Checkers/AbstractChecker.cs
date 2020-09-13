using UnityEngine;

namespace Sheeps.Checkers
{
    public abstract class AbstractChecker : MonoBehaviour
    {
        protected abstract void OnValueChanged(bool value);

        protected void SetValue(bool value)
        {
            if (_value != value)
            {
                _value = value;
                OnValueChanged(value);
            }
        }

        protected bool _value;
    }
}