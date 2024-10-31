using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Turret_Laser : MonoBehaviour {
	
	public int damageAmount = 15;
	public float reloadTime = 1f, turnSpeed = 5f, firePauseTime = .25f, range = 10f;
	private float nextFireTime, nextMoveTime;
	private Quaternion desiredRotation;
	public Transform myTarget,aim_Pan, pivot_Pan;
	public Transform[] muzzlePosition;
	private AudioSource aud;
	private LineRenderer laser;
	public Light muzzleFlash;
	public bool canSee = false;
	private int randomMuzzle;
	private GroundEnemy enemyScript;




	void Start ()
	{
		aud = GetComponent<AudioSource> ();
		laser = GetComponent<LineRenderer> ();
		laser.enabled = false;
		muzzleFlash.enabled = false;
	}

	void Update () 
	{
		TurretAimFire ();
		Tracking ();

	}

	void TurretAimFire()
	{


		if(myTarget){
			if(Time.time >= nextMoveTime){
				laser.SetPosition (0, muzzlePosition[randomMuzzle].position);
				aim_Pan.LookAt (myTarget);
				aim_Pan.eulerAngles = new Vector3 (0, aim_Pan.eulerAngles.y, 0);
				pivot_Pan.rotation = Quaternion.Lerp(pivot_Pan.rotation, aim_Pan.rotation, Time.deltaTime * turnSpeed);

			}



			if (Time.time >= nextFireTime && canSee) 
				FireRay ();

			else
			{
				laser.enabled = false;
				muzzleFlash.enabled = false;
			}

		}


		if (myTarget == null) {
			laser.enabled = false;
			muzzleFlash.enabled = false;
		}

	}


	void  OnTriggerStay (Collider col)
	{
		if(!myTarget){
			if (col.CompareTag ("Enemy")) {
				nextFireTime = Time.time + (reloadTime * 0.5f);
				myTarget = col.gameObject.transform;
			}
		}
	}


	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.transform == myTarget){
			myTarget = null;
		}
	}


	void Tracking ()
	{

		Vector3 fwd = muzzlePosition[randomMuzzle].TransformDirection (Vector3.forward);
		RaycastHit hit;
		Debug.DrawRay (muzzlePosition[randomMuzzle].position, fwd * range, Color.green);

		if (Physics.Raycast (muzzlePosition[randomMuzzle].position, fwd, out hit, range)) {
			if (hit.collider.CompareTag ("Enemy")) {
				canSee = true;
				laser.SetPosition (1, hit.point);
				enemyScript = hit.collider.GetComponent<GroundEnemy> ();

			} 

		}

		else {
			canSee = false;
		}
	}

	void FireRay ()
	{
		randomMuzzle = Random.Range (0, muzzlePosition.Length);
		aud.Play();
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		muzzleFlash.enabled = true;
		laser.enabled = true;
		enemyScript.TakeDamage (damageAmount);


	}
}


