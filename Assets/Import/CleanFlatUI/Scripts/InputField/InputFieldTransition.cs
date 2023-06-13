using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace RainbowArt.CleanFlatUI
{
    public class InputFieldTransition : MonoBehaviour
    {  
        public InputField inputField;
        public Animator animator;   
        EventTrigger eventTrigger;    

        void Start () 
        {
            ResetAnimation(animator);
            AddTriggersListener(inputField.gameObject,EventTriggerType.Select,InputFieldIn);
            inputField.onEndEdit.AddListener(delegate { InputFieldOut(); });
        }   

        void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if(trigger == null)
            {
                trigger = obj.AddComponent<EventTrigger>();
            }    
            if(trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }    
            UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventID;
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        public void InputFieldIn(BaseEventData data) 
        {
            UpdateGUI(true);
        }

        public void InputFieldOut()
        {
            UpdateGUI(false);
        }

        public void UpdateGUI(bool bIn)
        {
            if(bIn)
            {
                PlayAnimation(animator, "In");
            }
            else
            {
                if(inputField.text.Length == 0)
                {
                    PlayAnimation(animator, "Out");
                }  
            }
        }

        void PlayAnimation(Animator animator, string animStr)
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