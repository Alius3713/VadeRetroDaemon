using UnityEngine;

namespace _Project.Scripts.Rendering
{
    public class OccluderFadeObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] [Range(0f, 1f)] private float fadedAlpha = 0.35f;
        [SerializeField] private float fadeSpeed = 8f;

        private float _targetAlpha = 1f;
        private int _playerInsideCount;

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }
            
        }

        private void Update()
        {
            if (!targetRenderer) return;
            
            Color color = targetRenderer.color;
            float newAlpha = Mathf.MoveTowards(color.a, _targetAlpha, fadeSpeed * Time.deltaTime);
            
            if (Mathf.Approximately(color.a, newAlpha)) return;
            
            color.a = newAlpha;
            targetRenderer.color = color;
        }
        
        public void NotifyPlayerEntered()
        {
            _playerInsideCount++;
            _targetAlpha = fadedAlpha;
        }
        
        public void NotifyPlayerExited()
        {
            _playerInsideCount = Mathf.Max(0, _playerInsideCount - 1);

            if (_playerInsideCount == 0)
            {
                _targetAlpha = 1f;
            }
        }
        
        public void ForceVisible()
        {
            _playerInsideCount = 0;
            _targetAlpha = 1f;

            if (targetRenderer == null) return;

            Color color = targetRenderer.color;
            color.a = 1f;
            targetRenderer.color = color;
        }
    }
}
