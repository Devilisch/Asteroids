using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class MainMenu : MonoBehaviour {
	public void backToMainMenu() {
		string[] tags = { "Asteroid", "UFO", "Bullet" };

		// clear scene
		foreach (var tag in tags) {
			foreach (var tagObject in GameObject.FindGameObjectsWithTag(tag) ) {
				Destroy( tagObject );
			}
		}

		SceneManager.LoadScene( MENU_SCENE );
	}
}
