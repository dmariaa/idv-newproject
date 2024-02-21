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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class GameHUD : MonoBehaviour
    {
        public TextMeshProUGUI enemiesKilledText;
        public TextMeshProUGUI enemiesSpawnedText;
        public Slider playerLifeSlider;

        private IGameStats _gameStats;
        private void Awake()
        {
            _gameStats = ServiceLocator.Instance.GetService<IGameStats>();
            _gameStats.EnemiesKilled.OnValueChanged += OnEnemiesKilledChanged;
            _gameStats.EnemiesSpawned.OnValueChanged += OnEnemiesSpawnedChanged;
            _gameStats.PlayerLife.OnValueChanged += OnPlayerLifeChanged;
        }

        private void OnPlayerLifeChanged(int obj)
        {
            playerLifeSlider.value = (float)obj / _gameStats.PlayerMaxLife; 
        }

        private void OnDestroy()
        {
            _gameStats.EnemiesKilled.OnValueChanged -= OnEnemiesKilledChanged;
            _gameStats.EnemiesSpawned.OnValueChanged -= OnEnemiesSpawnedChanged;
        }

        private void OnEnemiesSpawnedChanged(int obj)
        {
            if(enemiesSpawnedText != null)
            {
                enemiesSpawnedText.text = obj.ToString();
            }
        }

        private void OnEnemiesKilledChanged(int obj)
        {
            if(enemiesKilledText != null)
            {
                enemiesKilledText.text = obj.ToString();
            }
        }
    }
}