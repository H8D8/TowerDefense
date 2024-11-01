using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menus : MonoBehaviour {


	public void LoadLevel(int level)
	{
		SceneManager.LoadScene (level);
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}

}
