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
using Services.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemiesManagement
{
    public class EnemiesSpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public float spawnRate = 10f;
        public float spawnTime = 5f;
        public float spawnRadius = 20f;
        
        private ObjectPool _enemyPool;
        private Transform _playerTransform;
        private float _lastSpawnTime;
        
        public void Awake()
        {
            _enemyPool = new ObjectPool();
            _enemyPool.Initialize(enemyPrefab.GetComponent<IPooleableObject>(), 200);
            _playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
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
                Enemy enemy = _enemyPool.Get() as Enemy;
                Vector2 startPoint = Random.insideUnitCircle.normalized * spawnRadius;
                enemy.OnEnemyDestroyed += OnEnemyDestroyed;
                enemy.transform.position = new Vector3(startPoint.x, 0, startPoint.y) + _playerTransform.position;
            }
        }

        private void OnEnemyDestroyed(IEnemy obj)
        {
            _enemyPool.Return(obj as IPooleableObject);
        }
    }
}