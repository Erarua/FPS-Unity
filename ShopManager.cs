using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShopManager : MonoBehaviourPunCallbacks
{
    public GameObject Shop;
    public MouseLock MouseLock;
    public WeaponManager WeaponManager;
    public PhotonView ppp;
   

    public int money = 100000;
    public int bonus;

    // Start is called before the first frame update
    void Start()
    {
        ppp= GetComponent<PhotonView>();
    }

    private void Update()

    {
        if (ppp.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.P) && Shop.activeSelf == false)
            {
                //Debug.Log("true");
                Shop.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                MouseLock.enabled = false;
                WeaponManager.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.P) && Shop.activeSelf == true)
            {
                //Debug.Log("false");
                Shop.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                MouseLock.enabled = true;
                WeaponManager.enabled = true;
            }
            bonus = WeaponManager.currentBonus;
            UpdateMoney();
        }
    }

    public void addMoney()
    {
        if (ppp.IsMine)
        {
            money += bonus;
        }
    }

    public void UpdateMoney()
    {
        if (ppp.IsMine)
        {
            Text moneyText = Shop.GetComponentInChildren<Text>();
            moneyText.text = "Current Money: $" + money;
        }
    }
}
