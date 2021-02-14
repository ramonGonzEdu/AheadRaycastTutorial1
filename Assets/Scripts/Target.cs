using UnityEngine;

public class Target : MonoBehaviour
{

	public float health = 50f;
	public GameObject explosion;

	private void Start()
	{

	}

	private void Update()
	{

	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0) { Instantiate(explosion, transform.position, transform.rotation); Destroy(gameObject); }
	}
}
