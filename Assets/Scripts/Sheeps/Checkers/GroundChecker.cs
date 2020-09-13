using System;
using UnityEngine;

namespace Sheeps.Checkers
{
    public class GroundChecker : AbstractChecker
    {
        public event Action<bool> OnGroundedChange;

        private void Start()
        {
            _mask = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            Vector3 downPoint = CalculateDownPoint();
            bool result = Physics.Raycast(downPoint, Vector3.down, out _, .35f, _mask);

            SetValue(result);
        }

        private Vector3 CalculateDownPoint()
        {
            Vector3 lMin = _collider.transform.InverseTransformPoint(_collider.bounds.min);
            Vector3 lCen = _collider.transform.InverseTransformPoint(_collider.bounds.center);

            Vector3 lDownMid = new Vector3(lCen.x, lMin.y, lCen.z);
            return _collider.transform.TransformPoint(lDownMid);
        }

        protected override void OnValueChanged(bool value)
        {
            OnGroundedChange(value);
        }

        private LayerMask _mask;
        [SerializeField] private Collider _collider;
    }
}