using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GunType", order = 1)]
public class GunType : ScriptableObject
{
	[Range(0, 100)]
	public float damage = 10;

	[Range(0, 100)]
	public float range = 100;

	[Range(0.001f, 8)]
	public float fireRate = 4;

	[Range(0, 30)]
	public float impactForce = 30;

	[Range(0, 200)]
	public float bulletsInMagazine = 16;

	[Range(0, 60)]
	public float reloadTime = 1;

	[Range(1, 10)]
	public float pickupTime = 1;

	public AnimationCurve damageFalloff = AnimationCurve.Linear(0, 1, 1, 0);
	public float cameraFovRegular = 60;
	public float cameraFovZoom = 10;
	public Vector2 mouseSensitivityRegular = new Vector2(2, 2);
	public Vector2 mouseSensitivityZoom = new Vector2(2f / 6f, 2f / 6f);
	public bool mouseSmoothRegular = false;
	public bool mouseSmoothZoom = true;

	public GameObject gunPrefab;
}