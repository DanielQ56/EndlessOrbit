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
        Global.onClick.AddListener(OpenGlobal);
    }


    private void OnEnable()
    {
        Global.gameObject.SetActive(true);
    }


    void OpenGlobal()
    {
        ScoreManager.instance.DisplayGlobalLeaderboard();
    }

    private void OnDestroy()
    { 
        Global.onClick.RemoveListener(OpenGlobal);
    }
}
