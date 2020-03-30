using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] bool isUnstable;

    Toggle toggle;
    // Start is called before the first frame update
    void Awake()
    {
        toggle = this.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(CheckToggle);
    }

    public void CheckToggle(bool value)
    {
        if (value)
        {
            ScoreManager.instance.DisplayScores(isUnstable);            
        }
    }

    private void OnDisable()
    {
        toggle.isOn = false;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(CheckToggle);
    }
}
