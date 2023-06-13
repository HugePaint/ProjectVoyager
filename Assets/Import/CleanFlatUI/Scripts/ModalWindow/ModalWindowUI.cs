using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class ModalWindowUI : MonoBehaviour
    {
        public Button button;
        public ModalWindow modalWindow;
        public void Start()
        {
            modalWindow.gameObject.SetActive(false);
            button.onClick.AddListener(OnButtonClick);  
        }
        public void OnButtonClick()
        {
            modalWindow.ShowModalWindow(); 
        }
    }
}