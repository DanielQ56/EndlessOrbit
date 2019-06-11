using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class SliderScript : MonoBehaviour
{
    [SerializeField] Image fillArea;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Slider slider;
    [SerializeField] int index;

    char[] trimChars = new char[] { ' ' };

    public UnityEvent UpdatedColor;

    private void Awake()
    {
        UpdatedColor = new UnityEvent();
    }

    public void ActivatePanel(float val)
    {
        slider.value = val;
        switch (index)
        {
            case 0:
                fillArea.color = new Color(1f, 1f - val, 1f - val) ;
                break;
            case 1:
                fillArea.color = new Color(1f - val, 1f, 1f - val);
                break;
            case 2:
                fillArea.color = new Color(1f - val, 1f - val, 1f);
                break;
        }
    }

    public void UpdatedInputField()
    {
        string text = inputField.text.Trim(trimChars);
        if (text.Length > 0)
        {
            float val = float.Parse(inputField.text);
            val = (val % 256) / 255;
            slider.value = val;
            switch (index)
            {
                case 0:
                    fillArea.color = new Color(1f, 1f - val, 1f - val);
                    break;
                case 1:
                    fillArea.color = new Color(1f - val, 1f, 1f - val);
                    break;
                case 2:
                    fillArea.color = new Color(1f - val, 1f - val, 1f);
                    break;
            }
        }
        UpdatedColor.Invoke();
    }

    public void UpdateSlider()
    {
        float value = (slider.value * 255);
        inputField.text = value.ToString();
        switch (index)
        {
            case 0:
                fillArea.color = new Color(1f, 1f - slider.value, 1f - slider.value);
                break;
            case 1:
                fillArea.color = new Color(1f - slider.value, 1f, 1f - slider.value);
                break;
            case 2:
                fillArea.color = new Color(1f - slider.value, 1f - slider.value, 1f);
                break;
        }
        UpdatedColor.Invoke();
    }

    public float getValue()
    {
        return slider.value;
    }
}
