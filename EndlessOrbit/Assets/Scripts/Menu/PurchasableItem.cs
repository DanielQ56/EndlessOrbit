using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableItem", menuName = "ScriptableObjects/PurchasableItem", order = 1)]
public class PurchasableItem : ScriptableObject
{
    public Sprite ItemSprite;
    public Material ItemMaterial;
    public string itemName;
    public int cost;
    public bool bought;
    public bool selected;
}
