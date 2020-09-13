using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameProcess
{
    public class BonusComponent : MonoBehaviour, IPointerDownHandler
    {
        public static event Action OnBonusClick;

        public float Opacity
        {
            get
            {
                return _text.color.a;
            }
            set
            {
                _text.color = ChangeAlpha(_text.color, value);
                _icon.color = ChangeAlpha(_icon.color, value);
            }
        }

        private void Start()
        {
            _spawnPointsQuan = _spawnPoints.Length;

            BonusHandler.OnStreakReached += OnStreakReached;
            Opacity = 0;
            _available = false;
        }

        private void OnStreakReached()
        {
            int randomPoint = UnityEngine.Random.Range(0, _spawnPointsQuan);

            transform.SetParent(_spawnPoints[randomPoint].transform);
            transform.localPosition = Vector3.zero;

            float timeStep = 1.0f / _disappearSteps;

            _currentVisibleChange?.Dispose();
            Appear(timeStep);

            _available = true;
        }

        private void Appear(float timeStep)
        {
            float newOpacity = Opacity + _disappearStep;
            if (newOpacity > 1)
            {
                Opacity = 1;
            }
            else
            {
                Opacity = newOpacity;
                _currentVisibleChange = Observable.Timer(TimeSpan.FromSeconds(timeStep))
                    .Subscribe(_ => { Appear(timeStep); })
                    .AddTo(this);
            }
        }

        private void Disappear(float timeStep)
        {
            float newOpacity = Opacity - _disappearStep;
            if (newOpacity < 0)
            {
                Opacity = 0;
            }
            else
            {
                Opacity = newOpacity;
                _currentVisibleChange = Observable.Timer(TimeSpan.FromSeconds(timeStep))
                    .Subscribe(_ => { Disappear(timeStep); })
                    .AddTo(this);
            }
        }

        private Color ChangeAlpha(Color color, float needAlpha)
        {
            return new Color(color.r, color.g, color.b, needAlpha);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_available)
            {
                _available = false;
                OnBonusClick();

                float timeStep = 1.0f / _disappearSteps;

                _currentVisibleChange.Dispose();
                Disappear(timeStep);
            }
        }

        private IDisposable _currentVisibleChange;

        private bool _available;
        private int _spawnPointsQuan;

        private const int _disappearSteps = 50;
        private const float _disappearStep = .02f;

        [SerializeField] private Text _text;
        [SerializeField] private Image _icon;

        [SerializeField] private GameObject[] _spawnPoints;
    }
}