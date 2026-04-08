using System;
using _Project.Scripts.Core.Data.Guide;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Guide
{
    public class GuideUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectiveTitleText;
        [SerializeField] private TextMeshProUGUI objectiveDescriptionText;
        [SerializeField] private GameObject panelRoot;

        private void Start()
        {
            SubscribeToEvents();
            Refresh();
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        public void Refresh()
        {
            if (GuideManager.Instance == null)
            {
                SetEmpty();
                return;
            }
            
            GuideObjectiveDefinition currentObjective = GuideManager.Instance.GetCurrentObjective();

            if (currentObjective == null)
            {
                SetEmpty();
                return;
            }

            if (panelRoot != null)
            {
                panelRoot.SetActive(true);
            }

            if (objectiveTitleText != null)
            {
                objectiveTitleText.text = currentObjective.Title;
            }

            if (objectiveDescriptionText != null)
            {
                objectiveDescriptionText.text = currentObjective.Description;
            }
        }

        private void HandleObjectiveStarted(GuideObjectiveDefinition objective)
        {
            Refresh();
        }

        private void HandleObjectiveCompleted(GuideObjectiveDefinition objective)
        {
            Refresh();
        }

        private void HandleGuideCleared()
        {
            Refresh();
        }

        private void HandleGuideCompleted()
        {
            Refresh();
        }

        private void SubscribeToEvents()
        {
            if (GuideManager.Instance == null) return;
            
            GuideManager.Instance.OnObjectiveStarted += HandleObjectiveStarted;
            GuideManager.Instance.OnObjectiveCompleted += HandleObjectiveCompleted;
            GuideManager.Instance.OnGuideCleared += HandleGuideCleared;
            GuideManager.Instance.OnGuideCompleted += HandleGuideCompleted;
        }
        
        private void UnsubscribeFromEvents()
        {
            if (GuideManager.Instance == null) return;

            GuideManager.Instance.OnObjectiveStarted -= HandleObjectiveStarted;
            GuideManager.Instance.OnObjectiveCompleted -= HandleObjectiveCompleted;
            GuideManager.Instance.OnGuideCleared -= HandleGuideCleared;
            GuideManager.Instance.OnGuideCompleted -= HandleGuideCompleted;
        }

        private void SetEmpty()
        {
            // if (panelRoot != null) panelRoot.SetActive(false);
            if (objectiveTitleText != null) objectiveTitleText.text = string.Empty;
            if (objectiveDescriptionText != null) objectiveDescriptionText.text = string.Empty;
        }
    }
}
