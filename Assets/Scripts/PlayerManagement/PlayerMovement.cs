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
using UnityEngine;

namespace PlayerManagement
{
    public class PlayerMovement : MonoBehaviour, IMoveable
    {
        public GameObject crosshair;
        
        public float walkSpeed = 5;
        public float runSpeed = 10;

        private Animator _animator;
        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private float _speed;
        
        private static readonly int Speed = Animator.StringToHash("velocity");
        private static readonly int IsRunning = Animator.StringToHash("is_running");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _movement * (_speed * Time.fixedDeltaTime));
        }

        public void Move(float horizontal, float vertical)
        {
            _movement = new Vector3(horizontal, 0, vertical);
            _speed = walkSpeed;
            _animator.SetFloat(Speed, _movement.magnitude);
            _animator.SetBool(IsRunning, false);
        }

        public void Run(float horizontal, float vertical)
        {
            _movement = new Vector3(horizontal, 0, vertical);
            _speed = runSpeed;
            _animator.SetFloat(Speed, _movement.magnitude);
            _animator.SetBool(IsRunning, true);
        }

        public void Aim(Vector3 aimingPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(aimingPosition);
            
            if(Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Floor")))
            {
                Vector3 aiming = hit.point;
                
                if((aiming - transform.position).magnitude <= 1.5f)
                {
                    aiming = transform.position + (aiming - transform.position).normalized * 1.5f;
                }

                aiming.y = 0.1f;
                crosshair.transform.position = aiming;
                transform.LookAt(aiming);
            }
        }
        
        
    }
}