using System;
using InputManagement.Commands;
using PlayerManagement;
using UnityEngine;

namespace InputManagement
{
    public class InputManager : MonoBehaviour
    {
        private CommandInvoker _commandInvoker;
        
        private WalkCommand _walkCommand;
        private RunCommand _runCommand;
        private AimCommand _aimCommand;
        private ShootCommand _shootCommand;
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            
            _commandInvoker = new CommandInvoker();
            
            GameObject player = GameObject.FindWithTag("Player");
            _walkCommand = new WalkCommand(player.GetComponent<IMoveable>());
            _runCommand = new RunCommand(player.GetComponent<IMoveable>());
            _aimCommand = new AimCommand(player.GetComponent<IMoveable>());
            _shootCommand = new ShootCommand(player.GetComponent<IShooter>());
        }

        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _runCommand.Horizontal = horizontal;
                _runCommand.Vertical = vertical;
                _commandInvoker.ExecuteCommand(_runCommand);
            }
            else
            {
                _walkCommand.Horizontal = horizontal;
                _walkCommand.Vertical = vertical;
                _commandInvoker.ExecuteCommand(_walkCommand);    
            }
            
            Vector3 mousePosition = Input.mousePosition;
            _aimCommand.AimingPosition = mousePosition;
            _commandInvoker.ExecuteCommand(_aimCommand);
            
            if(Input.GetButton("Fire1"))
            {
                _commandInvoker.ExecuteCommand(_shootCommand);
            }
        }
    }
}
