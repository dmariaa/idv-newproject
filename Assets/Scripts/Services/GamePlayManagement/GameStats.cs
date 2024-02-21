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

using Utils;

namespace Services.GamePlayManagement
{
    public class GameStats : IGameStats
    {
        private ObservableValue<int> _enemiesKilled;
        private ObservableValue<int> _enemiesSpawned;
        private ObservableValue<int> _playerLife;

        public ObservableValue<int> EnemiesKilled
        {
            get => _enemiesKilled;
        }

        public ObservableValue<int> EnemiesSpawned
        {
            get => _enemiesSpawned;
        }

        public ObservableValue<int> PlayerLife
        {
            get => _playerLife;
        }

        public int PlayerMaxLife
        {
            get => 1000;
        }

        public bool IsSingleton
        {
            get => true;
        }
        
        public void Initialize()
        {
            _enemiesKilled = new ObservableValue<int>(0);
            _enemiesSpawned = new ObservableValue<int>(0);
            _playerLife = new ObservableValue<int>(PlayerMaxLife);
        }
    }
}