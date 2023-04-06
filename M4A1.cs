using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scenes.TestFPS.Scripts.Weapon
{
	public class M4A1 : Firearms
	{
		private float current_FOV = 0;
		private float target_FOV;
		private IEnumerator AimCoroutine;
		private MouseLock mouseLock;

		protected override void Start()
		{
			base.Start();
			AmmoDamage = 15f;
			AimCoroutine = DoAim();
			mouseLock = FindObjectOfType<MouseLock>();
		}
		public override void Shoot()
		{
            if (CurrentAmmo <= 0) return;
            if (!AllowShoot()) return;
			if (reloading) return;
            CurrentAmmo--;
			MuzzleParticle.Play();
            GunAnimator.Play("Fire", aiming?1:0, 0);
            CreateBullet();

			mouseLock.FiringForTest();
            LastFireTime = Time.time;
            Debug.Log("Shoot");
		}

		public override void Reload()
		{
			if (CurrentAmmo == AmmoPerMag) return;
			if (CurrentMaxAmmoCarried <= 0) return;

			aiming = false;
            GunAnimator.SetBool("Aim", aiming);
            GunAnimator.SetLayerWeight(2, 1);
			GunAnimator.SetTrigger(CurrentAmmo>0?"ReloadLeft":"ReloadOutOf");
			reloading = true;
		}

        public override void EndReload()
		{
			if (CurrentMaxAmmoCarried < AmmoPerMag - CurrentAmmo)
			{
				CurrentAmmo += CurrentMaxAmmoCarried;
				CurrentMaxAmmoCarried = 0;
			}
			else
			{
				CurrentMaxAmmoCarried -= AmmoPerMag - CurrentAmmo;
				CurrentAmmo = AmmoPerMag;
			}

            GunAnimator.SetLayerWeight(2, 0);
			reloading = false;
        }

		public override void Aim(bool _aiming)
		{
			aiming = _aiming;
			if (reloading) return;
			GunAnimator.SetBool("Aim", aiming);

			if (AimCoroutine == null)
			{
				AimCoroutine = DoAim();
				StartCoroutine(AimCoroutine);
			}
			else
			{
				StopCoroutine(AimCoroutine);
				AimCoroutine = null;
				AimCoroutine = DoAim();
				StartCoroutine(AimCoroutine);
			}
		}


		//private void Update()
		//{
		//	if (Input.GetMouseButton(0))
		//	{
		//		Attack();
		//	}
  //          if (Input.GetKeyDown(KeyCode.R))
  //          {
		//		Reload();
  //          }
		//	if (Input.GetMouseButtonDown(1))
		//	{
		//		//aiming = true;
		//		Aim(true);
		//	}
		//	if (Input.GetMouseButtonUp(1))
		//	{
		//		//aiming = false;
		//		Aim(false);
		//	}
  //      }

		private IEnumerator DoAim()
		{
			while (true)
			{
				yield return null;

				target_FOV = aiming ? Scope0_FOV : OriginalFOV;
                Guncamera.fieldOfView =
					Mathf.SmoothDamp(Guncamera.fieldOfView,
						target_FOV,
						ref current_FOV, Time.deltaTime * 2);
            }
		}
	}
}
