using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSprite : MonoBehaviour
{
    private Image playerImage;
    void Start()
    {
        playerImage = GetComponent<Image>();
        playerImage.sprite = PlayerManager.instance.GetSelectedSprite();
    }
}
