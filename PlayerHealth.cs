using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// Ѫ��
    /// </summary>
    public float maxHealth = 200f;
    public float currentHealth;
    public float hitDamage;
    public bool isBossAttack;

    public PhotonView ll;
    public WeaponManager W;
    public PlayerMovement p;
    public MouseLock MouseLock;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        HPPP.max = maxHealth;
        HPPP.curHP = currentHealth;
        ll= GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ll.IsMine)
        {
            HPPP.curHP = currentHealth;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHitPlayer(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitPlayer(other);
    }

    private void OnHitPlayer(Collider other)
    {
        if(currentHealth > 0)
        {
            if (other.CompareTag("PicoArea"))
            {
                currentHealth -= 10;

                print("Player Got hit by PicoPico");

                StartCoroutine(OnDamage());
            }

            if (other.CompareTag("EnemyBullet"))
            {
                Enemy_Bullet eb = other.GetComponent<Enemy_Bullet>();
                currentHealth -= eb.damage;
                StartCoroutine(OnDamage());
                if (other.GetComponent<Rigidbody>())
                {
                    Destroy(other.gameObject);
                }
            }

            if (other.CompareTag("BossArea"))
            {
                MeleeAttacker a = other.GetComponent<MeleeAttacker>();
                currentHealth -= a.damage;

                print("Player Got hit by Boss");
                isBossAttack = true;

                StartCoroutine(OnDamage());
            }
        }
        

        HPPP.curHP = currentHealth;
    }

    IEnumerator OnDamage()
    {
        print("current player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnDie();
        }


        yield return new WaitForSeconds(0.2f);
        isBossAttack = false;
    }

    private void OnDie()
    {
        StopAllCoroutines();
        WeaponManager.ggboy.ShowUI<EndUi>("lossUI");
        Cursor.lockState = CursorLockMode.None;
        MouseLock.enabled = false;
        p.enabled = false;
        W.enabled = false;
    }
}
