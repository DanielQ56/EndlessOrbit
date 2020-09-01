using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Item : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] Image spriteImage;
    [SerializeField] GameObject Activated;

    public delegate void SelectingItem(int index);
    public static SelectingItem selectedDelagate;

    public delegate void BuyingItem();
    public static BuyingItem buyingDelegate;

    PurchasableItem currentItem;

    int childIndex;

    public void SetItem(PurchasableItem item, int index)
    {
        itemName.text = item.itemName;
        spriteImage.sprite = item.ItemSprite;
        Debug.Log("Item Name: " + item.itemName + ", Bought: " + item.bought + ", Selected: " + item.selected);
        cost.text = (item.bought ? "Owned" : item.cost.ToString());
        cost.color = Color.white;
        currentItem = item;
        childIndex = index;


        Activated.SetActive(item.selected);
        button.interactable = !item.selected;
    }

    public void ButtonPressed()
    {
        if(currentItem.bought)
        {
            selectedDelagate(childIndex);
            currentItem.selected = true;
            Activated.SetActive(true);
            button.interactable = false;
        }
        else
        {
            if(PlayerManager.instance.BuyItem(currentItem.cost))
            {
                buyingDelegate();
                cost.text = "Owned";
                currentItem.bought = true;
            }
        }

    }

    public void DeactivateItem()
    {
        currentItem.selected = false;
        button.interactable = true;
        Activated.SetActive(false);
    }

    

}
