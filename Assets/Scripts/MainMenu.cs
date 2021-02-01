using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class MainMenu : MonoBehaviour
{
	public void backToMainMenu() {
		SceneManager.LoadScene( MENU_SCENE );
	}
}
