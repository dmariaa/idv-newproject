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

namespace EnemiesManagement
{
    public class Enemy : MonoBehaviour, IEnemy, IPooleableObject
    {
        public int life = 10;

        private GameObject _enemiesParent;
        private int _hits;
        
        public bool Hit()
        {
            _hits++;
            Debug.Log($"Enemy {name} hit: {_hits} hits total");
            if (_hits >= life)
            {
                OnEnemyDestroyed?.Invoke(this);
                return true;
            }
            
            return false;
        }

        public event Action<IEnemy> OnEnemyDestroyed;

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
        }
    }
}