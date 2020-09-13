using UnityEngine;
using UniRx;
using System.Collections;
using GameProcess;

namespace Sheeps.Spawn
{
    public class SheepSpawnManager : MonoBehaviour
    {
        private IEnumerator Start()
        {
            _sheepSpawn.gameObject.SetActive(false);
            yield return new WaitForSeconds(_startDelay);
            _sheepSpawn.gameObject.SetActive(true);

            if (_sheepQuan > 1)
            {
                _sheepSpawns = new SheepSpawn[_sheepQuan];
                _sheepSpawns[0] = _sheepSpawn;

                float spawnStep = _sheepSpawn.CircleTime / _sheepQuan;
                LaunchSpawnThroughTime(spawnStep, 1);
            }

            GameHandler.OnGameEnd += OnGameEnd;
        }

        private void LaunchSpawnThroughTime(float time, int index)
        {
            Observable.Timer(System.TimeSpan.FromSeconds(time))
            .Subscribe(_ =>
            {
                _sheepSpawns[index] = Instantiate(_sheepSpawn);

                if (index < _sheepQuan - 1)
                {
                    LaunchSpawnThroughTime(time, index + 1);
                }
            })
            .AddTo(this);
        }

        private void OnGameEnd()
        {
            foreach (SheepSpawn sheepSpawn in _sheepSpawns)
            {
                if (!sheepSpawn.Catched)
                {
                    sheepSpawn.SheepSpeed = 0;
                    sheepSpawn.LaunchDissolving(1);
                }
            }
        }

        private SheepSpawn[] _sheepSpawns;

        [SerializeField] [Range(1,15)] private int _sheepQuan;
        [SerializeField] private SheepSpawn _sheepSpawn;
        [SerializeField] private float _startDelay;
    }
}