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

using System.Collections.Generic;
using WeaponsManagement;

namespace Services.ObjectPooling
{
    public class ObjectPool : IObjectPool
    {
        public int ActiveObjects => _activeObjects;
        public int TotalObjects => _pool.Count;
        
        private List<IPooleableObject> _pool;
        private int _initialNumberOfObjects;
        private int _activeObjects;
        
        private IPooleableObject _pooleableObjectPrototype;
        
        #region IService Implementation
        public bool IsSingleton => false;
        
        public void Initialize()
        {
            _pool = new List<IPooleableObject>();
            _initialNumberOfObjects = 0;
        }
        #endregion

        #region IObjectPool Implementation
        public void InitializePool(IPooleableObject pooleableObject, int initialNumberOfObjects)
        {
            _initialNumberOfObjects = initialNumberOfObjects;
            _pooleableObjectPrototype = pooleableObject;
            
            for (int i = 0; i < _initialNumberOfObjects; i++)
            {
                IPooleableObject obj = _pooleableObjectPrototype.Generate();
                obj.Available = true;
                _pool.Add(obj);
            }
        }

        public IPooleableObject Get()
        {
            IPooleableObject pooleableObject = _pool.Find(po => po.Available);

            if (pooleableObject == null)
            {
                pooleableObject = _pooleableObjectPrototype.Generate();
                _pool.Add(pooleableObject);
            }

            _activeObjects++;
            pooleableObject.Available = false;
            return pooleableObject;
        }

        public void Return(IPooleableObject pooleableObject)
        {
            _activeObjects--;
            pooleableObject.Reset();
            pooleableObject.Available = true;
        }
        #endregion
    }
}