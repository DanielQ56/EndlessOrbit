using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;

    public void SetText(string text)
    {
        infoText.text = text;
    }
}
