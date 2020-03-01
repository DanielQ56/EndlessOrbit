using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] List<PurchasableItem> allItems;

    int goldStars;
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


    public void SetupItems(bool[] bought = null, int selected = 0)
    {
        Debug.Log(bought == null);
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
        selectedIndex = selected;
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


    public void SetToDefault()
    {
        allItems[0].bought = true;
        allItems[0].selected = true;
        for(int i = 1; i < allItems.Count; ++i)
        {
            allItems[i].bought = false;
            allItems[i].selected = false;
        }
        goldStars = 0;
        silverStars = 0;
    }

    

    #region Stars
    public void Setup(int g = 0, int s = 0)
    {
        goldStars = g;
        silverStars = s;
        Debug.Log(goldStars + " " + silverStars);
    }

    public void AddStars(int g, int s)
    {
        goldStars += g;
        silverStars += s;
    }

    public int GetGoldStars()
    {
        return goldStars;
    }

    public int GetSilverStars()
    {
        return silverStars;
    }

    public bool BuyItem(bool premium, int cost)
    {
        if(premium)
        {
            if(goldStars >= cost)
            {
                goldStars -= cost;
                return true;
            }            
        }
        else
        {
            if (silverStars >= cost)
            {
                silverStars -= cost;
                return true;
            }
        }
        return false;
    }
    #endregion

}
