using System;
using UnityEngine;
using UniRx;

namespace Sheeps.Spawn
{
    public class SheepSpawn : MonoBehaviour
    {
        public static event Action OnSheepPassedNotCathced;

        public bool Catched { get; private set; }
        public float SheepSpeed
        {
            set
            {
                _animator.speed = value;
            }
        }
        public float CircleTime
        {
            get
            {
                return _placeAnim.length + _onEndPause;
            }
        }
        public float SheepDissolve
        {
            get
            {
                return _sheepMR.material.GetFloat("_dissolve");
            }
            set
            {
                try
                {
                    _sheepMR.material.SetFloat("_dissolve", value);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public void LaunchDissolving(float time)
        {
            Catched = true;

            float timeStep = time / _dissolveStepsQuan;
            SheepDissolve = .25f;
            Dissolve(timeStep);
        }

        private void Start()
        {
            _sheepMR.material = new Material(_dissolveMaterial);

            if (_onEndPause == 0)
            {
                _onEndAction = BaseOnEndAction;
            }
            else
            {
                _onEndAction = OnEndActionWithDelay;
            }
        }

        private void Dissolve(float timeStep)
        {
            float dissolve = SheepDissolve + _dissolveStep;
            if (dissolve > 1)
            {
                SheepDissolve = 1;
            }
            else
            {
                SheepDissolve = dissolve;
                Observable.Timer(TimeSpan.FromSeconds(timeStep))
                    .Subscribe(_ => { Dissolve(timeStep); })
                    .AddTo(this);
            }
        }

        private void CallEvent() => _onEndAction();

        private void BaseOnEndAction()
        {
            SheepDissolve = 0;

            if (!Catched)
            {
                OnSheepPassedNotCathced();
            }
            else
            {
                Catched = false;
            }
        }

        private void OnEndActionWithDelay()
        {
            BaseOnEndAction();

            _animator.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(_onEndPause))
                .Subscribe(_ => { _animator.gameObject.SetActive(true); })
                .AddTo(this);
        }

        private Action _onEndAction;

        private const float _dissolveStep = .01f;
        private const int _dissolveStepsQuan = 75;

        [SerializeField] private float _onEndPause;

        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _placeAnim;

        [SerializeField] private Material _dissolveMaterial;
        [SerializeField] private SkinnedMeshRenderer _sheepMR;
    }
}