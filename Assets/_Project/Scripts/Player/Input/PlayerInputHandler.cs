using System;
using UnityEngine;
using _Project.Scripts.Core;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace _Project.Scripts.Player.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction _moveAction;
        private InputAction _interactAction;
        
        public Vector2 MoveInput { get;  private set; }
        
        public bool InteractPressed { get; private set; }

        private void Awake()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _interactAction = InputSystem.actions.FindAction("Interact");
            
            if (_moveAction == null)
            {
                Debug.LogError("Nie znaleziono akcji 'Move' w Input System.");
                return;
            }

            if (_interactAction == null)
            {
                Debug.LogError("Nie znaleziono akcji 'Interact' w Input System.");
                return;
            }
            
            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
            
            _interactAction.performed += OnInteractPerformed;
            _interactAction.canceled += OnInteractCanceled;
        }
        
        private void OnDestroy()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= OnMove;
                _moveAction.canceled -= OnMove;
            }

            if (_interactAction != null)
            {
                _interactAction.performed -= OnInteractPerformed;
                _interactAction.canceled -= OnInteractCanceled;
            }
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            if (InputLock.Occupied)
            {
                MoveInput = Vector2.zero;
                return;
            }
            
            MoveInput = obj.action.ReadValue<Vector2>();
        }

        private void OnInteractPerformed(InputAction.CallbackContext obj)
        {
            if (InputLock.Occupied)
            {
                InteractPressed = false;
                return;
            }

            InteractPressed = true;
        }

        private void OnInteractCanceled(InputAction.CallbackContext obj)
        {
            InteractPressed = false;
        }

        private void LateUpdate()
        {
            if (!InputLock.Occupied) return;
            
            MoveInput = Vector2.zero;
            InteractPressed = false;
        }
    }
}
