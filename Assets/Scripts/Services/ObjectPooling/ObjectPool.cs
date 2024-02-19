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
        private List<IPooleableObject> _pool;
        private int _numberOfObjects;
        private IPooleableObject _pooleableObjectPrototype;

        public ObjectPool()
        {
            _pool = new List<IPooleableObject>();
            _numberOfObjects = 0;
        }

        public void Initialize(IPooleableObject pooleableObject, int initialNumberOfObjects)
        {
            _numberOfObjects = initialNumberOfObjects;
            _pooleableObjectPrototype = pooleableObject;
            
            for (int i = 0; i < _numberOfObjects; i++)
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

            pooleableObject.Available = false;
            return pooleableObject;
        }

        public void Return(IPooleableObject pooleableObject)
        {
            pooleableObject.Reset();
            pooleableObject.Available = true;
        }
    }
}