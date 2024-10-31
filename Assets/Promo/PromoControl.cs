using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoControl : MonoBehaviour {

	public Animator anim;
	public int clip;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire2"))
			clip++;

	
		anim.SetInteger ("turret", clip);
		
	}
}
