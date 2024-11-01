using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Portal : MonoBehaviour {

	public LevelManager levelScript;
	public GameObject portalBlue;
	private AudioSource aud;

	void Start ()
	{
		levelScript = levelScript.GetComponent<LevelManager> ();
		aud = GetComponent<AudioSource> ();
		portalBlue.SetActive (false);
	}


	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Enemy") || col.CompareTag("AirEnemy")) {
			Destroy (col.gameObject);
			levelScript.EnemyPortal (100);
			
			if(levelScript.portalHealth>0)
			aud.Play();

			portalBlue.SetActive (true);
			StartCoroutine(PortalEffect());
			levelScript.enemyCount--;
		}
	}

	IEnumerator PortalEffect()
	{
		yield return new WaitForSeconds(1f);
		portalBlue.SetActive (false);

	}

}
