using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
 public class ProgressBarDemo : MonoBehaviour
{  
    public ProgressBar mProgressBar; // the ProgressBar object.
    void Start()
    {
        mProgressBar.HasText = false; //set whether show the text value
        mProgressBar.MaxValue = 100; // set the maximum value of the Progress Bar
        mProgressBar.CurrentValue = 50; //set the current value of the Progress Bar
    }        
}
 */

namespace RainbowArt.CleanFlatUI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        float currentValue = 0f;
        [SerializeField]
        float maxValue = 100.0f;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public Image foreground;

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                if(currentValue == value)
                {
                    return;
                }
                currentValue = value;
                OnValueChanged();
            }
        }
        public float MaxValue
        {
            get => maxValue;
            set
            {
                if(maxValue == value)
                {
                    return;
                }
                maxValue = value;
                OnValueChanged();
            }
        }

        public bool HasText
        {
            get => hasText;
            set
            {
                if (hasText == value)
                {
                    return;
                }
                hasText = value;
                UpdateText();
            }
        }

        void OnValueChanged()
        {
            if(maxValue < 0)
            {
                maxValue = 100.0f;
            }
            if(currentValue < 0)
            {
                currentValue = 0f;
            }
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
            UpdateGUI();
        }

        void Start()
        {
            UpdateGUI();
        }

        void UpdateGUI()
        {
            UpdateForeground();                                  
            UpdateText();
        }

        void UpdateForeground()
        {
            foreground.fillAmount = currentValue / maxValue;
        }

        void UpdateText()
        {
            if (text != null && text.gameObject.activeSelf != hasText)
            {
                text.gameObject.SetActive(hasText);
            }
            if (hasText && (text != null))
            {
                text.text = (int)((currentValue/maxValue)*100) + "%";
            }
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            OnValueChanged();
        }
#endif
    }
}