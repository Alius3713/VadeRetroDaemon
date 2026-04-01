using _Project.Scripts.Interactions;
using _Project.Scripts.Player.Input;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Detection")] [SerializeField] private Vector2 interactionOffset = new Vector2(0f, -0.75f);
        [SerializeField] private float interactionRadius = 1f;
        [SerializeField] private LayerMask interactionLayerMask;
        
        private PlayerInputHandler _inputHandler;
        private bool _wasInteractPressedLastFrame;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
        }
        
        private void OnEnable()
        {
            if (_inputHandler != null)
            {
                _inputHandler.OnInteractPressed += TryInteract;
            }
        }

        private void OnDisable()
        {
            if (_inputHandler != null)
            {
                _inputHandler.OnInteractPressed -= TryInteract;
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void TryInteract()
        {
            Debug.Log("interaction controller: TryInteract called");
            Vector2 detectionPosition = (Vector2)transform.position + interactionOffset;
            Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPosition, interactionRadius, interactionLayerMask);
            
            if (hits == null || hits.Length == 0) return;
            
            IInteractable closestInteractable =  null;
            float closestDistanceSqr = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                if (!hit) continue;
                
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable == null)
                {
                    interactable = hit.GetComponentInParent<IInteractable>();
                }
                if (interactable == null) continue;
                
                float distanceSqr = ((Vector2)hit.transform.position - detectionPosition).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestInteractable = interactable;
                }
            }
            
            closestInteractable?.Interact();
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector2 detectionPosition = (Vector2)transform.position + interactionOffset;
            Gizmos.DrawWireSphere(detectionPosition, interactionRadius);
        }
#endif        
    }
}
