using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI errorText;

    public void SetText(string text)
    {
        errorText.text = text;
    }
}
