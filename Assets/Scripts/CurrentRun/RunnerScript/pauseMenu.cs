using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CurrentRun {
public class pauseMenu : MonoBehaviour
{
	public string mainMenuLevel;
	public startGameMenu startMenu;
	public GameObject PauseMenu;
	
	
	public void PauseGame () 
	{
		Time.timeScale = 0f;
		PauseMenu.SetActive (true);
		
	}
	
	public void ResumeGame () 
	{
		Time.timeScale = 1f;
		PauseMenu.SetActive (false);
	}
	
	public void RestartGame () 
	{
		Time.timeScale = 1f;
		FindObjectOfType <GameManager>().Reset();
	}
	
	public void QuitToMain () 
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(mainMenuLevel);
	}
}

}