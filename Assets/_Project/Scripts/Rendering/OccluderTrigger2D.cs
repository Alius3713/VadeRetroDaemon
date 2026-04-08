using System;
using UnityEngine;

namespace _Project.Scripts.Rendering
{
    public class OccluderTrigger2D : MonoBehaviour
    {
        [SerializeField] private OccluderFadeObject occluderFadeObject;
        [SerializeField] private string playerTag = "Player";

        private void Awake()
        {
            if (occluderFadeObject == null)
            {
                occluderFadeObject = GetComponent<OccluderFadeObject>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (occluderFadeObject == null) return;
            if (!other.CompareTag(playerTag)) return;
            
            occluderFadeObject.NotifyPlayerEntered();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (occluderFadeObject == null) return;
            if (!other.CompareTag(playerTag)) return;
            
            occluderFadeObject.NotifyPlayerExited();
        }
    }
}
