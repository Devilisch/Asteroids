using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class StartGame : MonoBehaviour
{
	public void startGame() {
		SceneManager.LoadScene(MAIN_SCENE);
	}
}
