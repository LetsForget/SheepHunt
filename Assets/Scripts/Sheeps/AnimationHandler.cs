using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public void OnGroundedChange(bool value)
    {
        _animator.SetBool("Grounded", value);
    }

    public void OnMovingChange(bool value)
    {
        _animator.SetBool("Moving", value);
    }

    [SerializeField] private Animator _animator;
}