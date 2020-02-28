using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    int goldStars;
    int silverStars;

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

    public void Setup(int g = 0, int s = 0)
    {
        goldStars = g;
        silverStars = s;
        Debug.Log("Gold Stars: " + g + ", Silver Stars: " + s);
    }

    public void AddStars(int g, int s)
    {
        goldStars += g;
        silverStars += s;
        Debug.Log("Gold Stars: " + g + ", Silver Stars: " + s);
    }

    public int GetGoldStars()
    {
        return goldStars;
    }

    public int GetSilverStars()
    {
        return silverStars;
    }
}
