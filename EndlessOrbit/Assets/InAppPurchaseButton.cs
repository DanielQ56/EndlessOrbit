using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType{
    ads,
    currency
}

public class InAppPurchaseButton : MonoBehaviour
{
    [SerializeField] ItemType type;

    private void Awake()
    {
        if(GoogleAds.instance.ShouldShowAds())
        {
            this.GetComponent<Button>().onClick.AddListener(ClickToPurchase);
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void ClickToPurchase()
    {
        switch (type)
        {
            case ItemType.ads:
                IAPManager.instance.RemoveAds();
                break;
        }

    }

    private void OnDestroy()
    {
        this.GetComponent<Button>().onClick.RemoveListener(ClickToPurchase);
    }
}
