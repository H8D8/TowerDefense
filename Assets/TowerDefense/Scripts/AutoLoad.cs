using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoLoad : MonoBehaviour {

	public int level;
	public float timer;


	// Use this for initialization
	void Start () {

		Invoke ("AutoScene", timer);

	}


	void AutoScene ()
	{
		SceneManager.LoadScene (level);
	}


}
