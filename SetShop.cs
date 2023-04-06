using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetShop : MonoBehaviour
{
    private ShopManager shopManager;
    private GameObject shop;
    private Button buy1;
    private Button buy2;
    private Button buy3;

    private Item item1 = new Item(100, "m4a1");
    private Item item2 = new Item(1000, "ak47");
    private Item item3 = new Item(100, "m4a1");
    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("ShopPanel");
        shop.SetActive(false);

        buy1 = GameObject.Find("buy1").GetComponent<Button>();//通过Find查找名称获得我们要的Button组件
        //buy1.onClick.AddListener(OnClickItem1);
    }

    //private void OnClickItem1()
    //{
    //    if (shop.activeSelf)
    //    {
    //        shopManager.buyItem(item1);
    //    }
    //}

    private void Update()
    {
        ShowShop();
        if(shop.activeSelf == true)
        {
            Debug.Log(Input.GetKeyDown(KeyCode.P));
        }

    }
    private void ShowShop()
    {
        if (Input.GetKeyDown(KeyCode.P)&&shop.activeSelf==false)
        {
            shop.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.P) && shop.activeSelf == true)
        {
            shop.SetActive(false);
        }
    }
    
}
