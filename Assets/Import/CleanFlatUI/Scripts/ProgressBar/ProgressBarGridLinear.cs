using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* set properties in C# example codes
 using UnityEngine;
 using RainbowArt.CleanFlatUI;
 public class ProgressBarDemo : MonoBehaviour
{  
    public ProgressBarGridLinear mProgressBar; // the ProgressBar object.
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
    public class ProgressBarGridLinear : MonoBehaviour
    {
        [SerializeField]
        int currentValue = 0;
        [SerializeField]
        int maxValue = 10;
        [SerializeField]
        float spacing = 10;
        [SerializeField]
        bool hasText = true;
        public Text text;  
        public RectTransform background;
        public RectTransform foreground;
        public RectTransform bgTemplate;
        public RectTransform fgTemplate;
        List<RectTransform> bgList = new List<RectTransform>(); 
        List<RectTransform> fgList = new List<RectTransform>();        

        public int CurrentValue
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
        
        public int MaxValue
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
            if(currentValue < 0)
            {
                currentValue = 0;
            }
            if (maxValue < 0)
            {
                maxValue = 10;
            }
            if(currentValue > maxValue)
            {
                currentValue = maxValue;
            }
            UpdateGUI();
        }
      
        void Start()
        {  
            OnValueChanged();
            CreateList(bgList, background, bgTemplate);
            CreateList(fgList, foreground, fgTemplate);
            UpdateGUI();
        }

        void UpdateGUI()
        {            
            UpdateForeground();                                  
            UpdateText();
        }   

        void CreateList( List<RectTransform> list, RectTransform rectParent, RectTransform template)
        {
            template.gameObject.SetActive(false);
            float curX = 0;
            float itemWidth = template.rect.width;
            for (int i = 0; i < maxValue; i++)
            {
                RectTransform item = CreateItem(rectParent, template,i);
                list.Add(item);
                Vector3 pos = item.anchoredPosition3D;
                pos.x = curX;
                item.anchoredPosition3D = pos;
                curX += (itemWidth + spacing);
            }
           
        }

        RectTransform CreateItem(RectTransform rectParent, RectTransform template,int index)
        {
            GameObject obj = GameObject.Instantiate(template.gameObject, rectParent);
            obj.gameObject.SetActive(true);
            obj.gameObject.name = "item" + (index + 1);
            RectTransform rectTrans = obj.GetComponent<RectTransform>();
            rectTrans.localScale = Vector3.one;
            rectTrans.localEulerAngles = Vector3.zero;
            rectTrans.anchoredPosition3D = Vector3.zero;
            return rectTrans;
        }

        void UpdateForeground()
        {
            for(int i = 0; i < fgList.Count; i++)
            {
                RectTransform rectTrans = fgList[i];
                if(i < currentValue)
                {                    
                    rectTrans.gameObject.SetActive(true);
                }  
                else
                {
                    rectTrans.gameObject.SetActive(false);
                }              
            }  
        } 

        void UpdateText()
        {
            if (text != null && text.gameObject.activeSelf != hasText)
            {
                text.gameObject.SetActive(hasText);
            }
            if (hasText && (text != null))
            {
                float val = (float)currentValue / (float)maxValue;
                text.text = Mathf.FloorToInt(val * 100) + "%";
            }
        }     
    }
}