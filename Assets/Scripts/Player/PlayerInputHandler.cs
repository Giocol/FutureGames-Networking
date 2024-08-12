using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction moveAction;
        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
            moveAction = controls.MainPlayer.Move;
        }

        private void Update()
        {
            ReadMoveInput(moveAction.ReadValue<Vector2>());
        }

        private void ReadMoveInput(Vector2 direction)
        {
            //handle movement on player
        }
    }
}