using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FlyingEnemy : MonoBehaviour {

	public LevelManager levelScript;
	public GameObject damageSmoke, explosionEffect;
	public int enemyCredits = 200;
	public float health = 100f, maxHealth = 100f;
	public Image healthBar;
	public GameObject healthGroup;
	public Transform propeller;


	public Transform waypointGroup;
	public Transform[] waypoints;
	public Transform aimPan, pivotPan;
	public int currentWaypoint;
	public float speed, reachDistance, turnSpeed;


	void Start ()
	{
		healthGroup.SetActive (false);
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelManager> ();
		waypointGroup = GameObject.Find ("WaypointGroup").transform;
		damageSmoke.SetActive (false);

		waypoints = new Transform[waypointGroup.childCount];
		int i = 0;

		foreach(Transform point in waypointGroup)
		{
			waypoints[i] = point;
			i++;
		}



		if (levelScript.waveLevel > 3) {
			health = health + (levelScript.waveLevel * 5);
			maxHealth = health;
		}

	}

	void Update ()
	{
		WaypointNavigation ();
		propeller.Rotate(Vector3.forward, 20f);
		healthBar.fillAmount = health / maxHealth;

	}



	void WaypointNavigation ()
	{
		//used to move the character towards the waypoints via position
		float distance = Vector3.Distance (waypoints [currentWaypoint].position, transform.position);
		transform.position = Vector3.MoveTowards (transform.position, waypoints [currentWaypoint].position, Time.deltaTime * speed);

		//used to rotate the character towards while moving towards waypoints
		aimPan.LookAt (waypoints [currentWaypoint].position);
		pivotPan.rotation = Quaternion.Lerp(pivotPan.rotation, aimPan.rotation, Time.deltaTime * turnSpeed);

		if (distance <= reachDistance) 
			currentWaypoint++;


		if (currentWaypoint >= waypoints.Length) {
			//Debug.Log ("We're reached the end");
			Destroy (gameObject);
		}
	}


	public void TakeDamage(int damageAmount)
	{
		if(health > 0)
		{
			health -= damageAmount;
			healthGroup.SetActive (true);

			if(health <= 0)
			{
				Explode();
				return;
			}
			else if(health/maxHealth <= 0.50f) //health is less than half
			{
				damageSmoke.SetActive(true);

			}
		}
	}


	void Explode ()
	{		
		levelScript.enemyCount--;
		levelScript.creditCount += enemyCredits;
		Instantiate(explosionEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);

	}


}
