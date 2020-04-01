using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StarAmounts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI silverStars;

    private void Awake()
    {
        Item.buyingDelegate += UpdateText;
    }

    private void OnEnable()
    {
        UpdateText();
    }

    void UpdateText()
    {
        silverStars.text = PlayerManager.instance.GetSilverStars().ToString();
    }

    private void OnDestroy()
    {
        Item.buyingDelegate -= UpdateText;
    }
}
