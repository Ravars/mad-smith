using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class UIMatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] private GameObject currentSelected;
        [SerializeField] private GameObject previouslySelected;
        [SerializeField] private RectTransform currentSelectedTransform;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 newPosition = scrollRect.transform.InverseTransformPoint(contentPanel.position) -
                                  scrollRect.transform.InverseTransformPoint(target.position);

            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}