using System;
using _Project.Scripts.Player.Input;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        
        private Rigidbody2D _rigidbody2D;
        private PlayerInputHandler _inputHandler;
        
        private Vector2 _moveInput;
        private Vector2 _lastMoveDirection = Vector2.down;
        
        public Vector2 MoveInput => _moveInput;
        public Vector2 LastMoveDirection => _lastMoveDirection;
        public bool IsMoving => _moveInput.sqrMagnitude > 0.0001f;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            _moveInput = _inputHandler.MoveInput;
            
            if (IsMoving) _lastMoveDirection = _moveInput.normalized;
        }

        private void FixedUpdate()
        {
            Vector2 targetPosition = _rigidbody2D.position + _moveInput *  (moveSpeed *  Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(targetPosition);
        }
    }
}
