using Sheeps.Checkers;
using UnityEngine;

namespace Sheeps
{
    public class SheepContainer : MonoBehaviour
    {
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private MovingChecker _movingChecker;
        [SerializeField] private AnimationHandler _animationHandler;

        private void Start()
        {
            _groundChecker.OnGroundedChange += _animationHandler.OnGroundedChange;
            _movingChecker.OnMovingChanged += _animationHandler.OnMovingChange;
        }
    }
}