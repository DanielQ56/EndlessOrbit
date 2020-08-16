using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;



    private void OnEnable()
    {
        buttonText.text = GlobalLeaderboard.instance.IsPlayerSignedIn() ? "Log Out" : "Log In";
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
    }
}
