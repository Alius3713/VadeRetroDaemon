using UnityEngine;

namespace _Project.Scripts.Rendering
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YSort : MonoBehaviour
    {
        [SerializeField] private int sortingOrderBase = 5000;
        [SerializeField] private int offset = 0;

        [Header("Optional")]
        [SerializeField] private Transform sortPoint;
        
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            if (sortPoint == null)
            {
                sortPoint = transform;
            }
        }

        private void LateUpdate()
        {
            UpdateSorting();
        }

        private void UpdateSorting()
        {
            float y = sortPoint.position.y;
            
            int sortingOrder = sortingOrderBase - Mathf.RoundToInt(y*100) +  offset;
            
            _renderer.sortingOrder = sortingOrder;
        }
    }
}
