using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Turret_Missile : MonoBehaviour {

	public GameObject myProjectile;
	public float reloadTime = 1f, turnSpeed = 5f, firePauseTime = 0.25f;
	public Transform myTarget;
	public Transform[] muzzlePositions;
	public Transform pivot_Tilt, pivot_Pan, aim_Pan, aim_Tilt;
	private float nextFireTime;
	private AudioSource aud;
	private int randomMuzzle;


	void Start ()
	{
		aud = GetComponent<AudioSource> ();
	}


	void Update () 
	{
		
		AimFire ();

	}

	void AimFire ()
	{
		if(myTarget)
		{
			aim_Pan.LookAt(myTarget);
			aim_Pan.eulerAngles = new Vector3(0, aim_Pan.eulerAngles.y, 0);
			aim_Tilt.LookAt(myTarget);

			pivot_Pan.rotation = Quaternion.Lerp(pivot_Pan.rotation, aim_Pan.rotation, Time.deltaTime * turnSpeed);
			pivot_Tilt.rotation = Quaternion.Lerp(pivot_Tilt.rotation, aim_Tilt.rotation, Time.deltaTime * turnSpeed);

			if(Time.time >= nextFireTime)
			{
				FireProjectile();
			}
		}
	}



	void OnTriggerStay (Collider col)
	{
		if(!myTarget)
		{
			if(col.CompareTag("AirEnemy"))
			{
				nextFireTime = Time.time + (reloadTime * 0.5f);
				myTarget = col.gameObject.transform;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.transform == myTarget)
		{
			myTarget = null;
		}
	}

	void FireProjectile()
	{

		nextFireTime = Time.time + reloadTime;
		randomMuzzle = Random.Range(0,muzzlePositions.Length);
		Instantiate(myProjectile, muzzlePositions[randomMuzzle].position, muzzlePositions[randomMuzzle].rotation);
		aud.Play ();
		myProjectile.GetComponent<Projectile>().myTarget = myTarget;
	}



}
