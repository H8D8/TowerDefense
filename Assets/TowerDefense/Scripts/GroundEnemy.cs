using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class GroundEnemy : MonoBehaviour {

	public LevelManager levelScript;
	public GameObject damageSmoke, explosionEffect;
	public int enemyCredits = 50;
	public float health = 100f, maxHealth = 100f;
	public Image healthBar;
	public GameObject healthGroup;

	private UnityEngine.AI.NavMeshAgent navAgent;
	private Transform target;

	// Use this for initialization
	void Start () {

		healthGroup.SetActive (false);
		levelScript = GameObject.Find("LevelManager").GetComponent<LevelManager> ();
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		target = GameObject.Find ("Portal").transform;
		damageSmoke.SetActive (false);

		if (levelScript.waveLevel > 3) {
			health = health + (levelScript.waveLevel * 5);
			maxHealth = health;
		}


	}

	// Update is called once per frame
	void Update () {

		healthBar.fillAmount = health / maxHealth;

		if (target) {
			navAgent.SetDestination (target.position);
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
			else if(health/maxHealth <= 0.50f)
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




