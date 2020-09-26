using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableParent : MonoBehaviour
{
    [SerializeField] GameObject[] sel;
    private void Awake()
    {
        foreach(GameObject selected in sel)
        {
            selected.GetComponent<Selectable>().Deselected();
        }

        if(sel.Length > 0)
        {
            sel[0].GetComponent<Selectable>().Selected();
        }
    }

    public void ButtonSelected(Selectable s)
    {
        foreach (GameObject selected in sel)
        {
            selected.GetComponent<Selectable>().Deselected();
        }
        s.Selected();

    }
}
