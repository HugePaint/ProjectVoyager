using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RainbowArt.CleanFlatUI
{
    public class DropdownMultiCheck : Dropdown
    {
        bool[] toggleIsOn;
        Toggle[] toggleList;
        Animator animator;

        public void Show()
        {
            base.Show();
            Transform contentTransform = transform.Find("Dropdown List/Viewport/Content");
            toggleList = contentTransform.GetComponentsInChildren<Toggle>(false);
            if(toggleIsOn == null){
                toggleIsOn = new bool[toggleList.Length];
            }
            for (int i = 0; i < toggleList.Length; i++)
            {
                Toggle item = toggleList[i];
                item.onValueChanged.RemoveAllListeners();
                item.isOn = toggleIsOn[i];
                item.onValueChanged.AddListener(x => OnSelectItemCustom(item));
            }
            if(animator == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animator = listTransform.gameObject.GetComponent<Animator>();                
            } 
            PlayAnimation(true);
        }

        public void Hide()
        {
            if(animator == null)
            {
                Transform listTransform = transform.Find("Dropdown List");
                animator = listTransform.gameObject.GetComponent<Animator>();                
            } 
            PlayAnimation(false);
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

        private void OnSelectItemCustom(Toggle toggle)
        {
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
            toggleIsOn[selectedIndex] = toggle.isOn;
        }
    }
}