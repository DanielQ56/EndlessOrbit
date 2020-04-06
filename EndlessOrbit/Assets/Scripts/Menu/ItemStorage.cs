using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] GameObject itemPrefab;

    private void Awake()
    {
        Item.selectedDelagate += this.DeactivateItem;

    }
    private void OnEnable()
    {
        SetupGrid();
    }


    void SetupGrid()
    {
        this.gameObject.SetActive(true);
        List<PurchasableItem> items = PlayerManager.instance.getAllItems();
        Debug.Log(items.Count);
        for(int i = 0; i < items.Count; ++i)
        {
            if (i < grid.transform.childCount)
            {
                grid.transform.GetChild(i).GetComponent<Item>().SetItem(items[i], i);
            }
            else
            {
                GameObject item = Instantiate(itemPrefab, grid.transform);
                item.GetComponent<Item>().SetItem(items[i], i);
            }
        }
    }



    void DeactivateItem(int itemIndex)
    {
        grid.transform.GetChild(PlayerManager.instance.GetSelectedIndex()).GetComponent<Item>().DeactivateItem();
        PlayerManager.instance.UpdatedSelectedIndex(itemIndex);       
    }


    private void OnDestroy()
    {
        Item.selectedDelagate -= DeactivateItem;

    }


}
