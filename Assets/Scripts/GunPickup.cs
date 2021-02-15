using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GunPickup : MonoBehaviour
{
	public GunType gun;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			var rcs = other.gameObject.GetComponent<RayCastShoot>();
			if (rcs != null)
			{
				rcs.gun = gun;
				rcs.InitializeGun();
			}
			Destroy(gameObject);
		}
	}
}
