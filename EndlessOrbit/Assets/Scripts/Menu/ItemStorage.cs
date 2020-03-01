using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject grid;
    [SerializeField] GameObject itemPrefab;

    private void Awake()
    {
        Item.selectedDelagate += this.DeactivateItem;

    }

    private void Start()
    {
        SetupGrid();
    }

    void SetupGrid()
    {
        List<PurchasableItem> items = PlayerManager.instance.getAllItems();
        for(int i = 0; i < items.Count; ++i)
        {
            GameObject item = Instantiate(itemPrefab, grid.transform);
            item.GetComponent<Item>().SetItem(items[i], i);
        }
        panel.SetActive(false);
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
