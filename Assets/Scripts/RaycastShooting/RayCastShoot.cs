using UnityEngine;
using System.Collections;

public class RayCastShoot : MonoBehaviour
{
	public GunType gun;
	private Camera fpsCamera;
	private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController fpsController;
	private ParticleSystem muzzleFlash;
	public GameObject impactEffect;
	private AudioSource rifleSound;
	private AudioSource reloadStartSound;
	private AudioSource reloadEndSound;

	private GameObject reloadSymbol;
	private GameObject reloadingSymbol;



	private void Start()
	{
		fpsCamera = GetComponentInChildren<Camera>();
		fpsController = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();

		InitializeGun();

		reloadSymbol = fpsCamera.gameObject.transform.Find("Reload Symbol").gameObject;
		reloadSymbol.SetActive(false);
		reloadingSymbol = fpsCamera.gameObject.transform.Find("Reloading Symbol").gameObject;
		reloadingSymbol.SetActive(false);
	}


	private float bulletsLeft = 0;
	private bool isReloading = false;
	private float cooldown = 0;
	private GameObject gunObj;
	public void InitializeGun()
	{
		if (gunObj != null) Destroy(gunObj);

		gunObj = Instantiate(gun.gunPrefab, gun.gunPrefab.transform.position, gun.gunPrefab.transform.rotation);
		gunObj.transform.SetParent(fpsCamera.gameObject.transform, false);

		muzzleFlash = gunObj.transform.Find("MuzzleFlash").gameObject.GetComponent<ParticleSystem>();
		rifleSound = gunObj.transform.Find("MuzzleFlash").gameObject.GetComponent<AudioSource>();
		reloadStartSound = gunObj.transform.Find("ReloadStart").gameObject.GetComponent<AudioSource>();
		reloadEndSound = gunObj.transform.Find("ReloadEnd").gameObject.GetComponent<AudioSource>();
		bulletsLeft = 0;

		cooldown = gun.pickupTime;
		Invoke("Reload", cooldown);
	}

	private void Update()
	{
		if (cooldown > 0) cooldown -= Time.deltaTime;

		if (bulletsLeft == 0 && !isReloading)
		{
			reloadSymbol.SetActive(true);

		}

		if (Input.GetButton("Fire1") && cooldown <= 0 && bulletsLeft > 0)
		{
			// nextTimeToFire = Time.time + (1f / gun.fireRate);
			cooldown = 1 / gun.fireRate;
			Shoot();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (Input.GetButton("Reload") && !isReloading)
		{
			Reload();
		}

		if (Input.GetButton("Zoom"))
		{
			fpsCamera.fieldOfView = Mathf.Lerp(fpsCamera.fieldOfView, gun.cameraFovZoom, 0.1f);
			fpsController.mouseLook.XSensitivity = Mathf.Lerp(fpsController.mouseLook.XSensitivity, gun.mouseSensitivityZoom.x, 0.1f);
			fpsController.mouseLook.YSensitivity = Mathf.Lerp(fpsController.mouseLook.YSensitivity, gun.mouseSensitivityZoom.y, 0.1f);
			if (gun.mouseSmoothZoom && !fpsController.mouseLook.velocityBased)
			{
				fpsController.mouseLook.curr = new Vector2();
			}
			fpsController.mouseLook.velocityBased = gun.mouseSmoothZoom;
		}
		else
		{
			fpsCamera.fieldOfView = Mathf.Lerp(fpsCamera.fieldOfView, gun.cameraFovRegular, 0.1f);
			fpsController.mouseLook.XSensitivity = Mathf.Lerp(fpsController.mouseLook.XSensitivity, gun.mouseSensitivityRegular.x, 0.1f);
			fpsController.mouseLook.YSensitivity = Mathf.Lerp(fpsController.mouseLook.YSensitivity, gun.mouseSensitivityRegular.y, 0.1f);
			if (gun.mouseSmoothRegular && !fpsController.mouseLook.velocityBased)
			{
				fpsController.mouseLook.curr = new Vector2();
			}
			fpsController.mouseLook.velocityBased = gun.mouseSmoothRegular;
		}
	}

	private void Reload()
	{
		isReloading = true;
		reloadStartSound.Play();
		reloadSymbol.SetActive(false);
		reloadingSymbol.SetActive(true);
		Invoke("DoReload", gun.reloadTime);
	}

	void DoReload()
	{
		reloadEndSound.Play();
		bulletsLeft = gun.bulletsInMagazine;
		isReloading = false;
		reloadingSymbol.SetActive(false);
	}

	void Shoot()
	{
		bulletsLeft--;
		muzzleFlash.Play();
		rifleSound.Play();

		RaycastHit hit;

		if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, gun.range))
		{

			Target target = hit.transform.GetComponent<Target>();

			float hitStr = gun.damageFalloff.Evaluate(hit.distance / gun.range);

			if (target != null)
			{
				target.TakeDamage(hitStr * gun.damage);
			}

			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForceAtPosition(fpsCamera.transform.forward * hitStr * gun.impactForce, hit.point, ForceMode.Impulse);
			}

			GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

			Destroy(impactGO, .2f);
		}
	}
}