using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI index;
    [SerializeField] TextMeshProUGUI value;
    [SerializeField] GameObject outline;

    public void SetVariables(int ind, string val, bool isMostRecent)
    {
        index.text = ind.ToString() + ".";
        value.text = val;
        outline.SetActive(isMostRecent);
    }
}
