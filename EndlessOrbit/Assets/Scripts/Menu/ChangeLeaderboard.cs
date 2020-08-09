using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeLeaderboard: MonoBehaviour
{
    [SerializeField] Button Local;
    [SerializeField] Button Global;

    private void Awake()
    {
        Local.onClick.AddListener(SwapToLocal);
        Global.onClick.AddListener(SwapToGlobal);
    }


    private void OnEnable()
    {
        Local.gameObject.SetActive(false);
        Global.gameObject.SetActive(true);
    }


    void SwapToGlobal()
    {
        Debug.Log("Swapping to global");
        ScoreManager.instance.ChangeLeaderboards(true, (MainGameManager.instance != null ? MainGameManager.instance.isUnstableMode() : false));
        Local.gameObject.SetActive(true);
        Global.gameObject.SetActive(false);
    }

    void SwapToLocal()
    {
        Debug.Log("Swapping to local");
        ScoreManager.instance.ChangeLeaderboards(false, (MainGameManager.instance != null ? MainGameManager.instance.isUnstableMode() : false));
        Local.gameObject.SetActive(false);
        Global.gameObject.SetActive(true);
    }

    private void OnDestroy()
    { 
        Local.onClick.RemoveListener(SwapToGlobal);
        Global.onClick.RemoveListener(SwapToLocal);
    }
}
