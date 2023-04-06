using Assets.Scenes.TestFPS.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WeaponButton : MonoBehaviour
{
    public ShopManager shopManager;
    public WeaponManager Manager;
    public Firearms weapon;
    public Text Text;
    public int price;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log(shopManager.money);
        if (shopManager.money < price) return;
        shopManager.money -= price;
        Debug.Log(shopManager.money);
        Text.text = "Purchased";
        price = 0;
        Manager.refresh(weapon);
    }
}
