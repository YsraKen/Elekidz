using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CurrentRun {
public class DeathMenu : MonoBehaviour
{
	public string mainMenuLevel;
	public startGameMenu startMenu;
	
	public void RestartGame () 
	{
		FindObjectOfType <GameManager>().Reset();
	}
	
	public void QuitToMain () 
	{
		SceneManager.LoadScene(mainMenuLevel);
	}
}
}