using Assets.Scenes.TestFPS.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondWeapon;
    public Firearms lastWeapon;

    private PhotonView photonView;

    public static UIManager ggboy;
    private Firearms CurrentWeapon;
    public int currentBonus;

    public PlayerMovement PlayerMovement;

    public GameObject AK;

    public int state;

    private void Start()
    {

        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            ggboy = new UIManager();
            ggboy.Init();
            CurrentWeapon = SecondWeapon;

            CurrentWeapon.gameObject.SetActive(true);
            Debug.Log(CurrentWeapon.name);
            currentBonus = CurrentWeapon.bonus;
            PlayerMovement.FPAnimator = CurrentWeapon.GunAnimator;
            state = 2;
        }
    }

    private void Update()
    {
        
            Swap();
        if (photonView.IsMine)
        {
            if (Input.GetMouseButton(0))
            {
                CurrentWeapon.Attack();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentWeapon.Reload();
            }
            if (Input.GetMouseButtonDown(1))
            {
                //aiming = true;
                if (!CurrentWeapon.reloading)
                {
                    CurrentWeapon.Aim(true);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                //aiming = false;
                CurrentWeapon.Aim(false);
            }

            if (CurrentWeapon.name == "AK47")
            {
                AKKK.cur = CurrentWeapon.CurrentAmmo;
                AKKK.max = CurrentWeapon.CurrentMaxAmmoCarried;

                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<AKKK>("AK47");
            }
            if (CurrentWeapon.name == "G3")
            {
                G333.cur = CurrentWeapon.CurrentAmmo;
                G333.max = CurrentWeapon.CurrentMaxAmmoCarried;
                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<G333>("G3");
            }
            if (CurrentWeapon.name == "P90")
            {
                P900.cur = CurrentWeapon.CurrentAmmo;
                P900.max = CurrentWeapon.CurrentMaxAmmoCarried;

                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<P900>("P90");
            }
            if (CurrentWeapon.name == "SCARL")
            {
                AKKK.cur = CurrentWeapon.CurrentAmmo;
                AKKK.max = CurrentWeapon.CurrentMaxAmmoCarried;

                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<AKKK>("SCARL");
            }
            if (CurrentWeapon.name == "M4A1")
            {
                AKKK.cur = CurrentWeapon.CurrentAmmo;
                AKKK.max = CurrentWeapon.CurrentMaxAmmoCarried;
                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<AKKK>("M4A1");
            }
            if (CurrentWeapon.name == "UZI")
            {
                UZII.cur = CurrentWeapon.CurrentAmmo;
                UZII.max = CurrentWeapon.CurrentMaxAmmoCarried;
                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<UZII>("UZI");
            }
            if (CurrentWeapon.name == "HandGun")
            {
                GUNN.cur = CurrentWeapon.CurrentAmmo;
                GUNN.max = CurrentWeapon.CurrentMaxAmmoCarried;
                if (lastWeapon != null)
                {
                    ggboy.CloseUI(lastWeapon.name.ToString());
                }
                ggboy.ShowUI<GUNN>("HandGun");
            }
        }
    }

    private void Swap()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                if (state == 1) return;
                if (MainWeapon == null) return;
                state = 1;
                CurrentWeapon.gameObject.SetActive(false);
                lastWeapon = CurrentWeapon;
                CurrentWeapon = MainWeapon;
                currentBonus = CurrentWeapon.bonus;
                PlayerMovement.FPAnimator = CurrentWeapon.GunAnimator;
                CurrentWeapon.gameObject.SetActive(true);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                if (state == 2) return;
                state = 2;
                CurrentWeapon.gameObject.SetActive(false);
                lastWeapon = CurrentWeapon;
                CurrentWeapon = SecondWeapon;
                currentBonus = CurrentWeapon.bonus;
                PlayerMovement.FPAnimator = CurrentWeapon.GunAnimator;
                CurrentWeapon.gameObject.SetActive(true);
            }
        }
    }

    public void refresh(Firearms firearms)
    {
        if (photonView.IsMine)
        {
            if (state == 1)
            {
                CurrentWeapon.gameObject.SetActive(false);
                lastWeapon = CurrentWeapon;
                MainWeapon = firearms;
                CurrentWeapon = firearms;
                currentBonus = CurrentWeapon.bonus;
                PlayerMovement.FPAnimator = CurrentWeapon.GunAnimator;
                CurrentWeapon.gameObject.SetActive(true);
            }
            else if (state == 2)
            {
                MainWeapon = firearms;
            }
        }
    }
}
