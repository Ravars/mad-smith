using System;
using Lean.Gui;
using MadSmith.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private LeanPulse notification;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private Image backgroundImage;
        public void SetNotification(string text, NotificationType type)
        {
            Color color = Color.white;
            switch (type)
            {
                case NotificationType.Alert:
                    color = Color.yellow;
                    break;
                case NotificationType.Success:
                    color = Color.green;
                    break;
                case NotificationType.Warning:
                    color = Color.red;
                    break;
            }

            backgroundImage.color = color;
            notificationText.text = text;
            notification.Pulse();
        }
    }
}