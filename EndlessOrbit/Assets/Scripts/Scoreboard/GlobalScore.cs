using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;


public class GlobalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI index;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] GameObject outline;
    [SerializeField] TextMeshProUGUI score;

    public void SetVariables(int ind, string name, int val, bool isMostRecent)
    {
        index.text = ind.ToString() + ".";
        this.name.text = name;
        score.text = val.ToString();
        outline.SetActive(isMostRecent);
    }
}
