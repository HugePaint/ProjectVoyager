using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RainbowArt.CleanFlatUI
{
    public class ModalWindow : MonoBehaviour
    {        
        public Image iconTitle;
        public Text title;
        public Button buttonClose;
        public Text description; 
        public RectTransform view;
        public RectTransform content;        
        public RectTransform buttonBar;
        public Button buttonConfirm; 
        public Button buttonCancel;           
        public Animator animator;               
        public UnityEvent onConfirmClick;
        public UnityEvent onCancelClick;      
        float contentSpaceHeight = 30f; 
        IEnumerator diableCoroutine;
        float disableTime = 0.5f;

        public void ShowModalWindow()
        {
            gameObject.SetActive(true);
            InitButtons();            
            InitAnimations();                  
            UpdateHeight();
            PlayAnimation(true); 
        }
        
        public void HideModelWindow()
        {
            PlayAnimation(false);    
            if(animator != null)
            {
                if(diableCoroutine != null)
                {
                    StopCoroutine(diableCoroutine);
                    diableCoroutine = null;
                }    
                diableCoroutine = DisableTransition();              
                StartCoroutine(diableCoroutine); 
            }  
            else
            {
                gameObject.SetActive(false);
            }          
        }

        IEnumerator DisableTransition()
        {
            yield return new WaitForSeconds(disableTime);
            gameObject.SetActive(false);         
        }  

        public void setIcon(Image newIcon)
        {
            iconTitle = newIcon;
        }

        public void SetTitle(Text newTitle)
        {
            title = newTitle;
        }

        public void setDesciption(Text newDescription)
        {
            description = newDescription;
            UpdateHeight();
        }
      
        void InitButtons()
        {
            if(buttonClose != null)
            {
                buttonClose.onClick.AddListener(OnCloseClick);  
            }
            if(buttonConfirm != null)
            {
                buttonConfirm.onClick.AddListener(OnCancelClick);  
            }
            if(buttonCancel != null)
            {
                buttonCancel.onClick.AddListener(OnConfirmClick);  
            }            
        }

        void OnCloseClick()
        {
            HideModelWindow();            
        }

        void OnCancelClick()
        {
            onCancelClick.Invoke();
            HideModelWindow();            
        }

        void OnConfirmClick()
        {
            onConfirmClick.Invoke();
            HideModelWindow();       
        }

        void InitAnimations()
        {
            if(animator != null)
            {
                animator.enabled = false;
                animator.gameObject.transform.localScale = Vector3.one;
                animator.gameObject.transform.localEulerAngles = Vector3.zero;
            } 
        }

        void UpdateHeight()
        {
            if(description != null)
            {
                float buttonBarHeight = 0f;                
                if(buttonBar != null)
                {
                    buttonBarHeight = buttonBar.rect.height;
                }
                float finalHeight = -content.anchoredPosition3D.y + description.preferredHeight + contentSpaceHeight + buttonBarHeight;
                view.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
                Vector3 pos = buttonBar.anchoredPosition3D;
                pos.y = -(-content.anchoredPosition3D.y + description.preferredHeight + contentSpaceHeight);
                buttonBar.anchoredPosition3D = pos;   
            }
        }
        
        void PlayAnimation(bool bStart)
        {
            if(animator != null)
            {
                if(animator.enabled == false)
                {
                    animator.enabled = true;
                }
                if(bStart)
                {
                    animator.Play("In",0,0);  
                }
                else
                {
                    animator.Play("Out",0,0);  
                }
            }            
        }         

        #if UNITY_EDITOR
        protected void OnValidate()
        {
            UpdateHeight();
        }
        #endif
    }
}