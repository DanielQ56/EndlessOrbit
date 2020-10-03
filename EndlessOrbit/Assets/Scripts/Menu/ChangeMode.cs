using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMode : MonoBehaviour
{
    [SerializeField] GameObject[] sel;
    private void Awake()
    {
        foreach(GameObject selected in sel)
        {
            selected.GetComponent<Selectable>().Deselected();
        }
    }

    private void OnEnable()
    {
        sel[ScoreManager.instance.GetMode() ? 1 : 0].GetComponent<Selectable>().Selected();
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
