using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI index;
    [SerializeField] TextMeshProUGUI value;

    public void SetVariables(int ind, string val)
    {
        index.text = ind.ToString() + ".";
        value.text = val;
    }
}
