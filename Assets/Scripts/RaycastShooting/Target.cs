using UnityEngine;
using System.Collections.Generic;

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

	private bool boxDead = false;
	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0 && !boxDead)
		{
			boxDead = true;
			float r = 10;
			var cols = Physics.OverlapSphere(transform.position, r);
			var rigidbodies = new List<Target>();
			foreach (var col in cols)
			{
				Target t = col.gameObject.GetComponent<Target>();
				if (t != null && !rigidbodies.Contains(t))
				{
					rigidbodies.Add(t);
				}
			}
			foreach (var t in rigidbodies)
			{
				t.health -= 10 / (t.transform.position - transform.position).magnitude;
			}

			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
