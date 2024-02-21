#region Copyright
// MIT License
// 
// Copyright (c) 2024 david.maria
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using Services;
using Services.GamePlayManagement;
using Services.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemiesManagement
{
    public class EnemiesSpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public float spawnRate = 10f;
        public float spawnTimeOut = 5f;
        public float spawnTime = 5f;
        public float spawnRadius = 20f;

        public event Action<EnemiesSpawner> OnEnemySpawned;
        public event Action<EnemiesSpawner> OnEnemyDestroyed;

        private IObjectPool _enemyPool;
        private Transform _playerTransform;
        private float _lastSpawnTime;
        private float _waveSpawnTimeOut;
        
        private IGameStats _gameStats;
        
        public void Awake()
        {
            _enemyPool = ServiceLocator.Instance.GetService<IObjectPool>();
            _enemyPool.InitializePool(enemyPrefab.GetComponent<IPooleableObject>(), 200);
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _gameStats = ServiceLocator.Instance.GetService<IGameStats>();
            _waveSpawnTimeOut = 0;
        }

        private void Update()
        {
            if (_waveSpawnTimeOut <= spawnTimeOut)
            {
                _waveSpawnTimeOut += Time.deltaTime;
                _lastSpawnTime = Time.time - spawnTime;
                return;
            }
            
            if (Time.time - _lastSpawnTime >= spawnTime)
            {
                _lastSpawnTime = Time.time;
                SpawnEnemies();
            }
        }

        public void SpawnEnemies()
        {
            for(int i = 0; i < spawnRate; i++)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            Vector2 startPoint;
            int maxTries = 10;
            var position = _playerTransform.position;
            
            do
            {
                startPoint = Random.insideUnitCircle.normalized * spawnRadius 
                             + new Vector2(position.x, position.z);

                if (maxTries-- < 0)
                    return;
            } while(Mathf.Abs(startPoint.x) >= 48f || Mathf.Abs(startPoint.y) >= 48f);
            
            Enemy enemy = _enemyPool.Get() as Enemy;
            enemy.OnEnemyDestroyed += OnEnemyDestroyed_Listener;
            enemy.transform.position = new Vector3(startPoint.x, 0, startPoint.y);
            OnEnemySpawned?.Invoke(this);
            _gameStats.EnemiesSpawned.Value++;
        }

        private void OnEnemyDestroyed_Listener(IEnemy obj)
        {
            _enemyPool.Return(obj as IPooleableObject);
            OnEnemyDestroyed?.Invoke(this);
            _gameStats.EnemiesKilled.Value++;
        }
    }
}