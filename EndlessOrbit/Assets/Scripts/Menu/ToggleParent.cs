using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleParent : MonoBehaviour
{
    [SerializeField] Toggle normal;
    [SerializeField] Toggle unstable;
   

    public void UpdateToggles(bool isUnstable)
    {
        if (isUnstable)
        {
            unstable.isOn = false;
            unstable.isOn = true;
        }
        else
        {
            normal.isOn = false;
            normal.isOn = true;
        }
    }
}
