using _Project.Scripts.Core.Data.Preparation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class ResolutionMethodItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI methodNameText;
        [SerializeField] private TextMeshProUGUI methodDescriptionText;
        [SerializeField] private GameObject selectedFrame;
        [SerializeField] private Button methodButton;
        
        private ResolutionMethodDefinition _method;
        private PreparationPanelUI _owner;

        public void Initialize(ResolutionMethodDefinition method, PreparationPanelUI owner, bool isSelected)
        {
            _method = method;
            _owner = owner;

            if (methodNameText != null)
            {
                methodNameText.text = _method != null ? _method.DisplayName : "Missing Method";
            }

            if (methodDescriptionText != null)
            {
                methodDescriptionText.text = _method != null ? _method.Description : string.Empty;
            }
            
            if (selectedFrame != null) selectedFrame.SetActive(isSelected);

            if (methodButton != null)
            {
                methodButton.onClick.RemoveAllListeners();
                methodButton.onClick.AddListener(OnClicked);
            }
        }

        private void OnClicked()
        {
            if (_owner == null || _method == null) return;
            
            _owner.HandleMethodClicked(_method);
        }
    }
}
