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
using Services.ObjectPooling;
using UnityEngine;
using WeaponsManagement;

namespace PlayerManagement
{
    public class PlayerAttack : MonoBehaviour, IShooter
    {
        public GameObject bulletPrefab;
        
        public int attackRate = 1;
        
        private Transform _crosshair;
        private Transform _bulletSpawnPoint;
        
        private IObjectPool _bulletPool;
        private float _lastAttackTime;
        private bool _fire;

        private void Awake()
        {
            _crosshair = GetComponent<PlayerMovement>().crosshair.transform;
            _bulletSpawnPoint = transform.Find("bullet_spawn_point");

            _bulletPool = ServiceLocator.Instance.GetService<IObjectPool>();
            _bulletPool.InitializePool(bulletPrefab.GetComponent<IBullet>(), 300);
            
            _lastAttackTime = Time.time;
        }
        
        private void Update()
        {
            if (_fire && Time.time - _lastAttackTime > 1f / attackRate)
            {
                _lastAttackTime = Time.time;
                _fire = false;
                ShootBullet();
            }
        }

        public void Shoot()
        {
            _fire = true;
        }

        private void ShootBullet()
        {
            Vector3 aimingDirection = _crosshair.position - _bulletSpawnPoint.position;
            IBullet bullet = _bulletPool.Get() as IBullet;
            bullet.Fire(_bulletSpawnPoint.position, aimingDirection.normalized);
            bullet.OnBulletDestroyed += OnBulletDestroyed;
        }

        private void OnBulletDestroyed(IBullet obj)
        {
            _bulletPool.Return(obj);
            obj.OnBulletDestroyed -= OnBulletDestroyed;
        }
    }
}