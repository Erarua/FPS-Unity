using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scenes.TestFPS.Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, Weapon
    {
        public Transform MuzzlePoint;
        public ParticleSystem MuzzleParticle;
        public GameObject BulletPrefab;

        public Camera Guncamera;
        protected float OriginalFOV;
        protected float Scope0_FOV = 30;
        protected float Scope1_FOV = 15;

        public int bonus;

        public float BulletSpeed;

        public bool reloading;
        protected bool aiming;

        public int AmmoPerMag = 30;
        public int MaxAmmoCarried = 120;

        public int CurrentAmmo;
        public int CurrentMaxAmmoCarried;

        public float FireRate;
        protected float LastFireTime;

        public float AmmoDamage = 9f;

        public Animator GunAnimator;
        protected AnimatorStateInfo GunStateInfo;

        
        protected virtual void Start()
        {
            CurrentAmmo = AmmoPerMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            OriginalFOV = Guncamera.fieldOfView;
        }
        
        public void Attack()
        {
            Shoot();
        }

        public abstract void Shoot();
        public abstract void Reload();
        public abstract void EndReload();
        public abstract void Aim(bool _aiming);

        protected bool AllowShoot()
        {
            return Time.time - LastFireTime > 1 / FireRate;
        }

        protected void CreateBullet()
        {
            GameObject tmp_bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            BulletScript bulletScript = tmp_bullet.GetComponent<BulletScript>();
            bulletScript.BulletDamage = AmmoDamage;
            var tmp_bullet_rigidbody = tmp_bullet.GetComponent<Rigidbody>();
            tmp_bullet_rigidbody.velocity = tmp_bullet.transform.forward * BulletSpeed;
            //Debug.Log("bullet");
        }
    }
}
