using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public static ToggleController instance;
    [SerializeField] List<GameObject> savedButtons;
    [SerializeField] Button b;

    GameObject justSelected;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        if(b != null)
        {
            b.onClick.AddListener(ToggleSelectedButton);
        }

    }

    public void SaveToggle(GameObject selected)
    {
        if (b == null)
        {
            GameObject button = GameObject.FindGameObjectWithTag("BackButton");
            if (button != null)
            {
                b = button.GetComponent<Button>();
                b.onClick.AddListener(ToggleSelectedButton);
            }
            

        }
        if (savedButtons.Contains(selected))
        {
            justSelected = selected;
        }
    }

    public void ToggleSelectedButton()
    {
        justSelected.GetComponent<ToggleIndicators>().SelectButton();
    }
}
