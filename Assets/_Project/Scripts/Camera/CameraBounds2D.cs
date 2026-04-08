using System;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraBounds2D : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boundsCollider;
        
        private UnityEngine.Camera _camera;
        private float _halfHeight;
        private float _halfWidth;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;

            if (_camera == null)
            {
                Debug.LogError("CameraBounds2D: No camera found!");
                return;
            }

            CalculateCameraExtents();
        }

        private void CalculateCameraExtents()
        {
            _halfHeight = _camera.orthographicSize;
            _halfWidth = _halfHeight * _camera.aspect;
            
        }

        public Vector3 ClampPosition(Vector3 targetPosition)
        {
            if (boundsCollider == null) return targetPosition;
            
            Bounds bounds = boundsCollider.bounds;
            
            float minX = bounds.min.x + _halfWidth;
            float maxX = bounds.max.x - _halfWidth;

            float minY = bounds.min.y + _halfHeight;
            float maxY = bounds.max.y - _halfHeight;
            
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);
            
            return new Vector3(clampedX, clampedY, targetPosition.z);
        }
    }
}
