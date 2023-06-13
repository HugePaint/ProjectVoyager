using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RainbowArt.CleanFlatUI
{
    public class TabView : MonoBehaviour
    {
        [SerializeField]
        int startIndex = 0;
        public int StartIndex
        {
            get => startIndex;
            set
            {
                startIndex = value;
            }
        }   
        public TabViewItem[] tabViews;
        [Serializable]
        public class TabViewItem
        {
            public GameObject tab; 
            public GameObject view;         
        }         
        int currentIndex = 0;                
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if(currentIndex == value)
                {
                    return;
                }
                currentIndex = value;
            }
        }   
        void OnEnable()
        {
            InitAnimators();
            InitTabViews();                    
        }
        public void OnDisable()
        {
            for (int i = 0; i < tabViews.Length; i++)
            {
                int index = i;
                TabViewItem item = tabViews[i];
                Toggle toggle = item.tab.GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
            }
        }

        void InitAnimators()
        {
            for(int i = 0; i < tabViews.Length; i++)
            {
                Animator animator = tabViews[i].view.GetComponent<Animator>();
                ResetAnimation(animator);                             
            }
        }

        public void InitTabViews()
        {
            for (int i = 0; i < tabViews.Length; i++)
            {
                int index = i;
                TabViewItem item = tabViews[i];
                Toggle toggle = item.tab.GetComponent<Toggle>();
                if(i == startIndex)
                {
                    toggle.SetIsOnWithoutNotify(true);
                    Animator animatorTab = item.tab.GetComponent<Animator>();
                    Animator animatorView = item.view.GetComponent<Animator>();
                    item.view.SetActive(true);
                    PlayAnimation(animatorTab, "On Init");
                    PlayAnimation(animatorView, "Init");
                }
                else
                {
                    toggle.SetIsOnWithoutNotify(false);
                    item.view.SetActive(false);
                    item.tab.GetComponent<Tab>().UpdateStatusContent();
                }
            }
            currentIndex = startIndex;  
            for (int i = 0; i < tabViews.Length; i++)
            {
                int index = i;
                TabViewItem item = tabViews[i];
                Toggle toggle = item.tab.GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((bool value) => TabValueChanged(index, value));
            }
        }

        public void TabValueChanged(int index, bool value)
        {
            TabViewItem item = tabViews[index];
            Toggle toggle = item.tab.GetComponent<Toggle>();     
            Tab tab = item.tab.GetComponent<Tab>();   
            Animator animatorTab = item.tab.GetComponent<Animator>(); 
            Animator animatorView = item.view.GetComponent<Animator>();                    
            if (toggle.isOn)
            {
                currentIndex = index;
                item.view.SetActive(true);      
                PlayAnimation(animatorTab, "On");
                PlayAnimation(animatorView, "On");
            }
            else
            {
                item.view.SetActive(false);
                PlayAnimation(animatorTab, "Off");
                ResetAnimation(animatorView);
            }
        }  

        void PlayAnimation(Animator animator,string animStr)
        {
            if(animator != null)
            {
                if(animator.enabled == false)
                {
                    animator.enabled = true;
                }
                animator.Play(animStr,0,0);  
            }
        }

        void ResetAnimation(Animator animator)
        {
            if(animator != null)
            {
                animator.enabled = false;
            }
        }
    }
}