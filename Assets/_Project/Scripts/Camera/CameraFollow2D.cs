using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private CameraBounds2D cameraBounds;
        [SerializeField] private PlayerController2D playerController;
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private float lookAheadDistance = 1.5f;
        [SerializeField] private float minMovementForLookAhead = 0.001f;

        private Vector3 _velocity;
        private Vector3 _previousTargetPosition;

        private void Awake()
        {
            if (playerController == null)  playerController = FindFirstObjectByType<PlayerController2D>(); 
        }

        private void Start()
        {
            if (target != null) SnapToTarget();
        }

        private void LateUpdate()
        {
            if (!target || !playerController) return;

            Vector3 currentTargetPosition = target.position;
            Vector3 frameDelta = currentTargetPosition - _previousTargetPosition;
            
            Vector2 moveInput = playerController.MoveInput;

            Vector3 lookAheadOffset = Vector3.zero;
            if (frameDelta.sqrMagnitude > minMovementForLookAhead * minMovementForLookAhead && moveInput.sqrMagnitude > 0.0001f)
            {
                lookAheadOffset = moveInput.normalized * lookAheadDistance;
            }
            
            Vector3 targetPosition = currentTargetPosition + offset + lookAheadOffset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);

            if (cameraBounds)
            {
                smoothedPosition = cameraBounds.ClampPosition(smoothedPosition);
            }
            
            transform.position = smoothedPosition;
            _previousTargetPosition = currentTargetPosition;
        }

        private void SnapToTarget()
        {
            if (!target) return;

            Vector3 targetPosition = target.position + offset;

            if (cameraBounds != null)
            {
                targetPosition = cameraBounds.ClampPosition(targetPosition);
            }
            
            transform.position = targetPosition;
            _previousTargetPosition = targetPosition;
        }
    }
}
