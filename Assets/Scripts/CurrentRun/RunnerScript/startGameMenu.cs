using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CurrentRun {
public class startGameMenu : MonoBehaviour
{
	public string mainMenuLevel;
	
	public void StartGame () 
	{
		FindObjectOfType <GameManager>().StartGame();
		
		//thePlayerController = FindObjectOfType <PlayerController>();
		//thePlayerController.gameObject.SetActive (true);
	}
	
	public void QuitToMain () 
	{
		SceneManager.LoadScene(mainMenuLevel);
	}
}
}