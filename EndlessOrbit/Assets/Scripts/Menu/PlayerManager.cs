using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] List<PurchasableItem> allItems;

    [SerializeField] GameObject ShopPanel;

    int silverStars;

    int selectedIndex = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void OpenShopInventory()
    {
        ShopPanel.SetActive(true);
    }


    public void SetupItems(bool[] bought = null, int selected = 0)
    {
        Debug.Log(bought == null);
        Debug.Log("Selected is: " + selected);
        if (bought != null)
        {
            for (int i = 0; i < allItems.Count; ++i)
            {
                if (i < bought.Length)
                    allItems[i].bought = bought[i];
                else
                    allItems[i].bought = false;
                allItems[i].selected = (i == selected);
            }
        }
        else
        {
            Debug.Log("Default");
            SetToDefault();
        }
        selectedIndex = selected;
    }

    public Sprite GetSelectedSprite()
    {
        return allItems[selectedIndex].ItemSprite;
    }

    public int GetSelectedIndex()
    {
        return selectedIndex;
    }

    public void UpdatedSelectedIndex(int i)
    {
        selectedIndex = i;
    }

    public ref List<PurchasableItem> getAllItems()
    {
        return ref allItems;
    }

    public void SetDefaultItems()
    {
        allItems[0].bought = true;
        allItems[0].selected = true;
        for (int i = 1; i < allItems.Count; ++i)
        {
            allItems[i].bought = false;
            allItems[i].selected = false;
        }
    }


    public void SetToDefault()
    {
        SetDefaultItems();
        selectedIndex = 0;
        silverStars = 0;
    }

    

    #region Stars
    public void Setup(int s = 0)
    {
        silverStars = s;
    }

    public void AddStars(int s)
    {
        silverStars += s;
    }

    public int GetSilverStars()
    {
        return silverStars;
    }

    public bool BuyItem(int cost)
    {
        if (silverStars >= cost)
        {
            silverStars -= cost;
            return true;
        }

        return false;
    }
    #endregion

}
