using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Selectable : MonoBehaviour
{
    [SerializeField] GameObject SelectedOverlay;

    Button b;

    SelectableParent sp;

    private void Awake()
    {
        b = this.GetComponent<Button>();
        sp = GetComponentInParent<SelectableParent>();
        b.onClick.AddListener(Clicked);
    }

    void Clicked()
    {
        sp.ButtonSelected(this);
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
