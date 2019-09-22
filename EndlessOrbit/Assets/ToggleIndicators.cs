using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ToggleIndicators : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject LeftArrow;
    [SerializeField] GameObject RightArrow;
    [SerializeField] bool FirstButton = false;
    
    Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        if(FirstButton)
        {
            button.Select();
            if(ToggleController.instance != null) ToggleController.instance.SaveToggle(this.gameObject);
        }
    }

    public void SelectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData data)
    {
        ToggleController.instance.SaveToggle(this.gameObject);
        Selected();
    }

    public void OnDeselect(BaseEventData data)
    {
        Deselected();
    }

    void Selected()
    {
        LeftArrow.SetActive(true);
        RightArrow.SetActive(true);
    }

    void Deselected()
    {
        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
    }

}
