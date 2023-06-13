using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class NotificationUI : MonoBehaviour
    {
        public Button button;
        public Notification notification;
        public void Start()
        {
            notification.gameObject.SetActive(false);
            button.onClick.AddListener(OnButtonClick);  
        }
        public void OnButtonClick()
        {
            notification.ShowNotification(); 
        }
    }
}