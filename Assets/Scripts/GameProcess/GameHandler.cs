using Sheeps.Spawn;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameProcess
{
    public class GameHandler : MonoBehaviour
    {
        public static event Action OnGameEnd;

        public static event Action OnMissClick;
        public static event Action OnCorrectClick;

        public int Points
        {
            get
            {
                return int.Parse(_pointsLabel.text);
            }
            private set
            {
                _pointsLabel.text = value.ToString();
            }
        }
        public int Time
        {
            get
            {
                return int.Parse(_timeLabel.text);
            }
            private set
            {
                _timeLabel.text = value.ToString();
            }
        }


        private void Start()
        {
            InputHandler.OnLMBPressed += OnLMBPressed;
            SheepSpawn.OnSheepPassedNotCathced += OnSheepPassedNotCathced;
            BonusComponent.OnBonusClick += OnBonusClick;

            Points = _startPoints;
            Time = _startTime;
            _gameoverLabel.gameObject.SetActive(false);

            TimerUpdate();

            OnGameEnd += OnGameEnded;
        }

        private void OnBonusClick()
        {
            Time += _bonusAddTime;
        }

        private void OnSheepPassedNotCathced()
        {
            Points -= 1;

            if (Points == 0)
            {
                OnGameEnd();
            }
        }

        private void OnLMBPressed()
        {
            if (Raycaster.RaycastMousePosFor(out SheepSpawn sheep) && !sheep.Catched)
            {
                sheep.LaunchDissolving(1);
                Points += 1;

                OnCorrectClick();
            }
            else
            { 
                OnMissClick();
            }
        }

        private void TimerUpdate()
        {
            _timer = Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    int currentTime = Time;

                    if (currentTime > 0)
                    {
                        Time = currentTime - 1;
                        TimerUpdate();
                    }
                    else
                    {
                        OnGameEnd();
                    }
                })
                .AddTo(this);
        }

        private void OnGameEnded()
        {
            _timer.Dispose();
            _gameoverLabel.gameObject.SetActive(true);
        }

        private IDisposable _timer;

        [SerializeField] private int _startPoints;
        [SerializeField] private int _startTime;
        [SerializeField] private int _bonusAddTime;

        [SerializeField] private Text _pointsLabel;
        [SerializeField] private Text _timeLabel;
        [SerializeField] private Text _gameoverLabel;
    }
}
