using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	public float timer;

	// Use this for initialization
	void Start () {

		Destroy (gameObject, timer);
	
	}

}
