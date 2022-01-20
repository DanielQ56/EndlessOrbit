using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] List<PurchasableItem> allItems;

    [SerializeField] GameObject ShopPanel;

    List<int> DailyLoginValues = new List<int>();


    int silverStars;

    int silverStarsTotal;

    int selectedIndex = 0;

    int totalGamesPlayed;

    int orbitsTraversed;



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

    #region Last Login
    [SerializeField] List<int> rewardValues; //needed so the player can't constantly cycle

    System.DateTime nextBonus;

    public void SetNextBonusTime(System.DateTime lastTime)
    {
        nextBonus = lastTime;
    }

    public bool BonusAvailable()
    {
        return System.DateTime.Now > nextBonus;
    }

    public void JustReceivedBonus()
    {
        nextBonus = System.DateTime.Now.AddHours(12);
    }

    public System.DateTime GetNextBonus()
    {
        return nextBonus;
    }

    public bool DailyLoginValuesEmpty()
    {
        return DailyLoginValues.Count == 0;
    }

    public void SetLoginValues(List<int> values)
    {
        DailyLoginValues.Clear();
        DailyLoginValues.AddRange(values);
    }

    #endregion

    #region Shop/Items

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
        silverStarsTotal = 0;
        totalGamesPlayed = 0;
        orbitsTraversed = 0;
    }
    #endregion


    #region Stars
    public void AddStars(int s)
    {
        silverStars += s;
        silverStarsTotal += s;
    }

    public int GetSilverStars()
    {
        return silverStars;
    }

    public int GetSilverStarsTotal()
    {
        return silverStarsTotal;
    }

    public bool BuyItem(int cost)
    {
        if (silverStars >= cost)
        {
            silverStars -= cost;
            return true;
        }
        ScoreManager.instance.ProvideInfo("Not enough stars!");
        return false;
    }
    #endregion

    #region Stats    
    public void SetupStats(int s = 0, int t = 0, int g = 0, int o = 0)
    {
        silverStars = s;
        silverStarsTotal = t;
        totalGamesPlayed = g;
        orbitsTraversed = o;
    }

    public void AddGamesPlayed()
    {
        ++totalGamesPlayed;
    }

    public void AddOrbitsTraversed(int o)
    {
        orbitsTraversed += o;
    }

    public int GetGamesPlayed()
    {
        return totalGamesPlayed;
    }

    public int GetOrbitsTraversed()
    {
        return orbitsTraversed;
    }
    #endregion


}
