using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SignInButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(CheckConnectionToPlayStore);
    }

    private void OnEnable()
    {
        UpdateText();
    }

    void CheckConnectionToPlayStore()
    {
        if(GlobalLeaderboard.instance.IsPlayerSignedIn())
        {
            GlobalLeaderboard.instance.SignOut();
        }
        else
        {
            GlobalLeaderboard.instance.AuthenticateUser();
        }
        UpdateText();
    }

    void UpdateText()
    {
        buttonText.text = GlobalLeaderboard.instance.IsPlayerSignedIn() ? "Log Out" : "Log In";
    }

    private void OnDestroy()
    {
        this.GetComponent<Button>().onClick.RemoveListener(CheckConnectionToPlayStore);
    }
}
