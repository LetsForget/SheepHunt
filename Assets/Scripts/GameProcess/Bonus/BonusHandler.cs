using Sheeps.Spawn;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameProcess
{
    public class BonusHandler : MonoBehaviour
    {
        public static event Action OnStreakReached;

        public int Streak
        {
            get
            {
                return (int)_slider.value;
            }
            private set
            {
                _slider.value = value;
            }
        }

        private void Start()
        {
       //     SheepSpawn.OnSheepPassedNotCathced += OnStreakLoss;
            GameHandler.OnMissClick += OnStreakLoss;

            GameHandler.OnCorrectClick += OnStreakIncrement;

            _slider.value = 0;
            _slider.maxValue = _streakToBonus;
        }

        private void OnStreakIncrement()
        {
            Streak++;
            if (Streak == _streakToBonus)
            {
                Streak = 0;
                OnStreakReached();
            }
        }

        private void OnStreakLoss()
        {
            Streak = 0;
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private int _streakToBonus;
    }
}