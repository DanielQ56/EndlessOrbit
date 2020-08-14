using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;



    private void OnEnable()
    {
        buttonText.text = GooglePlayLeaderboard.instance.IsPlayerSignedIn() ? "Log Out" : "Log In";
    }

    void CheckConnectionToPlayStore()
    {
        if(GooglePlayLeaderboard.instance.IsPlayerSignedIn())
        {
            GooglePlayLeaderboard.instance.SignOut();
        }
        else
        {
            GooglePlayLeaderboard.instance.AuthenticateUser();
        }
    }
}
