using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameInput : MonoBehaviour
{
    [SerializeField] TMP_InputField input;

    private void OnEnable()
    {
        input.text = ScoreManager.instance.GetName();
    }

    public void ChangeName()
    {
        ScoreManager.instance.SetName(input.text);
        this.gameObject.SetActive(false);
    }
}
