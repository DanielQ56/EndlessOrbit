using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Selectable: MonoBehaviour
{
    [SerializeField] GameObject SelectedOverlay;

    Button b;

    ChangeMode cm;

    private void Awake()
    {
        b = this.GetComponent<Button>();
        cm = GetComponentInParent<ChangeMode>();
        b.onClick.AddListener(Clicked);
    }

    void Clicked()
    {
        cm.ButtonSelected(this);
    }

    public void Selected()
    {
        if(SelectedOverlay != null)
            SelectedOverlay.SetActive(true);
    }

    public void Deselected()
    {
        if (SelectedOverlay != null)
            SelectedOverlay.SetActive(false);
    }



    private void OnDestroy()
    {
        b.onClick.RemoveListener(Clicked);
    }
}
