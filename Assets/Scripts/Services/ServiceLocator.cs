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
using System.Collections.Generic;

namespace Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }

                return _instance;
            }
        }
        
        private static readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();
        
        public void RegisterService<T>(T service) where T : IService
        {
            // Register the service
            string serviceName = typeof(T).Name;
            
            if (!_services.ContainsKey(serviceName))
            {
                _services.Add(serviceName, service);    
                
                if(service.IsSingleton)
                {
                    service.Initialize();
                }
            }
        }
        
        public void UnregisterService<T>() where T : IService
        {
            // Unregister the service
            string serviceName = typeof(T).Name;
            
            if (_services.ContainsKey(serviceName))
            {
                _services.Remove(serviceName);
            }
        }
        
        public T GetService<T>() where T : IService
        {
            // Get the service  
            string serviceName = typeof(T).Name;

            if (_services.ContainsKey(serviceName))
            {
                T service = (T)_services[serviceName];
                
                if (!service.IsSingleton)
                {
                    service = (T)Activator.CreateInstance(service.GetType());
                    service.Initialize();
                }

                return service;
            }

            return default(T);
        }
    }
}