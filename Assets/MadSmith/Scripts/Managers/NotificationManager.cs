using Lean.Gui;
using MadSmith.Scripts.UI;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.Managers
{
    public enum NotificationType
    {
        Alert,
        Success,
        Warning
    }
    
    public class NotificationManager : Singleton<NotificationManager>
    {
        [SerializeField] private Notification notification;
        public void NewNotification(string text, NotificationType type)
        {
            notification.SetNotification(text, type);
        }
    }
}