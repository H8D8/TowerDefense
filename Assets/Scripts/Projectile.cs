using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


	public GameObject explosionEffect;
	public Transform myTarget;
	public float range = 10f, speed = 10f;
	public int damageAmount = 25;
	private float dist;
	private FlyingEnemy enemyScript;


	void Update () 
	{
		ProjectileFollow ();
	}

	void ProjectileFollow ()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		dist += Time.deltaTime * speed;
		if(dist >= range)
			Explode();

		if(myTarget)
		{
			transform.LookAt(myTarget);
		}

	}


	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("AirEnemy")) {
			Explode ();			
			col.GetComponent<FlyingEnemy> ().TakeDamage (damageAmount);
		} 

		else
			Explode ();
	}

	void Explode()
	{
		Instantiate(explosionEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
