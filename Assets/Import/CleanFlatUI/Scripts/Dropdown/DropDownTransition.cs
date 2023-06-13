using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class DropDownTransition : Dropdown
    {
        Animator animator;
        //bool[] toggleIsOn;
        Toggle[] toggleList;
        IEnumerator diableCoroutine;
        float disableTime = 0.4f;

        public void Show()
        {
            base.Show();   
            Transform contentTransform = transform.Find("Dropdown List/Viewport/Content");
            toggleList = contentTransform.GetComponentsInChildren<Toggle>(false);
            //if(toggleIsOn == null){
            //    toggleIsOn = new bool[toggleList.Length];
            //}
            for (int i = 0; i < toggleList.Length; i++)
            {
                Toggle item = toggleList[i];
                item.onValueChanged.RemoveAllListeners();
                //item.isOn = toggleIsOn[i];
                item.onValueChanged.AddListener(x => OnSelectItemCustom(item));
            }

            if(animator == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animator = listTransform.gameObject.GetComponent<Animator>();                
            } 
            PlayAnimation(true);
        }

        private void OnSelectItemCustom(Toggle toggle)
        {
            if (!toggle.isOn)
                toggle.isOn = true;

            int selectedIndex = -1;
            Transform tr = toggle.transform;
            Transform parent = tr.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == tr)
                {
                    selectedIndex = i - 1;
                    break;
                }
            }

            if (selectedIndex < 0)
                return;
            value = selectedIndex;
            Hide();
        }

        public void Hide()
        {
            if(animator == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animator = listTransform.gameObject.GetComponent<Animator>();                
            } 
            PlayAnimation(false);   
            HideDropDown();          
        }

        public void HideDropDown()
        {
            if(diableCoroutine != null)
            {
                StopCoroutine(diableCoroutine);
                diableCoroutine = null;
            }    
            diableCoroutine = DisableTransition();              
            StartCoroutine(diableCoroutine);    
        }

        IEnumerator DisableTransition()
        {
            yield return new WaitForSeconds(disableTime);
            base.Hide();                                          
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

        public override void OnPointerClick(PointerEventData eventData)
        {
            Show();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Show();
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Hide();
        }
    }
}