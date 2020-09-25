using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableParent : MonoBehaviour
{
    [SerializeField] Selectable[] sel; 


    public void ButtonSelected(Selectable s)
    {
        foreach(Selectable selected in sel)
        {
            selected.Deselected();
        }
        s.Selected();

    }
}
