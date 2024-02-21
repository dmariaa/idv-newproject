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
using EnemiesManagement.EnemyIA;
using PlayerManagement;
using Services.ObjectPooling;
using UnityEngine;

namespace EnemiesManagement
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private static readonly int IsAttacking = Animator.StringToHash("is_attacking");
        
        [SerializeField] private float _attackDistance = 1.5f;
        public float AttackRate = 1.0f;
        public int life = 10;
        public float speed = 2f;
        
        private Animator _animator;
        private Transform _playerTransform;
        private IPlayerHit _playerHit;

        private float _lastAttackTime;
        private int _hits;
        
        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            _playerTransform = player.transform;
            _playerHit = player.GetComponent<IPlayerHit>();
            _animator = GetComponentInChildren<Animator>();
            Reset();
        }

        private void Update()
        {
            _currentState?.Update();
        }
        
        private void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        #region IEnemy implementation
        public bool Hit()
        {
            _hits++;

            if (_hits >= life)
            {
                OnEnemyDestroyed?.Invoke(this);
                return true;
            }
            
            return false;
        }

        public event Action<IEnemy> OnEnemyDestroyed;
        #endregion

        #region IPoolableObject implementation
        public IPooleableObject Generate()
        {
            return Instantiate(this);
        }

        public bool Available
        {
            get => !gameObject.activeSelf;
            set { gameObject.SetActive(!value); }
        }
        
        public void Reset()
        {
            _hits = 0;
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            SetState(new EnemyStateChasing());
        }
        #endregion

        #region IEnemyContext implementation
        private IEnemyState _currentState;

        public void SetState(IEnemyState state)
        {
            if(_currentState != null)
            {
                _currentState.Exit();
            }
            
            _currentState = state;
            _currentState.Enter(this);
        }

        public IEnemyState GetState()
        {
            return _currentState;
        }
        
        public float AttackRange { get => _attackDistance; }
        
        public Transform PlayerTransform { get => _playerTransform; }
        
        public Transform ZombieTransform { get => transform; }

        public void Move(Vector3 position)
        {
            transform.LookAt(_playerTransform);
            transform.position += transform.forward * (speed * Time.deltaTime);
            _animator.SetFloat(Velocity, 1.0f);
            _animator.SetBool(IsAttacking, false);
            
            _lastAttackTime = 0;
        }

        public void Attack()
        {
            _animator.SetFloat(Velocity, 0.0f);
            _animator.SetBool(IsAttacking, true);
            
            _lastAttackTime -= Time.deltaTime;

            if (_lastAttackTime < AttackRate)
            {
                _lastAttackTime = AttackRate;
                _playerHit.Hit();
            }
        }
        #endregion
    }
}